using System;

namespace Vyr.Skills.Tests.Fakes
{
    public class FakeRequest : IRequest
    {
        public FakeRequest()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; private set; }
    }
}
