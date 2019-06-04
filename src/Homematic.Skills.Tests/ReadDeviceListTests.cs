using Homematic.Api.Xml;
using Homematic.Testing;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Homematic.Skills.Tests
{
    public class ReadDeviceListTests
    {
        private readonly ITestOutputHelper output;

        public ReadDeviceListTests(ITestOutputHelper output)
        {
            if (output is null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            this.output = output;
        }

        [Fact]
        public async Task Test()
        {
            var responseCount = 0;

            var deviceListXml = await new Resources().ReadDeviceListXmlAsync();
            var httpClient = new HttpClient(new FakeHomematicHttpMessageHandler(deviceListXml));
            httpClient.BaseAddress = new Uri("http://baseurimustbeset.com/");

            var getDeviceListQuery = new GetDeviceListQuery(httpClient);

            var skill = new ReadDeviceList(getDeviceListQuery);
            skill.Enable();

            skill.Subscribe(r =>
            {
                responseCount++;
            });

            await skill.EnqueueAsync(new ReadDeviceListRequest());

            Thread.Sleep(2000);

            Assert.Equal(25, responseCount);
        }
    }
}
