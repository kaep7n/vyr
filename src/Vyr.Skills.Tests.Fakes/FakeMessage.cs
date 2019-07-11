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
            this.CreatedAtUtc = DateTime.UtcNow;
        }

        public string Id { get; }

        public string Topic { get; }

        public DateTime CreatedAtUtc { get; }
    }
}
