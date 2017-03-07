using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SchoolBusAPI.Import
{
    public class Block
    {
        [XmlAttribute]
        public string Area_Id { get; set; }

        [XmlAttribute]
        public string Equip_Type_Id { get; set; }

        [XmlAttribute]
        public string Block_Num { get; set; }

        [XmlAttribute]
        public string Cycle_Num { get; set; }

        [XmlAttribute]
        public string Max_Cycle { get; set; }

        [XmlAttribute]
        public string Last_Hired_Equip_Id { get; set; }

        [XmlAttribute]
        public string Block_Name { get; set; }

        [XmlAttribute]
        public string Closed { get; set; }

        [XmlAttribute]
        public string Closed_Comments { get; set; }

        [XmlAttribute]
        public string Created_Dt { get; set; }

        [XmlAttribute]
        public string Created_By { get; set; }
    
    }
}
