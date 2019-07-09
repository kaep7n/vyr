using System;
using Vyr.Core;

namespace Vyr.Skills.Tests.Fakes
{
    public class FakeMessage : IMessage
    {
        public FakeMessage()
        {
            this.Id = new Id();
            this.Topic = "Test";
        }

        public string Id { get; }

        public string Topic { get; }

        public DateTime CreatedAt { get; }
    }
}
