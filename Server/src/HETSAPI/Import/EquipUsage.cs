using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SchoolBusAPI.Import
{
    public class EquipUsage
    {
        [XmlAttribute]
        public string Equip_Id { get; set; }

        [XmlAttribute]
        public string Project_Id { get; set; }

        [XmlAttribute]
        public string Service_Area_Id { get; set; }

        [XmlAttribute]
        public string Worked_Dt { get; set; }

        [XmlAttribute]
        public string Entered_Dt { get; set; }
        [XmlAttribute]
        public string Hours { get; set; }
        [XmlAttribute]
        public string Rate { get; set; }
        [XmlAttribute]
        public string Hours2 { get; set; }
        [XmlAttribute]
        public string Rate2 { get; set; }
        [XmlAttribute]
        public string Hours3 { get; set; }
        [XmlAttribute]
        public string Rate3 { get; set; }
        [XmlAttribute]
        public string Created_Dt { get; set; }
        [XmlAttribute]
        public string Created_By { get; set; }
    }
}
