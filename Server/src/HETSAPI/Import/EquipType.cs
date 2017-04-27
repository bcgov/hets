using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class EquipType
    {
        [XmlElement("Equip_Type_Id")]
        public string Equip_Type_Id { get; set; }

        [XmlElement("SubSystem_Id")]
        public string SubSystem_Id { get; set; }

        [XmlElement("Service_Area_Id")]
        public string Service_Area_Id { get; set; }

        [XmlElement("Equip_Type_Cd")]
        public string Equip_Type_Cd { get; set; }

        [XmlElement("Equip_Type_Desc")]
        public string Equip_Type_Desc { get; set; }

        [XmlElement("Equip_Rental_Rate_No")]
        public string Equip_Rental_Rate_No { get; set; }

        [XmlElement("Equip_Rental_Rate_Page")]
        public string Equip_Rental_Rate_Page { get; set; }

        [XmlElement("Max_Hours")]
        public string Max_Hours { get; set; }

        [XmlElement("Extend_Hours")]
        public string Extend_Hours { get; set; }

        [XmlElement("Max_Hours_Sub")]
        public string Max_Hours_Sub { get; set; }

        [XmlElement("Second_Blk")]
        public string Second_Blk { get; set; }

        [XmlElement("Created_Dt")]
        public string Created_Dt { get; set; }

        [XmlElement("Created_By")]
        public string Created_By { get; set; }
    }
}
