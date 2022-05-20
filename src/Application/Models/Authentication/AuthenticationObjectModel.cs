namespace Application.Models.Authentication;

public class AuthenticationObjectModel
{
    public string Email { get; set; } = null!;
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public string TokenType { get; set; } = null!;
    public DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset RefreshTokenExpiresAt { get; set; }
    public string Scope { get; set; } = null!;
}