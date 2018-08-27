using System.Xml.Serialization;

namespace HetsImport.ImportModels
{
    /// <summary>
    /// Project Import Model
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Project Id
        /// </summary>
        [XmlElement("Project_Id")]
        public int Project_Id { get; set; }

        /// <summary>
        /// Service Area Id
        /// </summary>
        [XmlElement("Service_Area_Id")]
        public int Service_Area_Id { get; set; }

        /// <summary>
        /// Project Number
        /// </summary>
        [XmlElement("Project_Num")]
        public string Project_Num { get; set; }

        /// <summary>
        /// Job Description 1
        /// </summary>
        [XmlElement("Job_Desc1")]
        public string Job_Desc1 { get; set; }

        /// <summary>
        /// Job Description 2
        /// </summary>
        [XmlElement("Job_Desc2")]
        public string Job_Desc2 { get; set; }

        /// <summary>
        /// Created Date
        /// </summary>
        [XmlElement("Created_Dt")]
        public string Created_Dt { get; set; }

        /// <summary>
        /// Created By
        /// </summary>
        [XmlElement("Created_By")]
        public string Created_By { get; set; }
 
    }
}
