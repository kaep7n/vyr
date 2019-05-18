using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Timers;
using Grpc.Core;
using Pubsub;
using static Pubsub.PubSub;

namespace Vyr.Playground.Grpc
{
    public class PubSubServer : PubSubBase
    {
        private readonly BufferBlock<Event> buffer = new BufferBlock<Event>();
        private readonly ConcurrentDictionary<string, IServerStreamWriter<Event>> subscriberWritersMap = new ConcurrentDictionary<string, IServerStreamWriter<Event>>();

        public PubSubServer()
        {
        }

        public override async Task Subscribe(Subscription subscription, IServerStreamWriter<Event> responseStream, ServerCallContext context)
        {
            Console.WriteLine($"{subscription.Id}: Subscribing");

            this.subscriberWritersMap[subscription.Id] = responseStream;

            while (this.subscriberWritersMap.ContainsKey(subscription.Id))
            {
                Console.WriteLine($"{subscription.Id}: Check events for subscriber");

                var @event = await this.buffer.ReceiveAsync();
                Console.WriteLine($"{subscription.Id}: Received event for topic {@event.Topic} from buffer ");

                foreach (var serverStreamWriter in this.subscriberWritersMap.Values)
                {
                    Console.WriteLine($"{subscription.Id}: Write event to stream");
                    await serverStreamWriter.WriteAsync(@event);
                    Console.WriteLine($"{subscription.Id}: Wrote event to stream");
                }
            }
        }

        public override async Task<Event> Publish(Event request, ServerCallContext context)
        {
            await this.buffer.SendAsync(request);

            return request;
        }

        public override Task<Subscription> Unsubscribe(Subscription request, ServerCallContext context)
        {
            this.subscriberWritersMap.TryRemove(request.Id, out _);

            return Task.FromResult(request);
        }
    }
}
