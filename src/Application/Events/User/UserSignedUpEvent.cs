using System.Text.Json;
using Application.Events.Base;
using Application.Models;

namespace Application.Events.User;

public class UserSignedUpEvent : Event<UserObjectModel>
{
    public UserSignedUpEvent(UserObjectModel objectModel) : base(objectModel, DateTimeOffset.UtcNow)
    {
    }

    public UserSignedUpEvent(UserObjectModel objectModel, DateTimeOffset occurredAt) : base(objectModel, occurredAt)
    {
    }

    public override string ToString() => JsonSerializer.Serialize(this);
}