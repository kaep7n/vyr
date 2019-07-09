using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vyr.Core;

namespace Vyr.Skills.Tests.Fakes
{
    public class DataflowSkillFake : DataflowSkill
    {
        private readonly List<IMessage> processedRequests = new List<IMessage>();
        private readonly string[] topics;
        private readonly Func<IMessage, IMessage> messageProcessor;

        public DataflowSkillFake(Func<IMessage, IMessage> messageProcessor, params string[] topics)
        {
            if (topics is null)
            {
                throw new ArgumentNullException(nameof(topics));
            }

            this.topics = topics;
            this.messageProcessor = messageProcessor;
        }

        public bool HasProcessedAnyMessages()
        {
            return this.processedRequests.Any();
        }

        public int GetProcessedMessagesCount()
        {
            return this.processedRequests.Count;
        }

        public async Task SendAsync(IMessage message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            await this.PublishAsync(message);
        }

        protected override IEnumerable<string> GetTopics()
        {
            return this.topics;
        }

        protected override async Task ProcessAsync(IMessage message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            this.processedRequests.Add(message);

            var responseMessage = this.messageProcessor(message);

            if (responseMessage != null)
            {
                await this.PublishAsync(responseMessage);
            }

            await base.ProcessAsync(message);
        }
    }
}
