using System.Threading.Tasks;
using Vyr.Core;

namespace Vyr.Agents
{
    public interface IAgent
    {
        bool IsRunning { get; }

        void Run();

        void Idle();
    }
}
