using Domain.Models;

namespace Application.Services.Authentication;

public interface IAuthenticator
{
    Task<string> CreateSaltAsync(CancellationToken cancellationToken);
    Task<string> HashPasswordAsync(string password, string salt, CancellationToken cancellationToken);
    Task<bool> ValidatePasswordAsync(UserModel user, string password, CancellationToken cancellationToken);
}