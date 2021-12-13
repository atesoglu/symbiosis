using System;

namespace Application.Events.Base
{
    public abstract class EventBase
    {
        public bool IsPublished { get; set; }
        public DateTimeOffset OccurredAt { get; }

        protected EventBase()
        {
            OccurredAt = DateTimeOffset.Now;
        }

        protected EventBase(DateTimeOffset occurredAt)
        {
            OccurredAt = occurredAt;
        }
    }
}