using System.Xml.Serialization;

namespace HetsImport.ImportModels
{
    /// <summary>
    /// HETS Audit Import Mapping
    /// </summary>
    [XmlType(TypeName = "HETS_Audit")]
    public class HetsAudit
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
