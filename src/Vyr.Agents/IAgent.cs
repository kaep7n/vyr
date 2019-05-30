using System.Threading.Tasks;

namespace Vyr.Agents
{
    public interface IAgent
    {
        bool IsRunning { get; }

        void Run();

        Task EnqueueAsync(IMessage message);

        void Idle();
    }
}
