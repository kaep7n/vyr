using System.Collections.Generic;
using System.Xml.Serialization;

namespace Homematic.Api.Xml
{
    [XmlRoot(ElementName = "device")]
    public class DeviceState
    {
        [XmlElement(ElementName = "channel")]
        public List<ChannelState> Channel { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "ise_id")]
        public string IseId { get; set; }

        [XmlAttribute(AttributeName = "unreach")]
        public string Unreach { get; set; }

        [XmlAttribute(AttributeName = "sticky_unreach")]
        public string StickyUnreach { get; set; }

        [XmlAttribute(AttributeName = "config_pending")]
        public string ConfigPending { get; set; }
    }
}
