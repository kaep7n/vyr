using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
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
            await dataflowSkill.EnqueueAsync(new IncomingJob("1"));

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.True(dataflowSkill.HasProcessedAnyJob());
        }

        [Fact]
        public async Task Enqueue_to_not_enabled_skill_should_not_process_job()
        {
            var dataflowSkill = new DataflowSkillFake();
            await dataflowSkill.EnqueueAsync(new IncomingJob("1"));

            Assert.False(dataflowSkill.HasProcessedAnyJob());
        }

        [Fact]
        public async Task Enqueue_to_disabled_skill_should_not_process_job()
        {
            var dataflowSkill = new DataflowSkillFake();

            await dataflowSkill.EnqueueAsync(new Job("1"));
            Assert.False(dataflowSkill.HasProcessedAnyJob());

            dataflowSkill.Enable();
            
            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.False(dataflowSkill.HasProcessedAnyJob());
        }

        [Fact]
        public async Task Enqueue_should_process_job_when_enabled_but_not_after_disable()
        {
            var dataflowSkill = new DataflowSkillFake();
            dataflowSkill.Enable();
            await dataflowSkill.EnqueueAsync(new IncomingJob("1"));

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(1, dataflowSkill.ProcessedJobsCount());

            dataflowSkill.Disable();
            await dataflowSkill.EnqueueAsync(new IncomingJob("1"));

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(1, dataflowSkill.ProcessedJobsCount());
        }

        [Fact]
        public async Task Enqueue_should_process_job_only_once_when_enabled_was_called_multiple_times()
        {
            var dataflowSkill = new DataflowSkillFake();
            dataflowSkill.Enable();
            dataflowSkill.Enable();
            await dataflowSkill.EnqueueAsync(new IncomingJob("1"));

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(1, dataflowSkill.ProcessedJobsCount());
        }

        [Fact]
        public async Task Subscribe_should_send_result_to_target_action()
        {
            var incomingJob = new IncomingJob("1");

            var dataflowSkill = new DataflowSkillFake();
            dataflowSkill.Enable();
            dataflowSkill.Subscribe(r =>
            {
                var result = (JobResult)r;

                Assert.Equal(incomingJob, result.IncomingJob);
            });
            
            await dataflowSkill.EnqueueAsync(incomingJob);
            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);
        }

        [Fact]
        public async Task Subscribe_should_send_result_to_multiple_target_actions()
        {
            var resultCount = 0;
            var incomingJob = new IncomingJob("1");

            var dataflowSkill = new DataflowSkillFake();
            dataflowSkill.Enable();
            dataflowSkill.Subscribe(r =>
            {
                var result = (JobResult)r;
                Assert.Equal(incomingJob, result.IncomingJob);
                resultCount++;
            });
            dataflowSkill.Subscribe(r =>
            {
                var result = (JobResult)r;
                Assert.Equal(incomingJob, result.IncomingJob);
                resultCount++;
            });

            await dataflowSkill.EnqueueAsync(incomingJob);
            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(2, resultCount);
        }
    }
}
