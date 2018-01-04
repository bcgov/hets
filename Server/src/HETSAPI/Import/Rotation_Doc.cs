using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class Rotation_Doc
    {
        [XmlElement("Equip_Id")]
        public int Equip_Id { get; set; }

        [XmlElement("Note_Dt")]
        public string Note_Dt { get; set; }

        [XmlElement("Created_Dt")]
        public string Created_Dt { get; set; }

        [XmlElement("Service_Area_Id")]
        public string Service_Area_Id { get; set; }

        [XmlElement("Project_Id")]
        public int Project_Id { get; set; }

        [XmlElement("Note_Type")]
        public string Note_Type { get; set; }

        [XmlElement("Reason")]
        public string Reason { get; set; }

        [XmlElement("Note_Id")]
        public string Note_Id { get; set; }

        [XmlElement("Created_By")]
        public string Created_By { get; set; }
    }
}
