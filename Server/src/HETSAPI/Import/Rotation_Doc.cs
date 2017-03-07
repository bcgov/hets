using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SchoolBusAPI.Import
{
    public class Rotation_Doc
    {        
        [XmlAttribute]
        public string Equip_Id { get; set; }

        [XmlAttribute]
        public string Note_Dt { get; set; }
        [XmlAttribute]
        public string Created_Dt { get; set; }

        [XmlAttribute]
        public string Service_Area_Id { get; set; }
        [XmlAttribute]
        public string Project_Id { get; set; }
        [XmlAttribute]
        public string Note_Type { get; set; }
        [XmlAttribute]
        public string Reason { get; set; }
        [XmlAttribute]
        public string Note_Id { get; set; }
        [XmlAttribute]
        public string Created_By { get; set; }       
    }
}
