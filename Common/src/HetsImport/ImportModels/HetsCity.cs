using System.Xml.Serialization;

namespace HetsImport.ImportModels
{
    /// <summary>
    /// HETS City Import Mapping
    /// </summary>
    [XmlType(TypeName = "HETS_City")]
    public class HetsCity
    {
        /// <summary>
        /// City Id
        /// </summary>
        [XmlElement("City_Id")]
        public int City_Id { get; set; }

        /// <summary>
        /// Service Area Id
        /// </summary>
        [XmlElement("Service_Area_Id")]
        public int Service_Area_Id { get; set; }

        /// <summary>
        /// Sequence Number
        /// </summary>
        [XmlElement("Seq_Num")]
        public int Seq_Num { get; set; }

        /// <summary>
        /// City Name
        /// </summary>
        [XmlElement("City")]
        public string Name { get; set; }               
    }
}
