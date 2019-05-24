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
            await dataflowSkill.EnqueueAsync(new Job("1"));

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.True(dataflowSkill.HasProcessedAnyJob());
        }

        [Fact]
        public async Task Enqueue_to_not_enabled_skill_should_not_process_job()
        {
            var dataflowSkill = new DataflowSkillFake();
            await dataflowSkill.EnqueueAsync(new Job("1"));

            Assert.False(dataflowSkill.HasProcessedAnyJob());
        }

        [Fact]
        public async Task Enqueue_to_not_enabled_skill_should_not_process_job_until_skill_is_enabled()
        {
            var dataflowSkill = new DataflowSkillFake();

            await dataflowSkill.EnqueueAsync(new Job("1"));
            Assert.False(dataflowSkill.HasProcessedAnyJob());

            dataflowSkill.Enable();
            
            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.True(dataflowSkill.HasProcessedAnyJob());
        }

        [Fact]
        public async Task Enqueue_should_process_job_when_enabled_but_not_after_disable()
        {
            var dataflowSkill = new DataflowSkillFake();
            dataflowSkill.Enable();
            await dataflowSkill.EnqueueAsync(new Job("1"));

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(1, dataflowSkill.ProcessedJobsCount());

            dataflowSkill.Disable();
            await dataflowSkill.EnqueueAsync(new Job("1"));

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(1, dataflowSkill.ProcessedJobsCount());
        }
    }
}
