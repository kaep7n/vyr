using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using Vyr.Core;
using Vyr.Skills;

namespace Vyr.Playground.Agents
{
    public class TestSkill1 : DataflowSkill
    {
        private readonly Timer timer;

        public TestSkill1()
        {
            this.timer = new Timer(1000);
            this.timer.Elapsed += this.Timer_Elapsed;
        }

        protected override IEnumerable<string> GetTopics()
        {
            yield return "/";
        }

        public override Task EnableAsync()
        {
            this.timer.Start();
            return base.EnableAsync();
        }

        public override Task DisableAsync()
        {
            this.timer.Stop();
            return base.DisableAsync();
        }

        protected override Task ProcessAsync(IMessage message)
        {
            return base.ProcessAsync(message);
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await this.PublishAsync(new TestMessage());
        }
    }
}
