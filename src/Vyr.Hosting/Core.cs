using System;
using System.Collections.Generic;
using Vyr.Isolation;

namespace Vyr.Hosting
{
    public class Core
    {
        private readonly IIsolationStrategy isolationStrategy;
        private readonly AgentDescription[] agentDescriptions;
        private readonly List<object> agents = new List<object>();
        private readonly List<IIsolation> isolations = new List<IIsolation>();

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
                this.isolations.Add(isolation);

                var agent = isolation.Isolate(agentDescription);

                var runMethod = agent.GetType().GetMethod("RunAsync");
                runMethod.Invoke(agent, new object[] { });

                this.agents.Add(agent);
            }
        }

        public void Stop()
        {
            foreach (var isolation in this.isolations)
            {
                isolation.Free();
            }

            this.agents.Clear();
        }
    }
}
