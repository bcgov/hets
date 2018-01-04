using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class Project
    {
        [XmlElement("Project_Id")]
        public int Project_Id { get; set; }

        [XmlElement("Service_Area_Id")]
        public int Service_Area_Id { get; set; }

        [XmlElement("Project_Num")]
        public string Project_Num { get; set; }

        [XmlElement("Job_Desc1")]
        public string Job_Desc1 { get; set; }

        [XmlElement("Job_Desc2")]
        public string Job_Desc2 { get; set; }

        [XmlElement("Created_Dt")]
        public string Created_Dt { get; set; }

        [XmlElement("Created_By")]
        public string Created_By { get; set; }
 
    }
}
