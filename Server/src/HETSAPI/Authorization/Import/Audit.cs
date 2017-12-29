using System.Xml.Serialization;

namespace SchoolBusAPI.Import
{
    public class Audit
    {
        [XmlElement("Created_By")]
        public string Created_By { get; set; }

        [XmlElement("Created_Dt")]
        public string Created_Dt { get; set; }

        [XmlElement("Action")]
        public string Action { get; set; }

        [XmlElement("Reason")]
        public string Reason { get; set; }
    }
}
