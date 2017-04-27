using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class Block
    {
        [XmlElement("Area_Id")]
        public string Area_Id { get; set; }

        [XmlElement("Equip_Type_Id")]
        public string Equip_Type_Id { get; set; }

        [XmlElement("Block_Num")]
        public string Block_Num { get; set; }

        [XmlElement("Cycle_Num")]
        public string Cycle_Num { get; set; }

        [XmlElement("Max_Cycle")]
        public string Max_Cycle { get; set; }

        [XmlElement("Last_Hired_Equip_Id")]
        public string Last_Hired_Equip_Id { get; set; }

        [XmlElement("Block_Name")]
        public string Block_Name { get; set; }

        [XmlElement("Closed")]
        public string Closed { get; set; }

        [XmlElement("Closed_Comments")]
        public string Closed_Comments { get; set; }

        [XmlElement("Created_Dt")]
        public string Created_Dt { get; set; }

        [XmlElement("Created_By")]
        public string Created_By { get; set; }

    }
}
