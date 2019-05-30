using Vyr.Isolation;

namespace Vyr.Hosting
{
    public class InProcessHost : IHost
    {
        private readonly Core core;

        public InProcessHost(IIsolationStrategy isolationStrategy, AgentDescription[] agentDescriptions)
        {
            this.core = new Core(isolationStrategy, agentDescriptions);
        }

        public void Up()
        {
            this.core.Start();
        }

        public void Down()
        {
            this.core.Stop();
        }
    }
}
