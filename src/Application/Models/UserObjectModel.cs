using System.Text.Json;
using Application.Models.Base;
using Domain.Models;

namespace Application.Models;

public class UserObjectModel : ObjectModelBase<UserModel>
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }

    public UserObjectModel()
    {
    }

    public UserObjectModel(UserModel entity)
    {
        AssignFrom(entity);
    }

    public sealed override void AssignFrom(UserModel entity)
    {
        UserId = entity.UserId;
        Email = entity.Email;
        CreatedAt = entity.CreatedAt;
    }

    public override string ToString() => JsonSerializer.Serialize(this);
}