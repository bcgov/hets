using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SchoolBusAPI.Import
{
    public class Rotation_Doc
    {        
        [XmlElement]
        public string Equip_Id { get; set; }

        [XmlElement]
        public string Note_Dt { get; set; }
        [XmlElement]
        public string Created_Dt { get; set; }

        [XmlElement]
        public string Service_Area_Id { get; set; }
        [XmlElement]
        public string Project_Id { get; set; }
        [XmlElement]
        public string Note_Type { get; set; }
        [XmlElement]
        public string Reason { get; set; }
        [XmlElement]
        public string Note_Id { get; set; }
        [XmlElement]
        public string Created_By { get; set; }       
    }
}
