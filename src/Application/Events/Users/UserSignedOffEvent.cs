using System.Text.Json;
using Application.Events.Base;
using Application.Models;

namespace Application.Events.Users;

public class UserSignedOffEvent : Event<UserObjectModel>
{
    public UserSignedOffEvent(UserObjectModel objectModel) : base(objectModel, DateTimeOffset.UtcNow)
    {
    }

    public UserSignedOffEvent(UserObjectModel objectModel, DateTimeOffset occurredAt) : base(objectModel, occurredAt)
    {
    }

    public override string ToString() => JsonSerializer.Serialize(this);
}