using System.Xml.Serialization;

namespace HetsImport.ImportModels
{
    /// <summary>
    /// Area Import Model
    /// </summary>
    public class Area
    {
        /// <summary>
        /// Area Id
        /// </summary>
        [XmlElement("Area_Id")]
        public int Area_Id { get; set; }

        /// <summary>
        /// Area Code
        /// </summary>
        [XmlElement("Area_Cd")]
        public string Area_Cd { get; set; }

        /// <summary>
        /// Area Description
        /// </summary>
        [XmlElement("Area_Desc")]
        public string Area_Desc { get; set; }

        /// <summary>
        /// Service Area Id
        /// </summary>
        [XmlElement("Service_Area_Id")]
        public int Service_Area_Id { get; set; }

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
