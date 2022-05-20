using System.Security.Cryptography;
using Application.Models.Authentication;
using Application.Services.Authentication;
using Domain.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Authentication;

public class Authenticator : IAuthenticator
{
    private static readonly RandomNumberGenerator BytesGenerator = RandomNumberGenerator.Create();

    private readonly JwtOptions _jwtOptions;

    public Authenticator(IOptionsMonitor<JwtOptions> jwtSettings)
    {
        _jwtOptions = jwtSettings.CurrentValue;
    }

    public async Task<string> CreateSaltAsync(CancellationToken cancellationToken)
    {
        var bytes = new byte[128 / 8];
        BytesGenerator.GetNonZeroBytes(bytes);

        return await Task.FromResult(Convert.ToBase64String(bytes));
    }

    public async Task<string> HashPasswordAsync(string password, string salt, CancellationToken cancellationToken)
    {
        var salted = await HashAsync(password, salt);
        var saltedAndSpiced = await HashAsync(salted, _jwtOptions.Pepper);

        return saltedAndSpiced;
    }

    public async Task<bool> ValidatePasswordAsync(UserModel user, string password, CancellationToken cancellationToken)
    {
        var hashed = await HashPasswordAsync(password, user.PasswordSalt, cancellationToken);
        return await Task.FromResult(user.PasswordHash == hashed);
    }

    private static async Task<string> HashAsync(string toBeHashed, string base64Salt)
    {
        var bytes = new byte[128 / 8];
        BytesGenerator.GetNonZeroBytes(bytes);

        var salt = Convert.FromBase64String(base64Salt);
        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(password: toBeHashed, salt: salt, prf: KeyDerivationPrf.HMACSHA256, iterationCount: 10000, numBytesRequested: 256 / 8));

        return await Task.FromResult(hashed);
    }
}