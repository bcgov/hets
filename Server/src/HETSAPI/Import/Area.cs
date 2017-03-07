using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SchoolBusAPI.Import
{
    public class Area
    {
        [XmlAttribute]
        public string Area_Id { get; set; }
     
        [XmlAttribute]
        public string Area_Cd { get; set; }

        [XmlAttribute]
        public string Area_Desc { get; set; }

        [XmlAttribute]
        public string Service_Area_Id { get; set; }

        [XmlAttribute]
        public string Created_Dt { get; set; }

        [XmlAttribute]
        public string Created_By { get; set; }        
    }
}
