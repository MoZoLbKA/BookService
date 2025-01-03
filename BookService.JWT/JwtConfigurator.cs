using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookService.JwtAuth;

public static class JWTConfigurator
{
    public static void Configure(WebApplicationBuilder builder)
    {
        JwtConfiguration config = new JwtConfiguration();
        var mainSection = builder.Configuration.GetSection("JWT");
        string finalSectionName = builder.Environment.EnvironmentName switch
        {
            "DevServer" => "DevServer",
            "Production" => "Deploy",
            _ => "Default",
        };
        mainSection.GetChildren().FirstOrDefault(x => x.Key == finalSectionName)?.Bind(config);
        builder.Services.AddSingleton(config);
        AuthorizeJWTAttribute.Configuration = config;
        AuthorizeJwtCurrentAttribute.Configuration = config;
        builder.Services.AddScoped<IJwtGeneratorService, JwtGeneratorService>();
        builder.Services.AddScoped<IJwtValidatorService, JwtValidatorService>();
        builder.Services.AddScoped<IExpiredJwtValidatorService, ExpiredJwtValidatorService>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IJwtUserManager, JwtUserManager>();
    }

    public static void Configure(WebApplication application)
    {
        application.UseMiddleware<JwtMiddleware>();
    }
}
