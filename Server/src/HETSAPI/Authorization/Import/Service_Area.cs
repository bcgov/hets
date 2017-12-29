using System.Xml.Serialization;

namespace HETSAPI.Import
{
    // [XmlRoot("ArrayOfService_Area")]
    public class Service_Area
    {
        [XmlElement("Service_Area_Id")]
        public int Service_Area_Id { get; set; }

        [XmlElement("Service_Area_Cd")]
        public int Service_Area_Cd { get; set; }

        [XmlElement("Service_Area_Desc")]
        public string Service_Area_Desc { get; set; }

        [XmlElement("District_Area_Id")]
        public int District_Area_Id { get; set; }

        [XmlElement("Last_Year_End_Shift")]
        public string Last_Year_End_Shift { get; set; }

        [XmlElement("Address")]
        public string Address { get; set; }

        [XmlElement("Phone")]
        public string Phone { get; set; }

        [XmlElement("Fax ")]
        public string Fax { get; set; }

        [XmlElement("FiscalStart")]
        public string FiscalStart { get; set; }

        [XmlElement("FiscalEnd")]
        public string FiscalEnd { get; set; }

        [XmlElement("Created_Dt")]
        public string Created_Dt { get; set; }

        [XmlElement("Created_By")]
        public string Created_By { get; set; }     
    }
}
