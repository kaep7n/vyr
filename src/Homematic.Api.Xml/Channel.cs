using System.Xml.Serialization;

namespace Homematic.Api.Xml
{
    [XmlRoot(ElementName = "channel")]
    public class Channel
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "address")]
        public string Address { get; set; }

        [XmlAttribute(AttributeName = "ise_id")]
        public string IseId { get; set; }

        [XmlAttribute(AttributeName = "direction")]
        public string Direction { get; set; }

        [XmlAttribute(AttributeName = "parent_device")]
        public string ParentDevice { get; set; }

        [XmlAttribute(AttributeName = "index")]
        public string Index { get; set; }

        [XmlAttribute(AttributeName = "group_partner")]
        public string GroupPartner { get; set; }

        [XmlAttribute(AttributeName = "aes_available")]
        public string AesAvailable { get; set; }

        [XmlAttribute(AttributeName = "transmission_mode")]
        public string TransmissionMode { get; set; }

        [XmlAttribute(AttributeName = "visible")]
        public string Visible { get; set; }

        [XmlAttribute(AttributeName = "ready_config")]
        public string ReadyConfig { get; set; }

        [XmlAttribute(AttributeName = "operate")]
        public string Operate { get; set; }
    }
}
