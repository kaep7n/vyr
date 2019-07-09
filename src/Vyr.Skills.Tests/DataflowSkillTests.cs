using System;
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
        public void Enable_should_set_IsEnabled_to_true()
        {
            var sourceBlock = new BufferBlock<IMessage>();
            var targetBlock = new BufferBlock<IMessage>();

            var skill = new DataflowSkillFake(sourceBlock, targetBlock);

            skill.Enable();
            Assert.True(skill.IsEnabled);
        }

        [Fact]
        public async Task SendAsync_from_source_to_enabled_skill_should_process_request()
        {
            var sourceBlock = new BufferBlock<IMessage>();
            var targetBlock = new BufferBlock<IMessage>();

            var skill = new DataflowSkillFake(sourceBlock, targetBlock);
            skill.Enable();

            await sourceBlock.SendAsync(new FakeMessage());

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.True(skill.HasProcessedAnyRequests());
        }

        [Fact]
        public async Task SendAsync_from_source_to_not_enabled_skill_should_not_process_request()
        {
            var sourceBlock = new BufferBlock<IMessage>();
            var targetBlock = new BufferBlock<IMessage>();

            var skill = new DataflowSkillFake(sourceBlock, targetBlock);

            await sourceBlock.SendAsync(new FakeMessage());

            Assert.False(skill.HasProcessedAnyRequests());
        }

        [Fact]
        public async Task SendAsync_from_source_should_process_message_after_enable()
        {
            var sourceBlock = new BufferBlock<IMessage>();
            var targetBlock = new BufferBlock<IMessage>();

            var skill = new DataflowSkillFake(sourceBlock, targetBlock);

            await sourceBlock.SendAsync(new FakeMessage());

            Assert.False(skill.HasProcessedAnyRequests());

            skill.Enable();

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.True(skill.HasProcessedAnyRequests());
        }

        [Fact]
        public async Task SendAsync_from_source_should_process_request_when_enabled_but_not_after_disable()
        {
            var sourceBlock = new BufferBlock<IMessage>();
            var targetBlock = new BufferBlock<IMessage>();

            var skill = new DataflowSkillFake(sourceBlock, targetBlock);

            skill.Enable();
            await sourceBlock.SendAsync(new FakeMessage());

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(1, skill.GetProcessedRequestsCount());

            skill.Disable();
            await sourceBlock.SendAsync(new FakeMessage());

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(1, skill.GetProcessedRequestsCount());
        }

        [Fact]
        public async Task SendAsync_from_source_to_should_process_request_only_once_when_enabled_was_called_multiple_times()
        {
            var sourceBlock = new BufferBlock<IMessage>();
            var targetBlock = new BufferBlock<IMessage>();

            var skill = new DataflowSkillFake(sourceBlock, targetBlock);

            skill.Enable();
            skill.Enable();

            await sourceBlock.SendAsync(new FakeMessage());

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(1, skill.GetProcessedRequestsCount());
        }

        [Fact]
        public async Task SendAsync_from_source_to_should_send_response_to_target_block()
        {
            var targetCount = 0;

            var sourceBlock = new BufferBlock<IMessage>();
            var targetBlock = new ActionBlock<IMessage>(m => targetCount++);

            var message = new FakeMessage();

            var skill = new DataflowSkillFake(sourceBlock, targetBlock);
            skill.Enable();

            await sourceBlock.SendAsync(message);

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(1, targetCount);
        }

        [Fact]
        public void Disable_should_set_IsEnabled_to_false()
        {
            var sourceBlock = new BufferBlock<IMessage>();
            var targetBlock = new BufferBlock<IMessage>();

            var skill = new DataflowSkill(sourceBlock, targetBlock);

            skill.Enable();
            Assert.True(skill.IsEnabled);

            skill.Disable();
            Assert.False(skill.IsEnabled);
        }
    }
}
