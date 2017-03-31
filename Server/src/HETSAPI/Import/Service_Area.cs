using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class Service_Area
    {
        [XmlElement]
        public int Service_Area_Id { get; set; }

        [XmlElement]
        public int Service_Area_Cd { get; set; }

        [XmlElement]
        public string Service_Area_Desc { get; set; }
        [XmlElement]
        public int District_Area_Id { get; set; }
        [XmlElement]
        public string Last_Year_End_Shift { get; set; }
        [XmlElement]
        public string Address { get; set; }
        [XmlElement]
        public string Phone { get; set; }
        [XmlElement]
        public string Fax { get; set; }
        [XmlElement]
        public string FiscalStart { get; set; }
        [XmlElement]
        public string FiscalEnd { get; set; }
        [XmlElement]
        public string Created_Dt { get; set; }
        [XmlElement]
        public string Created_By { get; set; }     
    }
}
