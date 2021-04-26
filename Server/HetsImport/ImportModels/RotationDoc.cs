using System.Xml.Serialization;

namespace HetsImport.ImportModels
{
    /// <summary>
    /// RotationDoc Import Model
    /// </summary>
    [XmlType(TypeName = "RotationDoc")]    
    public class RotationDoc
    {
        /// <summary>
        /// Note Id
        /// </summary>
        [XmlElement("Note_Id")]
        public int Note_Id { get; set; }

        /// <summary>
        /// Equip Id
        /// </summary>
        [XmlElement("Equip_Id")]
        public int Equip_Id { get; set; }

        /// <summary>
        /// Note Dt
        /// </summary>
        [XmlElement("Note_Dt")]
        public string Note_Dt { get; set; }
        
        /// <summary>
        /// Service Area Id
        /// </summary>
        [XmlElement("Service_Area_Id")]
        public string Service_Area_Id { get; set; }

        /// <summary>
        /// Project Id
        /// </summary>
        [XmlElement("Project_Id")]
        public int Project_Id { get; set; }

        /// <summary>
        /// Note Type
        /// </summary>
        [XmlElement("Note_Type")]
        public string Note_Type { get; set; }

        /// <summary>
        /// Reason
        /// </summary>
        [XmlElement("Reason")]
        public string Reason { get; set; }

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
    }
}
