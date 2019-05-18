using Pubsub;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vyr.Playground.Grpc
{
    partial class Program
    {
        public class Subscriber
        {
            private readonly PubSub.PubSubClient pubSubClient;
            private Subscription subscription;

            public Subscriber(PubSub.PubSubClient pubSubClient)
            {
                this.pubSubClient = pubSubClient;
            }

            public void Subscribe()
            {
                this.subscription = new Subscription() { Id = Guid.NewGuid().ToString() };

                Task.Run(async () =>
                {
                    using var call = this.pubSubClient.Subscribe(this.subscription);
                
                     while (true)
                     {
                         Console.WriteLine($"{this.subscription.Id}: waiting for data");

                         if (!await call.ResponseStream.MoveNext())
                         {
                             Console.WriteLine($"{this.subscription.Id}: no data available");
                             continue;
                         }

                         Console.WriteLine($"{this.subscription.Id}: Event received: " + call.ResponseStream.Current);
                     }
                 });
            }

            public void Publish()
            {
                this.pubSubClient.Publish(new Event { Topic = "config", Value = $"Message from {this.subscription.Id}" });
            }

            public void Unsubscribe()
            {
                this.pubSubClient.Unsubscribe(this.subscription);
            }
        }
    }
}
