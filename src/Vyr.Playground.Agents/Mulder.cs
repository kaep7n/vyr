﻿using System;
using Vyr.Agents;

namespace Vyr.Playground.Agents
{
    public class Mulder : IAgent
    {
        public bool IsRunning => throw new NotImplementedException();

        public void Idle()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            Console.WriteLine("Hey i'm Mulder");
        }
    }
}
