using System.Xml.Serialization;

namespace HETSAPI.Import
{
   // [XmlRoot("HETS_City")]
    public class HETS_City
    {
        [XmlElement("City_Id")]
        public int City_Id { get; set; }

        [XmlElement("Service_Area_Id")]
        public int Service_Area_Id { get; set; }

        [XmlElement("Seq_Num")]
        public int Seq_Num { get; set; }

        [XmlElement("City")]
        public string Name { get; set; }               
    }
}
