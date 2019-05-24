using System.Collections.Generic;
using System.Xml.Serialization;

namespace Homematic.Api.Xml
{
    [XmlRoot(ElementName = "channel")]
    public class ChannelState
    {
        [XmlElement(ElementName = "datapoint")]
        public List<DatapointState> Datapoint { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "ise_id")]
        public string IseId { get; set; }
    }
}
