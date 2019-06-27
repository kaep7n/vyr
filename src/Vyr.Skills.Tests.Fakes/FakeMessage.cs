using System;
using Vyr.Core;

namespace Vyr.Skills.Tests.Fakes
{
    public class FakeMessage : IMessage
    {
        public FakeMessage()
        {
            this.Id = new Id();
        }

        public string Id { get; }

        public DateTime CreatedAt { get; }
    }
}
