using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class EquipType
    {
        [XmlElement]
        public string Equip_Type_Id { get; set; }

        [XmlElement]
        public string SubSystem_Id { get; set; }

        [XmlElement]
        public string Service_Area_Id { get; set; }

        [XmlElement]
        public string Equip_Type_Cd { get; set; }

        [XmlElement]
        public string Equip_Type_Desc { get; set; }

        [XmlElement]
        public string Equip_Rental_Rate_No { get; set; }

        [XmlElement]
        public string Equip_Rental_Rate_Page { get; set; }

        [XmlElement]
        public string Max_Hours { get; set; }

        [XmlElement]
        public string Extend_Hours { get; set; }

        [XmlElement]
        public string Max_Hours_Sub { get; set; }

        [XmlElement]
        public string Second_Blk { get; set; }
        [XmlElement]
        public string Created_Dt { get; set; }
      


    }
}
