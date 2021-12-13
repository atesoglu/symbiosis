using System.Text.Json;
using Application.Models;
using Application.Request;

namespace Application.Flows.Users.Queries
{
    public class FindUserByEmailCommand : Request<UserObjectModel>
    {
        public string Email { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}