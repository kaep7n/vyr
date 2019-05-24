using System.Xml.Serialization;

namespace Homematic.Api.Xml
{
    [XmlRoot(ElementName = "state")]
    public class State
    {
        [XmlElement(ElementName = "device")]
        public DeviceState DeviceState { get; set; }
    }
}
