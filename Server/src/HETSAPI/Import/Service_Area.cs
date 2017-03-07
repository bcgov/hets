using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SchoolBusAPI.Import
{
    public class Service_Area
    {
        [XmlAttribute]
        public string Service_Area_Id { get; set; }

        [XmlAttribute]
        public string Service_Area_Cd { get; set; }

        [XmlAttribute]
        public string Service_Area_Desc { get; set; }
        [XmlAttribute]
        public string District_Area_Id { get; set; }
        [XmlAttribute]
        public string Last_Year_End_Shift { get; set; }
        [XmlAttribute]
        public string Address { get; set; }
        [XmlAttribute]
        public string Phone { get; set; }
        [XmlAttribute]
        public string Fax { get; set; }
        [XmlAttribute]
        public string FiscalStart { get; set; }
        [XmlAttribute]
        public string FiscalEnd { get; set; }
        [XmlAttribute]
        public string Created_Dt { get; set; }
        [XmlAttribute]
        public string Created_By { get; set; }     
    }
}
