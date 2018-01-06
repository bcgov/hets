using System.Xml.Serialization;

namespace HETSAPI.ImportModels
{
    /// <summary>
    /// Service Area Import Model
    /// </summary>
    public class Service_Area
    {
        /// <summary>
        /// Service Area Id
        /// </summary>
        [XmlElement("Service_Area_Id")]
        public int Service_Area_Id { get; set; }

        /// <summary>
        /// Service Area Code
        /// </summary>
        [XmlElement("Service_Area_Cd")]
        public int Service_Area_Cd { get; set; }

        /// <summary>
        /// Service Area Description
        /// </summary>
        [XmlElement("Service_Area_Desc")]
        public string Service_Area_Desc { get; set; }

        /// <summary>
        /// District Area Id
        /// </summary>
        [XmlElement("District_Area_Id")]
        public int District_Area_Id { get; set; }

        /// <summary>
        /// Last Year End Shift
        /// </summary>
        [XmlElement("Last_Year_End_Shift")]
        public string Last_Year_End_Shift { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        [XmlElement("Address")]
        public string Address { get; set; }

        /// <summary>
        /// Phone
        /// </summary>
        [XmlElement("Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// Fax
        /// </summary>
        [XmlElement("Fax ")]
        public string Fax { get; set; }

        /// <summary>
        /// Fiscal Start
        /// </summary>
        [XmlElement("FiscalStart")]
        public string FiscalStart { get; set; }

        /// <summary>
        /// Fiscal End
        /// </summary>
        [XmlElement("FiscalEnd")]
        public string FiscalEnd { get; set; }

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
