using System.Xml.Serialization;

namespace HETSAPI.ImportModels
{
    /// <summary>
    /// HETS Region Import Model
    /// </summary>
    public class HETS_Region
    {
        /// <summary>
        /// Region Id
        /// </summary>
        [XmlElement("Region_Id")]
        public int Region_Id { get; set; }

        /// <summary>
        /// Ministry Region Id
        /// </summary>
        [XmlElement("Ministry_Region_Id")]
        public int Ministry_Region_Id { get; set; }

        /// <summary>
        /// Region Name
        /// </summary>
        [XmlElement("Region_Name")]
        public string Name { get; set; }

        /// <summary>
        /// Region Number
        /// </summary>
        [XmlElement("Region_Number")]
        public int Region_Number { get; set; }
    }
}

