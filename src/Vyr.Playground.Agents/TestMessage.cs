using System;
using Vyr.Core;

namespace Vyr.Playground.Agents
{
    public class TestMessage : IMessage
    {
        public TestMessage()
        {
            this.Id = new Id();
            this.Topic = "/";
            this.CreatedAtUtc = DateTime.UtcNow;
        }

        public string Id { get; }

        public string Topic { get; }

        public DateTime CreatedAtUtc { get; }
    }
}
