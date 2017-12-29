using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class HETS_AUDIT
    {
        [XmlElement]
        public string Created_By { get; set; }

        [XmlElement]
        public string Created_Dt { get; set; }

        [XmlElement]
        public string Action { get; set; }

        [XmlElement]
        public string Reason { get; set; }
        
    }
}
