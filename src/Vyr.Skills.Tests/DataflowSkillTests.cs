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
        public void Disable_should_set_IsEnabled_to_false()
        {
            var dataflowSkill = new DataflowSkill();

            dataflowSkill.Enable();
            Assert.True(dataflowSkill.IsEnabled);

            dataflowSkill.Disable();
            Assert.False(dataflowSkill.IsEnabled);
        }

        [Fact]
        public async Task Enqueue_to_enabled_skill_should_process_job()
        {
            var dataflowSkill = new DataflowSkillFake();
            dataflowSkill.Enable();
            await dataflowSkill.EnqueueAsync(new Request());

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.True(dataflowSkill.HasProcessedAnyRequests());
        }

        [Fact]
        public async Task Enqueue_to_not_enabled_skill_should_not_process_job()
        {
            var dataflowSkill = new DataflowSkillFake();
            await dataflowSkill.EnqueueAsync(new Request());

            Assert.False(dataflowSkill.HasProcessedAnyRequests());
        }

        [Fact]
        public async Task Enqueue_to_disabled_skill_should_not_process_job()
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
        public async Task Enqueue_should_process_job_when_enabled_but_not_after_disable()
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
        public async Task Enqueue_should_process_job_only_once_when_enabled_was_called_multiple_times()
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
        public async Task Subscribe_should_send_result_to_target_action()
        {
            var incomingJob = new Request();

            var dataflowSkill = new DataflowSkillFake();
            dataflowSkill.Enable();
            dataflowSkill.Subscribe(r =>
            {
                var result = (Response)r;

                Assert.Equal(incomingJob, result.Request);
            });
            
            await dataflowSkill.EnqueueAsync(incomingJob);
            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);
        }

        [Fact]
        public async Task Subscribe_should_send_result_to_multiple_target_actions()
        {
            var resultCount = 0;
            var request = new Request();

            var dataflowSkill = new DataflowSkillFake();
            dataflowSkill.Enable();
            dataflowSkill.Subscribe(r =>
            {
                var result = (Response)r;
                Assert.Equal(request, result.Request);
                resultCount++;
            });
            dataflowSkill.Subscribe(r =>
            {
                var result = (Response)r;
                Assert.Equal(request, result.Request);
                resultCount++;
            });

            await dataflowSkill.EnqueueAsync(request);
            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(2, resultCount);
        }
    }
}
