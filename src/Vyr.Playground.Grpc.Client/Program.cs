using Grpc.Core;
using Pubsub;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vyr.Playground.Grpc
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var tasks = new List<Task>();

            for (int i = 0; i < 2; i++)
            {
                tasks.Add(SubscribeToTopic("config"));
            }

            await Task.WhenAll(tasks);
        }

        private static async Task SubscribeToTopic(string topic)
        {
            Console.WriteLine("Creating channel");
            var channel = new Channel("127.0.0.1:50052", ChannelCredentials.Insecure);
            Console.WriteLine("Creating client");
            var client = new Subscriber(new PubSub.PubSubClient(channel));

            Console.WriteLine("Subscribing to topic test");
            await client.Subscribe();
            Console.WriteLine($"Subscribted to topic");
        }

        public class Subscriber
        {
            private readonly PubSub.PubSubClient pubSubClient;
            private Subscription subscription;

            public Subscriber(PubSub.PubSubClient pubSubClient)
            {
                this.pubSubClient = pubSubClient;
            }

            public async Task Subscribe()
            {
                this.subscription = new Subscription() { Id = Guid.NewGuid().ToString() };
                using (var call = this.pubSubClient.Subscribe(this.subscription))
                {
                    //Receive
                    var responseReaderTask = Task.Run(async () =>
                    {
                        while (await call.ResponseStream.MoveNext())
                        {
                            Console.WriteLine($"{this.subscription.Id}: Event received: " + call.ResponseStream.Current);
                        }
                    });

                    await responseReaderTask;
                }
            }

            public void Unsubscribe()
            {
                this.pubSubClient.Unsubscribe(this.subscription);
            }
        }
    }
}
