using System;
using Vyr.Core;

namespace Vyr.Skills.Tests.Fakes
{
    public class FakeMessage : IMessage
    {
        public FakeMessage(string topic)
        {
            this.Id = new Id();
            this.Topic = topic;
            this.CreatedAt = DateTime.UtcNow;
        }

        public string Id { get; }

        public string Topic { get; }

        public DateTime CreatedAt { get; }
    }
}
