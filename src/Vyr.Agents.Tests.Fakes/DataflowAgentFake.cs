using System.Collections.Generic;
using System.Linq;
using Vyr.Skills;

namespace Vyr.Agents.Tests.Fakes
{
    public class DataflowAgentFake : DataflowAgent
    {
        private readonly List<IMessage> processedMessages = new List<IMessage>();

        public DataflowAgentFake(IEnumerable<ISkill> skills)
            : base(skills)
        {
        }

        public bool HasProcessedAnyMessages()
        {
            return this.processedMessages.Any();
        }

        public int GetProcessedMessagesCount()
        {
            return this.processedMessages.Count;
        }

        protected override void ProcessMessage(IMessage message)
        {
            this.processedMessages.Add(message);
        }

        protected override void ProcessResponse(IResponse response)
        {
        }
    }
}
