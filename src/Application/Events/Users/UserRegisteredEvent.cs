using System.Text.Json;
using Application.Events.Base;
using Application.Models;

namespace Application.Events.Users;

public class UserRegisteredEvent : Event<UserObjectModel>
{
    public UserRegisteredEvent(UserObjectModel objectModel) : base(objectModel, DateTimeOffset.UtcNow)
    {
    }

    public UserRegisteredEvent(UserObjectModel objectModel, DateTimeOffset occurredAt) : base(objectModel, occurredAt)
    {
    }

    public override string ToString() => JsonSerializer.Serialize(this);
}