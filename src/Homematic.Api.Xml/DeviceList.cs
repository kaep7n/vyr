using System.Collections.Generic;
using System.Xml.Serialization;

namespace Homematic.Api.Xml
{
    [XmlRoot(ElementName = "deviceList")]
    public class DeviceList
    {
        [XmlElement(ElementName = "device")]
        public List<Device> Devices { get; set; }
    }
}
