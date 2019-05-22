using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using static PublishAndSubcribe.PubSub;

namespace Vyr.Playground.Grpc
{
    partial class Program
    {
        private const int messageCount = 4000;
        private const int concurrentSendProcesses = 4;

        static async Task Main(string[] args)
        {
            _ = Task.Run(async () =>
                {
                    var receivingChannel = new Channel("127.0.0.1:50052", ChannelCredentials.Insecure);
                    var receivingClient = new Subscriber(new PubSubClient(receivingChannel));
                    receivingClient.Subscribe("configuration/changed");
                    await receivingClient.AttachAsync();
                });

            var sendingChannel = new Channel("127.0.0.1:50052", ChannelCredentials.Insecure);
            var sendingClient = new Subscriber(new PubSubClient(sendingChannel));

            var sw = Stopwatch.StartNew();

            var messagesPerProcessCount = messageCount / concurrentSendProcesses;
            Console.WriteLine($"Sending {messageCount} messages with {concurrentSendProcesses} processes");

            var tasks = new List<Task>();

            for (int i = 0; i < concurrentSendProcesses; i++)
            {
               var task = Task.Run(() =>
                {
                    for (var i = 0; i < messagesPerProcessCount; i++)
                    {
                        sendingClient.Publish("configuration/changed");
                    }
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            sw.Stop();

            Console.WriteLine($"Sending {messageCount} messages took {sw.Elapsed}");

            Console.ReadLine();
        }
    }
}
