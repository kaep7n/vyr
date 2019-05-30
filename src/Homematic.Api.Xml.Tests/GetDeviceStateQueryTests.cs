using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Homematic.Api.Xml.Tests
{
    public class GetDeviceStateQueryTests
    {
        [DebugOnlyFact]
        public async Task ExecuteAsync()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://192.168.2.101/config/xmlapi/");

            var getDeviceStateQuery = new GetDeviceStateQuery(httpClient, "2074");

            var state = await getDeviceStateQuery.ExecuteAsync();

            Assert.NotNull(state);
        }
    }
}
