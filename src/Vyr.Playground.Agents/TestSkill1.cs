using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vyr.Core;
using Vyr.Skills;

namespace Vyr.Playground.Agents
{
    public class TestSkill1 : DataflowSkill
    {
        protected override IEnumerable<string> GetTopics()
        {
            yield return "/";
        }

        protected override Task ProcessAsync(IMessage message)
        {
            return base.ProcessAsync(message);
        }
    }
}
