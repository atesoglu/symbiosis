using System.Text.Json;
using Domain.Models.Base;

namespace Domain.Models
{
    public class UserModel : ModelBase
    {
        public int UserId { get; set; }
        public string Email { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}