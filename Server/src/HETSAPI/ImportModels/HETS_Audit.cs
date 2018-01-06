using System.Xml.Serialization;

namespace HETSAPI.ImportModels
{
    /// <summary>
    /// HETS Audit Import Mapping
    /// </summary>
    public class HETS_AUDIT
    {
        /// <summary>
        /// Created By
        /// </summary>
        [XmlElement]
        public string Created_By { get; set; }

        /// <summary>
        /// Created Date
        /// </summary>
        [XmlElement]
        public string Created_Dt { get; set; }

        /// <summary>
        /// Action
        /// </summary>
        [XmlElement]
        public string Action { get; set; }

        /// <summary>
        /// Reason
        /// </summary>
        [XmlElement]
        public string Reason { get; set; }
        
    }
}
