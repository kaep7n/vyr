using System.Threading.Tasks;

namespace Vyr.Agents
{
    public interface IAgent
    {
        bool IsRunning { get; }

        Task RunAsync();

        Task IdleAsync();
    }
}
