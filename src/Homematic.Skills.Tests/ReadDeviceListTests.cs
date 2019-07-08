using Homematic.Api.Xml;
using Homematic.Testing;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Vyr.Core;
using Xunit;

namespace Homematic.Skills.Tests
{
    public class ReadDeviceListTests
    {
        [Fact]
        public async Task Test()
        {
            var responseCount = 0;

            var deviceListXml = await new Resources().ReadDeviceListXmlAsync();
            var httpClient = new HttpClient(new FakeHomematicHttpMessageHandler(deviceListXml));
            httpClient.BaseAddress = new Uri("http://baseurimustbeset.com/");

            var getDeviceListQuery = new GetDeviceListQuery(httpClient);

            var source = new BufferBlock<IMessage>();

            var skill = new ReadDeviceList(source, getDeviceListQuery);
            skill.Enable();

            skill.Subscribe(r =>
            {
                responseCount++;
            });

            await source.SendAsync(new ReadDeviceListRequest());

            Thread.Sleep(2000);

            Assert.Equal(25, responseCount);
        }
    }
}
