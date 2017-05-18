using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class EquipUsage
    {
        [XmlElement("Equip_Id")]
        public int? Equip_Id { get; set; }

        [XmlElement("Project_Id")]
        public int? Project_Id { get; set; }

        [XmlElement("Service_Area_Id")]
        public int? Service_Area_Id { get; set; }

        [XmlElement("Worked_Dt")]
        public string Worked_Dt { get; set; }

        [XmlElement("Entered_Dt")]
        public string Entered_Dt { get; set; }

        [XmlElement("Hours")]
        public string Hours { get; set; }

        [XmlElement("Rate")]
        public string Rate { get; set; }

        [XmlElement("Hours2")]
        public string Hours2 { get; set; }

        [XmlElement("Rate2")]
        public string Rate2 { get; set; }

        [XmlElement("Hours3")]
        public string Hours3 { get; set; }

        [XmlElement("Rate3")]
        public string Rate3 { get; set; }

        [XmlElement("Created_Dt")]
        public string Created_Dt { get; set; }

        [XmlElement("Created_By")]
        public string Created_By { get; set; }
    }
}
