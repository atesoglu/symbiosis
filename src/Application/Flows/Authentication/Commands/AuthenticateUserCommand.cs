using System.Text.Json;
using Application.Models;
using Application.Models.Authentication;
using Application.Request;

namespace Application.Flows.Authentication.Commands;

public class AuthenticateUserCommand : Request<AuthenticationObjectModel>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    public override string ToString() => JsonSerializer.Serialize(this);
}