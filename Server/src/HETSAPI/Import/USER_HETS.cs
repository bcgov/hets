using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class User_HETS
    {
        [XmlElement("Popt_ID")]
        public int Popt_Id { get; set; }

        [XmlElement("Service_Area_Id")]
        public int Service_Area_Id { get; set; }

        [XmlElement("User_Cd")]
        public string User_Cd { get; set; }

        [XmlElement("Authority")]
        public string Authority { get; set; }

        [XmlElement("Default_Service_Area")]
        public string Default_Service_Area { get; set; }

        [XmlElement("Created_Dt")]
        public string Created_Dt { get; set; }

        [XmlElement("Created_By")]
        public string Created_By { get; set; }

        [XmlElement("Modified_Dt")]
        public string Modified_Dt { get; set; }

        [XmlElement("Modified_By")]
        public string Modified_By { get; set; }
    }
}
