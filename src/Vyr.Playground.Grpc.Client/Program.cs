using Grpc.Core;
using Pubsub;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Vyr.Playground.Grpc
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var subs = new List<Subscriber>();

            for (int i = 0; i < 5; i++)
            {
                subs.Add(SubscribeToTopic());
            }

            //subs[0].Publish();

            Console.ReadLine();
        }

        private static Subscriber SubscribeToTopic()
        {
            var channel = new Channel("127.0.0.1:50052", ChannelCredentials.Insecure);
            var client = new Subscriber(new PubSub.PubSubClient(channel));

            client.Subscribe();

            return client;
        }

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

                using var call = this.pubSubClient.Subscribe(this.subscription);
                
                //Receive
                _ = Task.Run(async () =>
                 {
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
