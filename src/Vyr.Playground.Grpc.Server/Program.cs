using Grpc.Core;
using PublishAndSubcribe;
using System;
using System.Threading.Tasks;

namespace Vyr.Playground.Grpc
{
    class Program
    {
        private const int Port = 50052;

        static async Task Main(string[] args)
        {
            var server = new Server
            {
                Services = { PubSub.BindService(new PubSubServer()) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("PubSub server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            await server.ShutdownAsync();
        }
    }
}
