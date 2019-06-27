using System;
using Vyr.Core;

namespace Vyr.Skills.Tests.Fakes
{
    public class FakeResultMessage : IMessage
    {
        public FakeResultMessage(IMessage sourceMessage)
        {
            if (sourceMessage is null)
            {
                throw new ArgumentNullException(nameof(sourceMessage));
            }

            this.Id = new Id();
            this.CreatedAt = DateTime.UtcNow;
            this.SourceMessage = sourceMessage;
        }

        public string Id { get; }

        public IMessage SourceMessage { get; }

        public DateTime CreatedAt { get; }
    }
}
