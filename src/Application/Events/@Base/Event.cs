using System;
using System.Text.Json;
using Application.Models.Base;

namespace Application.Events.Base
{
    public abstract class Event<T> : EventBase
        where T : ObjectModelBase
    {
        public T Model { get; }

        protected Event(T objectModel, DateTimeOffset occurredAt) : base(occurredAt)
        {
            Model = objectModel;
        }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}