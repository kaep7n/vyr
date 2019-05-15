using System;
using System.Collections.Generic;
using Vyr.Agents;
using Vyr.Isolation;

namespace Vyr.Hosting
{
    public class Core
    {
        private readonly IIsolationStrategy isolationStrategy;
        private readonly AgentDescription[] agentDescriptions;
        private readonly List<object> agents = new List<object>();

        public Core(IIsolationStrategy isolationStrategy, AgentDescription[] agentDescriptions)
        {
            if (isolationStrategy == null)
            {
                throw new ArgumentNullException(nameof(isolationStrategy));
            }

            this.isolationStrategy = isolationStrategy;
            this.agentDescriptions = agentDescriptions;
        }

        public void Start()
        {
            foreach (var agentDescription in this.agentDescriptions)
            {
                var isolation = this.isolationStrategy.Create();

                var agent = isolation.Isolate(agentDescription);

                this.agents.Add(agent);
            }
        }

        public void Stop()
        {
            this.agents.Clear();
        }
    }
}
