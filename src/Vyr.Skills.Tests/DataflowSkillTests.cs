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
            var dataflowSkill = new DataflowSkill();

            dataflowSkill.Enable();
            Assert.True(dataflowSkill.IsEnabled);
        }

        [Fact]
        public async Task EnqueueAsync_should_throw_ArgumentNullException()
        {
            var dataflowSkill = new DataflowSkill();

            await Assert.ThrowsAsync<ArgumentNullException>(() => dataflowSkill.EnqueueAsync(null));
        }

        [Fact]
        public async Task EnqueueAsync_to_enabled_skill_should_process_request()
        {
            var dataflowSkill = new DataflowSkillFake();
            dataflowSkill.Enable();
            await dataflowSkill.EnqueueAsync(new Request());

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.True(dataflowSkill.HasProcessedAnyRequests());
        }

        [Fact]
        public async Task EnqueueAsync_to_not_enabled_skill_should_not_process_request()
        {
            var dataflowSkill = new DataflowSkillFake();
            await dataflowSkill.EnqueueAsync(new Request());

            Assert.False(dataflowSkill.HasProcessedAnyRequests());
        }

        [Fact]
        public async Task EnqueueAsync_to_disabled_skill_should_not_process_request()
        {
            var dataflowSkill = new DataflowSkillFake();

            await dataflowSkill.EnqueueAsync(new Request());
            Assert.False(dataflowSkill.HasProcessedAnyRequests());

            dataflowSkill.Enable();

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.False(dataflowSkill.HasProcessedAnyRequests());
        }

        [Fact]
        public async Task EnqueueAsync_should_process_request_when_enabled_but_not_after_disable()
        {
            var dataflowSkill = new DataflowSkillFake();
            dataflowSkill.Enable();
            await dataflowSkill.EnqueueAsync(new Request());

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(1, dataflowSkill.GetProcessedRequestsCount());

            dataflowSkill.Disable();
            await dataflowSkill.EnqueueAsync(new Request());

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(1, dataflowSkill.GetProcessedRequestsCount());
        }

        [Fact]
        public async Task EnqueueAsync_should_process_request_only_once_when_enabled_was_called_multiple_times()
        {
            var dataflowSkill = new DataflowSkillFake();
            dataflowSkill.Enable();
            dataflowSkill.Enable();
            await dataflowSkill.EnqueueAsync(new Request());

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(1, dataflowSkill.GetProcessedRequestsCount());
        }

        [Fact]
        public void Subscribe_should_throw_ArgumentNullException()
        {
            var dataflowSkill = new DataflowSkillFake();
            Assert.Throws<ArgumentNullException>(() => dataflowSkill.Subscribe(null));
        }

        [Fact]
        public async Task Subscribe_should_send_response_to_target_action()
        {
            var request = new Request();

            var dataflowSkill = new DataflowSkillFake();
            dataflowSkill.Enable();
            dataflowSkill.Subscribe(r =>
            {
                var response = (Response)r;

                Assert.Equal(request, response.Request);
            });

            await dataflowSkill.EnqueueAsync(request);
            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);
        }

        [Fact]
        public async Task Subscribe_should_send_response_to_multiple_target_actions()
        {
            var responseCount = 0;
            var request = new Request();

            var dataflowSkill = new DataflowSkillFake();
            dataflowSkill.Enable();
            dataflowSkill.Subscribe(r =>
            {
                var response = (Response)r;
                Assert.Equal(request, response.Request);
                responseCount++;
            });
            dataflowSkill.Subscribe(r =>
            {
                var response = (Response)r;
                Assert.Equal(request, response.Request);
                responseCount++;
            });

            await dataflowSkill.EnqueueAsync(request);
            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(2, responseCount);
        }

        [Fact]
        public void Disable_should_set_IsEnabled_to_false()
        {
            var dataflowSkill = new DataflowSkill();

            dataflowSkill.Enable();
            Assert.True(dataflowSkill.IsEnabled);

            dataflowSkill.Disable();
            Assert.False(dataflowSkill.IsEnabled);
        }
    }
}
