using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using BookService.Domain.Entities.Users.Enums;

namespace BookService.JwtAuth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeJWTAttribute : Attribute, IAuthorizationFilter
{
    public static JwtConfiguration Configuration = null!;
    private readonly UserRole[] _roles;
    private readonly bool _redirect;
    public AuthorizeJWTAttribute() : this(Array.Empty<UserRole>()) { }

    public AuthorizeJWTAttribute(params UserRole[] roles)
    {
        _roles = roles;
        _redirect = false;
    }

    public AuthorizeJWTAttribute(bool redirect, params UserRole[] roles)
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

        Claim? stringRoleIdClaim = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "role");
        if (stringRoleIdClaim == null)
        {
            context.Result = GetUnauthorizedResult(context);
            return;
        }
        if (!int.TryParse(stringRoleIdClaim.Value, out int roleInt))
        {
            context.Result = GetUnauthorizedResult(context);
            return;
        }
        UserRole role = (UserRole)roleInt;
        if (Enum.GetName(role) == null)
        {
            context.Result = GetUnauthorizedResult(context);
            return;
        }
        if (_roles.Length == 0 || role == UserRole.Admin)
        {
            return;
        }
        if (!_roles.Contains(role))
        {
            context.Result = GetUnauthorizedResult(context);
        }
    }

    private ActionResult GetUnauthorizedResult(AuthorizationFilterContext context)
    {
        context.HttpContext.Response.Cookies.Delete("token");
        context.HttpContext.Response.Cookies.Delete("refresh-token");
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
