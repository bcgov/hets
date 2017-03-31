using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class EquipUsage
    {
        [XmlElement]
        public string Equip_Id { get; set; }

        [XmlElement]
        public string Project_Id { get; set; }

        [XmlElement]
        public string Service_Area_Id { get; set; }

        [XmlElement]
        public string Worked_Dt { get; set; }

        [XmlElement]
        public string Entered_Dt { get; set; }
        [XmlElement]
        public string Hours { get; set; }
        [XmlElement]
        public string Rate { get; set; }
        [XmlElement]
        public string Hours2 { get; set; }
        [XmlElement]
        public string Rate2 { get; set; }
        [XmlElement]
        public string Hours3 { get; set; }
        [XmlElement]
        public string Rate3 { get; set; }
        [XmlElement]
        public string Created_Dt { get; set; }
        [XmlElement]
        public string Created_By { get; set; }
    }
}
