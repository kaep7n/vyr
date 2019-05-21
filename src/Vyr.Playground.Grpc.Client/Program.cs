using Grpc.Core;
using System;
using System.Collections.Generic;
using static PublishAndSubcribe.PubSub;

namespace Vyr.Playground.Grpc
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var subs = new List<Subscriber>();

            for (int i = 0; i < 5; i++)
            {
                subs.Add(SubscribeToTopic());
            }

            subs[0].Publish();

            Console.ReadLine();
        }

        private static Subscriber SubscribeToTopic()
        {
            var channel = new Channel("127.0.0.1:50052", ChannelCredentials.Insecure);
            var client = new Subscriber(new PubSubClient(channel));

            client.Subscribe();

            return client;
        }
    }
}
