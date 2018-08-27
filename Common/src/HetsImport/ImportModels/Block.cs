using System.Xml.Serialization;

namespace HetsImport.ImportModels
{
    /// <summary>
    /// Block Import Model
    /// </summary>
    public class Block
    {
        /// <summary>
        /// Area Id
        /// </summary>
        [XmlElement("Area_Id")]
        public int? Area_Id { get; set; }

        /// <summary>
        /// Equipment Type Id
        /// </summary>
        [XmlElement("Equip_Type_Id")]
        public int? Equip_Type_Id { get; set; }

        /// <summary>
        /// Block Number
        /// </summary>
        [XmlElement("Block_Num")]
        public string Block_Num { get; set; }

        /// <summary>
        /// Cycle Number
        /// </summary>
        [XmlElement("Cycle_Num")]
        public string Cycle_Num { get; set; }

        /// <summary>
        /// Maximum Cycles
        /// </summary>
        [XmlElement("Max_Cycle")]
        public string Max_Cycle { get; set; }

        /// <summary>
        /// Last Hired Equipment Id
        /// </summary>
        [XmlElement("Last_Hired_Equip_Id")]
        public int? Last_Hired_Equip_Id { get; set; }

        /// <summary>
        /// Block Name
        /// </summary>
        [XmlElement("Block_Name")]
        public string Block_Name { get; set; }

        /// <summary>
        /// Closed
        /// </summary>
        [XmlElement("Closed")]
        public string Closed { get; set; }

        /// <summary>
        /// Closed Comment
        /// </summary>
        [XmlElement("Closed_Comments")]
        public string Closed_Comments { get; set; }

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
