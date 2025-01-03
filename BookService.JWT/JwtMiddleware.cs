using BookService.Infrastructure.Persistence.UnitOfWorks.Custom;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;

namespace BookService.JwtAuth;

public sealed class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _provider;
    public JwtMiddleware(RequestDelegate next, IServiceProvider provider)
    {
        _next = next;
        _provider = provider;
    }

    public async Task InvokeAsync
        (HttpContext context,
        IJwtGeneratorService jwtGenerator,
        IJwtValidatorService jwtValidator,
        IExpiredJwtValidatorService expiredJwtValidator,
        JwtConfiguration jwtConfig)
    {
        string? refreshToken = null;
        if (context.Request.Cookies.TryGetValue("refresh-token", out string? refreshCookieValue) && refreshCookieValue != null)
        {
            refreshToken = refreshCookieValue;
        }
        else
        {
            Console.WriteLine("refresh-token not found!!!");
            Debug.WriteLine("refresh-token not found!!!");
            await _next(context);
            return;
        }
        string? rawJwtToken = null;
        if (context.Request.Headers.TryGetValue("Authorization", out StringValues authHeaderValues))
        {
            rawJwtToken = authHeaderValues.ToString();
        }
        else if (context.Request.Cookies.TryGetValue("token", out string? cookieValue) && cookieValue != null)
        {
            rawJwtToken = $"Bearer {cookieValue}";
        }
        if (string.IsNullOrEmpty(rawJwtToken) || !rawJwtToken.Contains("Bearer"))
        {
            Console.WriteLine("token not found!!!");
            Debug.WriteLine("token not found!!!");
            await _next(context);
            return;
        }
        var token = rawJwtToken.Split(' ')[1];
        bool expired = false;
        IDictionary<string, JsonElement>? jwtClaims = jwtValidator.ValidateJwtToken(token);
        if (jwtClaims == null)
        {
            jwtClaims = expiredJwtValidator.ValidateJwtToken(token);
            expired = true;
            if (jwtClaims == null)
            {
                Console.WriteLine("claims double invalid!!!");
                Debug.WriteLine("claims double invalid!!!");
                await _next(context);
                return;
            }
        }
        if (!jwtClaims.TryGetValue("id", out var element) || !int.TryParse(element.GetString(), out int id))
        {
            Console.WriteLine("id parse error!!!");
            Debug.WriteLine("id parse error!!!");
            await _next(context);
            return;
        }
        using (var scope = _provider.CreateScope())
        using (var unitOfWork = scope.ServiceProvider.GetRequiredService<IUserUnitOfWork>())
        {
            var user = await unitOfWork.FindAsync(id);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshExpiryTime == null || user.RefreshExpiryTime < DateTime.UtcNow)
            {
                Console.WriteLine("user or refresh err!!!");
                Debug.WriteLine("user or refresh err!!!");
                await _next(context);
                return;
            }
            if (expired)
            {
                var expireTime = DateTime.UtcNow.AddMinutes(jwtConfig.RefreshMinutes);
                user.RefreshExpiryTime = expireTime;
                unitOfWork.Update(user);
                await unitOfWork.SaveAsync();
                var jwt = jwtGenerator.Generate(user.GetGeneratorClaims());
                context.Response.Cookies.Append("token", jwt, new CookieOptions()
                {
                    Expires = expireTime
                });
                context.Response.Cookies.Append("refresh-token", refreshToken, new CookieOptions()
                {
                    Expires = expireTime,
                    HttpOnly = true
                });
            }
            var identities = new ClaimsIdentity(user.GetClaims());
            context.User = new ClaimsPrincipal(new[] { identities });
        }
        await _next(context);
    }
}
