using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vyr.Agents;
using Vyr.Core;
using Vyr.Skills;

namespace Vyr.Playground.Agents
{
    public class TestAgent : DataflowAgent
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<TestAgent> logger;

        public TestAgent(IEnumerable<ISkill> skills, IConfiguration configuration, ILogger<TestAgent> logger) 
            : base(skills)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.configuration = configuration;
            this.logger = logger;
        }

        public override Task RunAsync()
        {
            this.logger.LogInformation($"running test agent with name {this.configuration["name"]}");
            return base.RunAsync();
        }

        public override Task IdleAsync()
        {
            this.logger.LogInformation("idling test agent");
            return base.IdleAsync();
        }

        protected override void ProcessMessage(IMessage message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            this.logger.LogInformation($"processing message {message.Id}");
            base.ProcessMessage(message);
        }
    }
}
