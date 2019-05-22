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

        private readonly ConcurrentDictionary<string,Subscription> subscriptions = new ConcurrentDictionary<string, Subscription>();

        public PubSubServer()
        {
        }

        public override Task<Subscription> Subscribe(Subscription request, ServerCallContext context)
        {
            Console.WriteLine($"{request.ClientId}: Subscribing");

            this.subscriptions.TryAdd(request.ClientId, request);

            return Task.FromResult(request);
        }

        public override async Task Attach(Subscription request, IServerStreamWriter<Event> responseStream, ServerCallContext context)
        {
            while (this.subscriptions.TryGetValue(request.ClientId, out var subscription))
            {
                var @event = await this.buffer.ReceiveAsync();

                if (subscription.Topics.Contains(@event.Topic))
                {
                    await responseStream.WriteAsync(@event);
                }
            }
        }

        public override Task<Subscription> Unsubscribe(Subscription request, ServerCallContext context)
        {
            return base.Unsubscribe(request, context);
        }

        public async override Task<Event> Publish(Event request, ServerCallContext context)
        {
            await this.buffer.SendAsync(request);

            return request;
        }
    }
}
