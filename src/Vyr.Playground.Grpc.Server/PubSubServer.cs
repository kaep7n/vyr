using System;
using System.Collections.Generic;
using System.Text;
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
        private readonly Timer timer = new Timer();
        private readonly BufferBlock<Event> buffer = new BufferBlock<Event>();
        private readonly Dictionary<string, IServerStreamWriter<Event>> subscriberWritersMap = new Dictionary<string, IServerStreamWriter<Event>>();

        public PubSubServer()
        {
            this.timer.Interval = 1000;
            this.timer.Elapsed += this.Timer_Elapsed;
            this.timer.Start();
        }

        public override async Task Subscribe(Subscription subscription, IServerStreamWriter<Event> responseStream, ServerCallContext context)
        {
            this.subscriberWritersMap[subscription.Id] = responseStream;

            while (this.subscriberWritersMap.ContainsKey(subscription.Id))
            {
                var @event = await this.buffer.ReceiveAsync();

                foreach (var serverStreamWriter in this.subscriberWritersMap.Values)
                {
                    await serverStreamWriter.WriteAsync(@event);
                }
            }
        }

        public override Task<Subscription> Unsubscribe(Subscription request, ServerCallContext context)
        {
            this.subscriberWritersMap.Remove(request.Id);

            return Task.FromResult(request);
        }


        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.buffer.SendAsync(new Event { Topic = "config", Value = "config has changed" });
        }
    }
}
