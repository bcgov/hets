using System.Xml.Serialization;

namespace HetsImport.ImportModels
{
    /// <summary>
    /// Equip(ment) Attach(ment) Import Model
    /// </summary>
    [XmlType(TypeName = "Equip_Attach")]
    public class EquipAttach
    {
        /// <summary>
        /// Equipment Id
        /// </summary>
        [XmlElement("Equip_Id")]
        public int? Equip_Id { get; set; }

        /// <summary>
        /// Attachment Sequence Number
        /// </summary>
        [XmlElement("Attach_Seq_Num")]
        public int? Attach_Seq_Num { get; set; }

        /// <summary>
        /// Attachment Decription
        /// </summary>
        [XmlElement("Attach_Desc")]
        public string Attach_Desc { get; set; }

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
