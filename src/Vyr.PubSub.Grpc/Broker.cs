using Grpc.Core;
using PubSub;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vyr.Core;
using Vyr.Playground.Grpc;
using Vyr.Skills;

namespace Vyr.PubSub.Grpc
{
    public class Broker : DataflowSkill
    {
        private readonly Server server;

        public Broker()
        {
            this.server = new Server
            {
                Services = { BrokerService.BindService(new BrokerServer()) },
                Ports = { new ServerPort("localhost", 60000, ServerCredentials.Insecure) }
            };
        }

        public override Task EnableAsync()
        {
            this.server.Start();

            return base.EnableAsync();
        }

        public override async Task DisableAsync()
        {
            await this.server.ShutdownAsync();
            await base.DisableAsync();
        }

        protected override IEnumerable<string> GetTopics()
        {
            return base.GetTopics();
        }

        protected override Task ProcessAsync(IMessage message)
        {
            return base.ProcessAsync(message);
        }
    }
}
