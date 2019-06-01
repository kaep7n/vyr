using Homematic.Api.Xml;
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
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://192.168.2.101/config/xmlapi/");

            var getDeviceListQuery = new GetDeviceListQuery(httpClient);
            //var devices = await getDeviceListQuery.ExecuteAsync();

            var skill = new ReadDeviceList(getDeviceListQuery);
            skill.Enable();

            skill.Subscribe(r =>
            {
                this.output.WriteLine(((ReadDeviceListResponse)r).Device.Name);
            });

            await skill.EnqueueAsync(new ReadDeviceListRequest());

            Thread.Sleep(1000);
        }
    }
}
