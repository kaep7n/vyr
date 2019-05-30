using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Vyr.Skills.Tests
{
    public partial class DataflowSkillTests
    {
        [Fact]
        public void Enable_should_set_IsEnabled_to_true()
        {
            var skill = new DataflowSkill();

            skill.Enable();
            Assert.True(skill.IsEnabled);
        }

        [Fact]
        public async Task EnqueueAsync_should_throw_ArgumentNullException()
        {
            var skill = new DataflowSkill();

            await Assert.ThrowsAsync<ArgumentNullException>(() => skill.EnqueueAsync(null));
        }

        [Fact]
        public async Task EnqueueAsync_to_enabled_skill_should_process_request()
        {
            var skill = new DataflowSkillFake();
            skill.Enable();
            await skill.EnqueueAsync(new Request());

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.True(skill.HasProcessedAnyRequests());
        }

        [Fact]
        public async Task EnqueueAsync_to_not_enabled_skill_should_not_process_request()
        {
            var skill = new DataflowSkillFake();
            await skill.EnqueueAsync(new Request());

            Assert.False(skill.HasProcessedAnyRequests());
        }

        [Fact]
        public async Task EnqueueAsync_to_disabled_skill_should_not_process_request()
        {
            var skill = new DataflowSkillFake();

            await skill.EnqueueAsync(new Request());
            Assert.False(skill.HasProcessedAnyRequests());

            skill.Enable();

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.False(skill.HasProcessedAnyRequests());
        }

        [Fact]
        public async Task EnqueueAsync_should_process_request_when_enabled_but_not_after_disable()
        {
            var skill = new DataflowSkillFake();
            skill.Enable();
            await skill.EnqueueAsync(new Request());

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(1, skill.GetProcessedRequestsCount());

            skill.Disable();
            await skill.EnqueueAsync(new Request());

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(1, skill.GetProcessedRequestsCount());
        }

        [Fact]
        public async Task EnqueueAsync_should_process_request_only_once_when_enabled_was_called_multiple_times()
        {
            var skill = new DataflowSkillFake();
            skill.Enable();
            skill.Enable();
            await skill.EnqueueAsync(new Request());

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(1, skill.GetProcessedRequestsCount());
        }

        [Fact]
        public void Subscribe_should_throw_ArgumentNullException()
        {
            var skill = new DataflowSkillFake();
            Assert.Throws<ArgumentNullException>(() => skill.Subscribe(null));
        }

        [Fact]
        public async Task Subscribe_should_send_response_to_target_action()
        {
            var request = new Request();

            var skill = new DataflowSkillFake();
            skill.Enable();
            skill.Subscribe(r =>
            {
                var response = (Response)r;

                Assert.Equal(request, response.Request);
            });

            await skill.EnqueueAsync(request);
            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);
        }

        [Fact]
        public async Task Subscribe_should_send_response_to_multiple_target_actions()
        {
            var responseCount = 0;
            var request = new Request();

            var skill = new DataflowSkillFake();
            skill.Enable();
            skill.Subscribe(r =>
            {
                var response = (Response)r;
                Assert.Equal(request, response.Request);
                responseCount++;
            });
            skill.Subscribe(r =>
            {
                var response = (Response)r;
                Assert.Equal(request, response.Request);
                responseCount++;
            });

            await skill.EnqueueAsync(request);
            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(2, responseCount);
        }

        [Fact]
        public void Disable_should_set_IsEnabled_to_false()
        {
            var skill = new DataflowSkill();

            skill.Enable();
            Assert.True(skill.IsEnabled);

            skill.Disable();
            Assert.False(skill.IsEnabled);
        }
    }
}
