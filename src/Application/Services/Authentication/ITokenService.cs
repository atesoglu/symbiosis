using Application.Models;
using Application.Models.Authentication;

namespace Application.Services.Authentication;

public interface ITokenService
{
    Task<string> BuildAsync(UserObjectModel user, CancellationToken cancellationToken);
    Task<bool> IsTokenValidAsync(JwtObjectModel jwt, CancellationToken cancellationToken);
}