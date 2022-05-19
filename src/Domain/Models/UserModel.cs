using System.Text.Json;
using Domain.Models.Base;

namespace Domain.Models
{
    public class UserModel : ModelBase
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}