using System;
using System.Net.Http;
using System.Threading.Tasks;
using Vyr.Testing;
using Xunit;

namespace Homematic.Api.Xml.Tests
{
    public class GetDeviceListQueryTests
    {
        [DebugOnlyFact]
        public async Task ExecuteAsync()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://192.168.2.101/config/xmlapi/");

            var getDeviceListQuery = new GetDeviceListQuery(httpClient);

            var deviceList = await getDeviceListQuery.ExecuteAsync();

            Assert.NotNull(deviceList);
            Assert.NotEmpty(deviceList.Devices);
        }
    }
}
