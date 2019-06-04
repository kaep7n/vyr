using System.IO;
using System.Threading.Tasks;

namespace Homematic.Testing
{
    public class Resources
    {
        public async Task<string> ReadDeviceListXmlAsync()
        {
            return await File.ReadAllTextAsync("Resources/Devicelist.xml");
        }

        public async Task<string> ReadDeviceStatusXmlAsync()
        {
            return await File.ReadAllTextAsync("Resources/DeviceState2074.xml");
        }
    }
}
