using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Threading.Tasks;

namespace Vyr.Playground.Docker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine"))
                .CreateClient();

            var containers = await client.Containers.ListContainersAsync(new ContainersListParameters() { All = true });

            foreach (var container in containers)
            {
                Console.WriteLine($"{container.ID} {container.Status}");
            }

            Console.WriteLine("Hello World!");
        }
    }
}
