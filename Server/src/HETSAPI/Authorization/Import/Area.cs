using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HETSAPI.Import
{
   // [XmlRoot("ArrayOfArea"), XmlType("ArrayOfArea")]
    public class Area
    {
        [XmlElement("Area_Id")]
        public int Area_Id { get; set; }

        [XmlElement("Area_Cd")]
        public string Area_Cd { get; set; }

        [XmlElement("Area_Desc")]
        public string Area_Desc { get; set; }

        [XmlElement("Service_Area_Id")]
        public int Service_Area_Id { get; set; }

        [XmlElement("Created_Dt")]
        public string Created_Dt { get; set; }

        [XmlElement("Created_By")]
        public string Created_By { get; set; }        
    }
}
