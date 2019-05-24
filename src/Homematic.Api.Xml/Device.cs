using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Homematic.Api.Xml
{
    [XmlRoot(ElementName = "device")]
    public class Device
    {
        [XmlElement(ElementName = "channel")]
        public List<Channel> Channel { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "address")]
        public string Address { get; set; }

        [XmlAttribute(AttributeName = "ise_id")]
        public string IseId { get; set; }

        [XmlAttribute(AttributeName = "interface")]
        public string Interface { get; set; }

        [XmlAttribute(AttributeName = "device_type")]
        public string DeviceType { get; set; }

        [XmlAttribute(AttributeName = "ready_config")]
        public string ReadyConfig { get; set; }
    }
}
