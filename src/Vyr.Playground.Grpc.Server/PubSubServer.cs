using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Grpc.Core;
using PublishAndSubcribe;
using static PublishAndSubcribe.PubSub;

namespace Vyr.Playground.Grpc
{
    public class PubSubServer : PubSubBase
    {
        private readonly BufferBlock<Event> buffer = new BufferBlock<Event>();

        private readonly List<Subscription> subscriptions = new List<Subscription>();

        public PubSubServer()
        {
        }


        public override Task<Subscription> Subscribe(Subscription request, ServerCallContext context)
        {
            Console.WriteLine($"{request.ClientId}: Subscribing");

            this.subscriptions.Add(request);

            return Task.FromResult(request);
        }

        public override Task Attach(Subscription request, IServerStreamWriter<Event> responseStream, ServerCallContext context)
        {
            return base.Attach(request, responseStream, context);
        }

        public override Task<Subscription> Unsubscribe(Subscription request, ServerCallContext context)
        {
            return base.Unsubscribe(request, context);
        }

        public override Task<Event> Publish(Event request, ServerCallContext context)
        {
            var configChanged = request.Content.Unpack<ConfigChanged>();

            return Task.FromResult(request);
        }
    }
}
