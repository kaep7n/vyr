using System;

namespace Vyr.Agents.Tests.Fakes
{
    public class FakeMessage : IMessage
    {
        public FakeMessage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; private set; }
    }
}
