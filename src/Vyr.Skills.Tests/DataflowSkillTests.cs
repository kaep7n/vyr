using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Vyr.Core;
using Vyr.Skills.Tests.Fakes;
using Xunit;

namespace Vyr.Skills.Tests
{
    public partial class DataflowSkillTests
    {
        [Fact]
        public async Task EnableAsync_should_set_IsEnabled_to_true()
        {
            var sourceBlock = new BufferBlock<IMessage>();
            var targetBlock = new BufferBlock<IMessage>();

            var skill = new DataflowSkillFake(i => null, "Test");
            skill.SetSource(sourceBlock);
            skill.SetTarget(targetBlock);

            await skill.EnableAsync();
            Assert.True(skill.IsEnabled);
        }

        [Fact]
        public async Task SendAsync_from_source_to_enabled_skill_should_process_request()
        {
            var sourceBlock = new BufferBlock<IMessage>();
            var targetBlock = new BufferBlock<IMessage>();

            var skill = new DataflowSkillFake(i => null, "Test");
            skill.SetSource(sourceBlock);
            skill.SetTarget(targetBlock);

            await skill.EnableAsync();

            await sourceBlock.SendAsync(new FakeMessage("Test"));

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.True(skill.HasProcessedAnyMessages());
        }

        [Fact]
        public async Task SendAsync_from_source_to_not_enabled_skill_should_not_process_request()
        {
            var sourceBlock = new BufferBlock<IMessage>();
            var targetBlock = new BufferBlock<IMessage>();

            var skill = new DataflowSkillFake(i => null, "Test");
            skill.SetSource(sourceBlock);
            skill.SetTarget(targetBlock);

            await sourceBlock.SendAsync(new FakeMessage("Test"));

            Assert.False(skill.HasProcessedAnyMessages());
        }

        [Fact]
        public async Task SendAsync_from_source_should_process_message_after_enable()
        {
            var sourceBlock = new BufferBlock<IMessage>();
            var targetBlock = new BufferBlock<IMessage>();

            var skill = new DataflowSkillFake(i => null, "Test");
            skill.SetSource(sourceBlock);
            skill.SetTarget(targetBlock);

            await sourceBlock.SendAsync(new FakeMessage("Test"));

            Assert.False(skill.HasProcessedAnyMessages());

            await skill.EnableAsync();

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.True(skill.HasProcessedAnyMessages());
        }

        [Fact]
        public async Task SendAsync_from_source_should_process_request_when_enabled_but_not_after_disable()
        {
            var sourceBlock = new BufferBlock<IMessage>();
            var targetBlock = new BufferBlock<IMessage>();

            var skill = new DataflowSkillFake(i => null, "Test");
            skill.SetSource(sourceBlock);
            skill.SetTarget(targetBlock);

            await skill.EnableAsync();
            await sourceBlock.SendAsync(new FakeMessage("Test"));

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(1, skill.GetProcessedMessagesCount());

            await skill.DisableAsync();
            await sourceBlock.SendAsync(new FakeMessage("Test"));

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(1, skill.GetProcessedMessagesCount());
        }

        [Fact]
        public async Task SendAsync_from_source_to_should_process_request_only_once_when_enabled_was_called_multiple_times()
        {
            var sourceBlock = new BufferBlock<IMessage>();
            var targetBlock = new BufferBlock<IMessage>();

            var skill = new DataflowSkillFake(i => null, "Test");
            skill.SetSource(sourceBlock);
            skill.SetTarget(targetBlock);

            await skill.EnableAsync();
            await skill.EnableAsync();

            await sourceBlock.SendAsync(new FakeMessage("Test"));

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(1, skill.GetProcessedMessagesCount());
        }

        [Fact]
        public async Task SendAsync_from_source_to_should_send_response_to_target_block()
        {
            var targetCount = 0;

            var sourceBlock = new BufferBlock<IMessage>();
            var targetBlock = new ActionBlock<IMessage>(m => targetCount++);

            var message = new FakeMessage("Test");

            var skill = new DataflowSkillFake(i => new FakeMessage("Target"), "Test");
            skill.SetSource(sourceBlock);
            skill.SetTarget(targetBlock);

            await skill.EnableAsync();

            await sourceBlock.SendAsync(message);

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(1, targetCount);
        }

        [Fact]
        public async Task Disable_should_set_IsEnabled_to_false()
        {
            var sourceBlock = new BufferBlock<IMessage>();
            var targetBlock = new BufferBlock<IMessage>();

            var skill = new DataflowSkillFake(i => null, "Test");
            skill.SetSource(sourceBlock);
            skill.SetTarget(targetBlock);

            await skill.EnableAsync();
            Assert.True(skill.IsEnabled);

            await skill.DisableAsync();
            Assert.False(skill.IsEnabled);
        }
    }
}
