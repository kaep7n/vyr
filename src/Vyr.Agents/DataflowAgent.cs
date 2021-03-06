﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Vyr.Core;
using Vyr.Skills;

namespace Vyr.Agents
{
    public class DataflowAgent : IAgent
    {
        private readonly IEnumerable<ISkill> skills;

        private readonly BroadcastBlock<IMessage> bus = new BroadcastBlock<IMessage>(null);

        public DataflowAgent(IEnumerable<ISkill> skills)
        {
            if (skills is null)
            {
                throw new ArgumentNullException(nameof(skills));
            }

            this.skills = skills;
        }

        public bool IsRunning { get; private set; }

        public virtual async Task RunAsync()
        {
            foreach (var skill in this.skills)
            {
                if (skill is DataflowSkill dataflowSkill)
                {
                    dataflowSkill.SetSource(this.bus);
                    dataflowSkill.SetTarget(this.bus);
                }

                await skill.EnableAsync();
            }

            this.IsRunning = true;
        }
        public virtual async Task IdleAsync()
        {
            this.IsRunning = false;

            foreach (var skill in this.skills)
            {
                await skill.DisableAsync();
            }
        }

        protected virtual void ProcessMessage(IMessage message)
        {
        }
    }
}
