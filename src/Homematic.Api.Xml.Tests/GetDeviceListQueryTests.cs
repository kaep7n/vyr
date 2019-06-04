using Homematic.Testing;
using Homematic.Testing.Properties;
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
            var httpClient = new HttpClient(new FakeHomematicHttpMessageHandler(Resources.Devicelist));
            httpClient.BaseAddress = new Uri("http://baseurimustbeset.com/");

            var getDeviceListQuery = new GetDeviceListQuery(httpClient);

            var deviceList = await getDeviceListQuery.ExecuteAsync();

            Assert.NotNull(deviceList);
            Assert.NotEmpty(deviceList.Devices);
        }
    }
}
