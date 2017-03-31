using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class Block
    {
        [XmlElement]
        public string Area_Id { get; set; }

        [XmlElement]
        public string Equip_Type_Id { get; set; }

        [XmlElement]
        public string Block_Num { get; set; }

        [XmlElement]
        public string Cycle_Num { get; set; }

        [XmlElement]
        public string Max_Cycle { get; set; }

        [XmlElement]
        public string Last_Hired_Equip_Id { get; set; }

        [XmlElement]
        public string Block_Name { get; set; }

        [XmlElement]
        public string Closed { get; set; }

        [XmlElement]
        public string Closed_Comments { get; set; }

        [XmlElement]
        public string Created_Dt { get; set; }

        [XmlElement]
        public string Created_By { get; set; }
    
    }
}
