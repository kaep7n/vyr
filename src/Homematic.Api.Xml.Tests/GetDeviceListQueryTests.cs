using Homematic.Testing;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Homematic.Api.Xml.Tests
{
    public class GetDeviceListQueryTests
    {
        [Fact]
        public async Task ExecuteAsync()
        {
            var deviceListXml = await new Resources().ReadDeviceListXmlAsync();
            var httpClient = new HttpClient(new FakeHomematicHttpMessageHandler(deviceListXml));
            httpClient.BaseAddress = new Uri("http://baseurimustbeset.com/");

            var getDeviceListQuery = new GetDeviceListQuery(httpClient);

            var deviceList = await getDeviceListQuery.ExecuteAsync();

            Assert.NotNull(deviceList);
            Assert.NotEmpty(deviceList.Devices);
        }
    }
}
