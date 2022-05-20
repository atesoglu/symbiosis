namespace Application.Models.Authentication;

public class JwtOptions
{
    public string Key { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int ExpirationInMinutes { get; set; }
    public string Pepper { get; set; } = null!;

    public JwtOptions()
    {
        ExpirationInMinutes = 20;
    }
}