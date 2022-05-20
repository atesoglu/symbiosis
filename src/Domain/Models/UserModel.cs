using System.Text.Json;
using Domain.Models.Base;

namespace Domain.Models;

public class UserModel : ModelBase
{
    public int UserId { get; set; }
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string PasswordSalt { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }

    public UserModel()
    {
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public override string ToString() => JsonSerializer.Serialize(this);
}