using Xunit;

namespace Vyr.Agents.Tests
{
    public class DataflowAgentTests
    {
        [Fact]
        public void Run_should_set_IsRunning_true()
        {
            var agent = new DataflowAgent();
            agent.Run();

            Assert.True(agent.IsRunning);
        }

        [Fact]
        public void Idle_after_Run_should_set_IsRunning_false()
        {
            var agent = new DataflowAgent();
            agent.Run();
            agent.Idle();

            Assert.False(agent.IsRunning);
        }
    }
}
