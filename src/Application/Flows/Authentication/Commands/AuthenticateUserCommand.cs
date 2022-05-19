using System.Text.Json;
using Application.Models;
using Application.Request;

namespace Application.Flows.Authentication.Commands
{
    public class AuthenticateUserCommand : Request<UserObjectModel>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}