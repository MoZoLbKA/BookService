using JWT.Algorithms;
using JWT.Builder;

namespace BookService.JwtAuth;

public interface IJwtGeneratorService
{
    string Generate(IEnumerable<KeyValuePair<string, object>> claims);
}

public sealed class JwtGeneratorService : IJwtGeneratorService
{
    private JwtBuilder _builder;
    private JwtConfiguration _config;
    public JwtGeneratorService(JwtConfiguration config)
    {
        _config = config;
        _builder = JwtBuilder.Create()
                      .WithAlgorithm(new HMACSHA256Algorithm())
                      .WithSecret(new[] { _config.Key })
                      .AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(_config.Minutes).ToUnixTimeSeconds());
    }
    public string Generate(IEnumerable<KeyValuePair<string, object>> claims)
        => _builder.AddClaims(claims).Encode();
}
