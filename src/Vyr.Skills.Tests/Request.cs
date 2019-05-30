using System;

namespace Vyr.Skills.Tests
{
    public class Request : IRequest
    {
        public Request()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; private set; }
    }
}
