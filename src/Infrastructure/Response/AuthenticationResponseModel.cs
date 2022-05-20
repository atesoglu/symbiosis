using System;
using System.Text.Json.Serialization;
using Application.Models.Authentication;

namespace Infrastructure.Response;

/// <summary>
/// Generic authentication response model to be serialized as json response to http requests.
/// </summary>
public class AuthenticationResponseModel
{
    /// <summary>
    /// Authenticated user's email address.
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Access token to be used in subsequent request headers.
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = null!;

    /// <summary>
    /// Refresh token to be used to refresh tokens.
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; } = null!;

    /// <summary>
    /// Token type: Bearer
    /// </summary>
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    /// <summary>
    /// Total seconds which access token get expires  
    /// </summary>
    [JsonPropertyName("expires_at")]
    public DateTimeOffset ExpiresAt { get; set; }

    /// <summary>
    /// Refresh token's expiration date time against the UTC timezone.
    /// </summary>
    [JsonPropertyName("refresh_token_expires_at")]
    public DateTimeOffset RefreshTokenExpiresAt { get; set; }

    /// <summary>
    /// Authentication scope.
    /// </summary>
    [JsonPropertyName("scope")]
    public string Scope { get; set; }

    public AuthenticationResponseModel()
    {
        TokenType = "Bearer";
        Scope = "api";
    }

    public AuthenticationResponseModel(AuthenticationObjectModel model)
    {
        TokenType = "Bearer";
        Scope = "api";

        Email = model.Email;
        AccessToken = model.AccessToken;
        RefreshToken = model.RefreshToken;
        ExpiresAt = model.ExpiresAt;
        RefreshTokenExpiresAt = model.RefreshTokenExpiresAt;
    }
}