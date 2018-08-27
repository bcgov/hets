using System.Xml.Serialization;

namespace HetsImport.ImportModels
{
    /// <summary>
    /// HETS District Import Model
    /// </summary>
    public class HetsDistrict
    {
        /// <summary>
        /// District Id
        /// </summary>
        [XmlElement("District_Id")]
        public int District_Id { get; set; }

        /// <summary>
        /// Ministry District Id
        /// </summary>
        [XmlElement("Ministry_District_Id")]
        public int Ministry_District_Id { get; set; }

        /// <summary>
        /// District Name
        /// </summary>
        [XmlElement("District_Name")]
        public string Name { get; set; }

        /// <summary>
        /// Region Id
        /// </summary>
        [XmlElement("Region_ID")]
        public int Region_ID { get; set; }

        /// <summary>
        /// District Number
        /// </summary>
        [XmlElement("District_Number")]
        public string District_Number { get; set; }
    }
}

