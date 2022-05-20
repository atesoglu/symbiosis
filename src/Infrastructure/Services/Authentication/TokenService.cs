using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Models;
using Application.Models.Authentication;
using Application.Services.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.Authentication;

public class TokenService : ITokenService
{
    private readonly JwtOptions _options;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public TokenService(IOptionsMonitor<JwtOptions> settings)
    {
        _options = settings.CurrentValue;
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public async Task<(string, DateTimeOffset)> BuildAsync(UserObjectModel user, CancellationToken cancellationToken)
    {
        var expiresAtUtc = DateTimeOffset.UtcNow.AddMinutes(_options.ExpirationInMinutes);

        var token = await Task.FromResult(_tokenHandler.WriteToken(new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Issuer,
            claims: new[]
            {
                new Claim(ClaimTypes.Actor, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                // new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            },
            expires: expiresAtUtc.UtcDateTime,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)), SecurityAlgorithms.HmacSha256Signature))));

        return (token, expiresAtUtc);
    }

    public async Task<bool> IsTokenValidAsync(JwtObjectModel jwt, CancellationToken cancellationToken)
    {
        try
        {
            _tokenHandler.ValidateToken(jwt.Token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _options.Issuer,
                    ValidAudience = _options.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)),
                }, out var validatedToken);
        }
        catch
        {
            return await Task.FromResult(false);
        }

        return await Task.FromResult(true);
    }
}