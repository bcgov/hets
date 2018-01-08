using System.Xml.Serialization;

namespace HETSAPI.ImportModels
{
    /// <summary>
    /// HETS User Import Model
    /// </summary>
    public class UserHets
    {
        /// <summary>
        /// Popt Id
        /// </summary>
        [XmlElement("Popt_ID")]
        public int Popt_Id { get; set; }

        /// <summary>
        /// Service Area Id
        /// </summary>
        [XmlElement("Service_Area_Id")]
        public int Service_Area_Id { get; set; }

        /// <summary>
        /// User Code
        /// </summary>
        [XmlElement("User_Cd")]
        public string User_Cd { get; set; }

        /// <summary>
        /// Authority
        /// </summary>
        [XmlElement("Authority")]
        public string Authority { get; set; }

        /// <summary>
        /// Default Service Area
        /// </summary>
        [XmlElement("Default_Service_Area")]
        public string Default_Service_Area { get; set; }

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

        /// <summary>
        /// Modified Date
        /// </summary>
        [XmlElement("Modified_Dt")]
        public string Modified_Dt { get; set; }

        /// <summary>
        /// Modified By
        /// </summary>
        [XmlElement("Modified_By")]
        public string Modified_By { get; set; }
    }
}
