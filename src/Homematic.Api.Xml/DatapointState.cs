using System;
using System.Text;
using System.Xml.Serialization;

namespace Homematic.Api.Xml
{
    [XmlRoot(ElementName = "datapoint")]
    public class DatapointState
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "ise_id")]
        public string IseId { get; set; }

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }

        [XmlAttribute(AttributeName = "valuetype")]
        public string ValueType { get; set; }

        [XmlAttribute(AttributeName = "valueunit")]
        public string ValueUnit { get; set; }

        [XmlAttribute(AttributeName = "timestamp")]
        public string Timestamp { get; set; }
    }
}
