using System.Xml.Serialization;

namespace HetsImport.ImportModels
{
    /// <summary>
    /// Audit Import Model
    /// </summary>
    public class Audit
    {
        /// <summary>
        /// Created By
        /// </summary>
        [XmlElement("Created_By")]
        public string Created_By { get; set; }

        /// <summary>
        /// Created Date
        /// </summary>
        [XmlElement("Created_Dt")]
        public string Created_Dt { get; set; }

        /// <summary>
        /// Action
        /// </summary>
        [XmlElement("Action")]
        public string Action { get; set; }

        /// <summary>
        /// Reason
        /// </summary>
        [XmlElement("Reason")]
        public string Reason { get; set; }
    }
}
