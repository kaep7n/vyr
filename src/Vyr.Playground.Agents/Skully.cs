using System;
using System.Threading.Tasks;
using Vyr.Agents;
using Vyr.Core;

namespace Vyr.Playground.Agents
{
    public class Skully : IAgent
    {
        public bool IsRunning => throw new NotImplementedException();

        public Task EnqueueAsync(IMessage message)
        {
            throw new NotImplementedException();
        }

        public void Idle()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            Console.WriteLine("Hey i'm Skully");
        }
    }
}
