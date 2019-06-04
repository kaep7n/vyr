using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Homematic.Testing
{
    public class FakeHomematicHttpMessageHandler : HttpMessageHandler
    {
        private readonly string xml;

        public FakeHomematicHttpMessageHandler(string xml)
        {
            if (xml is null)
            {
                throw new ArgumentNullException(nameof(xml));
            }

            this.xml = xml;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            responseMessage.RequestMessage = request;
            responseMessage.Content = new StringContent(this.xml);

            return Task.FromResult(responseMessage);
        }
    }
}
