
namespace Vyr.Agents
{
    public class DataflowAgent : IAgent
    {
        public bool IsRunning { get; private set; }

        public void Run()
        {
            this.IsRunning = true;
        }

        public void Idle()
        {
            this.IsRunning = false;
        }
    }
}
