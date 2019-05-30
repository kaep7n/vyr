
using System;
using System.Collections.Generic;
using Vyr.Skills;

namespace Vyr.Agents
{
    public class DataflowAgent : IAgent
    {
        private readonly IEnumerable<ISkill> skills;

        public DataflowAgent(IEnumerable<ISkill> skills)
        {
            if (skills is null)
            {
                throw new ArgumentNullException(nameof(skills));
            }

            this.skills = skills;
        }

        public bool IsRunning { get; private set; }

        public void Run()
        {
            foreach (var skill in this.skills)
            {
                skill.Enable();
            }

            this.IsRunning = true;
        }

        public void Idle()
        {
            this.IsRunning = false;
        }
    }
}
