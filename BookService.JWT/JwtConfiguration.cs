namespace BookService.JwtAuth;
public sealed class JwtConfiguration
{
    public string Key { get; set; } = null!;
    public int Minutes { get; set; }
    public int RefreshMinutes { get; set; }
    public string Redirect { get; set; } = null!;
}
