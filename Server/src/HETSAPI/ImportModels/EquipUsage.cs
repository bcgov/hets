using System.Xml.Serialization;

namespace HETSAPI.ImportModels
{
    /// <summary>
    /// Equip(ment) Usage Import Model
    /// </summary>
    [XmlType(TypeName = "Equip_Usage")]
    public class EquipUsage
    {
        /// <summary>
        /// Equipment Id
        /// </summary>
        [XmlElement("Equip_Id")]
        public int? Equip_Id { get; set; }

        /// <summary>
        /// Project Id
        /// </summary>
        [XmlElement("Project_Id")]
        public int? Project_Id { get; set; }

        /// <summary>
        /// Service Area Id
        /// </summary>
        [XmlElement("Service_Area_Id")]
        public int? Service_Area_Id { get; set; }

        /// <summary>
        /// Worked Date
        /// </summary>
        [XmlElement("Worked_Dt")]
        public string Worked_Dt { get; set; }

        /// <summary>
        /// Entered Date
        /// </summary>
        [XmlElement("Entered_Dt")]
        public string Entered_Dt { get; set; }

        /// <summary>
        /// Hours
        /// </summary>
        [XmlElement("Hours")]
        public string Hours { get; set; }

        /// <summary>
        /// Rate
        /// </summary>
        [XmlElement("Rate")]
        public string Rate { get; set; }

        /// <summary>
        /// Hours 2
        /// </summary>
        [XmlElement("Hours2")]
        public string Hours2 { get; set; }

        /// <summary>
        /// Rate 2
        /// </summary>
        [XmlElement("Rate2")]
        public string Rate2 { get; set; }

        /// <summary>
        /// Hours 3
        /// </summary>
        [XmlElement("Hours3")]
        public string Hours3 { get; set; }

        /// <summary>
        /// Rate 3
        /// </summary>
        [XmlElement("Rate3")]
        public string Rate3 { get; set; }

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
