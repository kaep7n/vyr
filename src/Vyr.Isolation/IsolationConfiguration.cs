using System;

namespace Vyr.Isolation
{
    public class IsolationConfiguration
    {
        public IsolationConfiguration(AgentConfiguration agentConfiguration)
        {
            if (agentConfiguration is null)
            {
                throw new ArgumentNullException(nameof(agentConfiguration));
            }

            this.AgentConfiguration = agentConfiguration;
        }

        public AgentConfiguration AgentConfiguration { get; }
    }
}
