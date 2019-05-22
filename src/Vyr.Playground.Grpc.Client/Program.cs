using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static PublishAndSubcribe.PubSub;

namespace Vyr.Playground.Grpc
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
              {
                  var receivingChannel = new Channel("127.0.0.1:50052", ChannelCredentials.Insecure);
                  var receivingClient = new Subscriber(new PubSubClient(receivingChannel));
                  receivingClient.Subscribe("configuration/changed");
                  await receivingClient.AttachAsync();
              });

            var sendingChannel = new Channel("127.0.0.1:50052", ChannelCredentials.Insecure);
            var sendingClient = new Subscriber(new PubSubClient(sendingChannel));
            sendingClient.Publish("configuration/changed");

            Console.ReadLine();
        }
    }
}
