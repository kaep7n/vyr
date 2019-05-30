namespace Vyr.Agents
{
    public interface IAgent
    {
        bool IsRunning { get; }

        void Run();

        void Idle();
    }
}
