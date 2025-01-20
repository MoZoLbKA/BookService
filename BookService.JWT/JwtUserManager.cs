using BookService.Domain.Entities.Users.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookService.JwtAuth;

public interface IJwtUserManager
{
    bool IsAuthorised();
    int GetUserId();
    string? GetUserName();
    UserRole? GetUserRole();
}

public sealed class JwtUserManager : IJwtUserManager
{
    private readonly IHttpContextAccessor _contextAccessor;
    public JwtUserManager(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public bool IsAuthorised()
        => GetUserId() != default;

    public int GetUserId()
    {
        string? value = _contextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
        if (!int.TryParse(value, out int id))
        {
            return 0;
        }
        return id;
    }
    public string? GetUserName()
        => _contextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
    public UserRole? GetUserRole()
    {
        string? value = _contextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
        if (!int.TryParse(value, out int intRole))
        {
            return null;
        }
        UserRole roleValue = (UserRole)intRole;
        if (Enum.GetName(roleValue) == null)
        {
            return null;
        }
        return roleValue;
    }
}
