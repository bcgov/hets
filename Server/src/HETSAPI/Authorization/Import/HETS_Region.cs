using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class HETS_Region
    {
        [XmlElement("Region_Id")]
        public int Region_Id { get; set; }

        [XmlElement("Ministry_Region_Id")]
        public int Ministry_Region_Id { get; set; }

        [XmlElement("Region_Name")]
        public string Name { get; set; }

        [XmlElement("Region_Number")]
        public int Region_Number { get; set; }
    }
}

