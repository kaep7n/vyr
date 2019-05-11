using System;
using Vyr.Isolation;

namespace Vyr.Hosting
{
    public class InProcessHost : IHost
    {
        private readonly Core core;

        public InProcessHost(IIsolationStrategy isolationStrategy, string[] assemblies)
        {
            this.core = new Core(isolationStrategy, assemblies);
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
