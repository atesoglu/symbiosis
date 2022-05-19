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
    private readonly JwtSettings _settings;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public TokenService(IOptionsMonitor<JwtSettings> settings)
    {
        _settings = settings.CurrentValue;
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public async Task<string> BuildAsync(UserObjectModel user, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_tokenHandler.WriteToken(new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Issuer,
            claims: new[]
            {
                new Claim(ClaimTypes.Actor, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            },
            expires: DateTime.Now.AddMinutes(20),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key)), SecurityAlgorithms.HmacSha256Signature))));
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
                    ValidIssuer = _settings.Issuer,
                    ValidAudience = _settings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key)),
                }, out var validatedToken);
        }
        catch
        {
            return await Task.FromResult(false);
        }

        return await Task.FromResult(true);
    }
}