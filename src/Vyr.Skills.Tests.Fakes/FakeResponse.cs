using System;

namespace Vyr.Skills.Tests.Fakes
{
    public class FakeResponse : IResponse
    {
        public FakeResponse(IRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            this.Id = Guid.NewGuid().ToString();
            this.Request = request;
        }

        public string Id { get; private set; }

        public IRequest Request { get; }
    }
}
