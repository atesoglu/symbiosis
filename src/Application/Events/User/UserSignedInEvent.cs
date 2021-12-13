using System.Text.Json;
using Application.Events.Base;
using Application.Models;

namespace Application.Events.User
{
    public class UserSignedInEvent : Event<UserObjectModel>
    {
        public UserSignedInEvent(UserObjectModel objectModel) : base(objectModel, DateTimeOffset.UtcNow)
        {
        }

        public UserSignedInEvent(UserObjectModel objectModel, DateTimeOffset occurredAt) : base(objectModel, occurredAt)
        {
        }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}