using BookService.Domain.Entities.Users.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace BookService.JwtAuth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeJwtCurrentAttribute : Attribute, IAuthorizationFilter
{
    public static JwtConfiguration Configuration = null!;
    private readonly UserRole[] _roles;
    private readonly bool _redirect;
    public AuthorizeJwtCurrentAttribute() : this([]) { }

    public AuthorizeJwtCurrentAttribute(params UserRole[] roles)
    {
        _roles = roles;
        _redirect = false;
    }

    public AuthorizeJwtCurrentAttribute(bool redirect, params UserRole[] roles)
    {
        _roles = roles;
        _redirect = redirect;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
        {
            return;
        }

        var unauthorised = GetUnauthorizedResult;
        var userManager = context.HttpContext.RequestServices.GetRequiredService<IJwtUserManager>();
        if (!userManager.IsAuthorised())
        {
            context.Result = unauthorised.Invoke(context.HttpContext);
            return;
        }
    }

    private ActionResult GetUnauthorizedResult(HttpContext context)
    {
        context.Response.Cookies.Delete("token");
        context.Response.Cookies.Delete("refresh-token");
        if (_redirect)
        {
            return new RedirectResult(Configuration.Redirect);
        }
        else
        {
            return new JsonResult("Unauthorized") { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}
