using JWT.Algorithms;
using JWT.Builder;
using System.Text.Json;

namespace BookService.JwtAuth;

public interface IJwtValidatorService
{
    IDictionary<string, JsonElement>? ValidateJwtToken(string? token);
}

public sealed class JwtValidatorService : IJwtValidatorService
{
    private readonly JwtBuilder _builder;
    private readonly JwtConfiguration _config;
    public JwtValidatorService(JwtConfiguration config)
    {
        _config = config;
        _builder = JwtBuilder
            .Create()
            .WithAlgorithm(new HMACSHA256Algorithm())
            .WithSecret(new[] { _config.Key })
            .MustVerifySignature();
    }
    public IDictionary<string, JsonElement>? ValidateJwtToken(string? token)
    {
        if (token == null)
        {
            return null;
        }
        try
        {
            var data = _builder.Decode<IDictionary<string, JsonElement>>(token);
            return data;
        }
        catch
        {
            return null;
        }
    }
}
