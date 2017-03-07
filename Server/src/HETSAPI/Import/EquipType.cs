using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SchoolBusAPI.Import
{
    public class EquipType
    {
        [XmlAttribute]
        public string Equip_Type_Id { get; set; }

        [XmlAttribute]
        public string SubSystem_Id { get; set; }

        [XmlAttribute]
        public string Service_Area_Id { get; set; }

        [XmlAttribute]
        public string Equip_Type_Cd { get; set; }

        [XmlAttribute]
        public string Equip_Type_Desc { get; set; }

        [XmlAttribute]
        public string Equip_Rental_Rate_No { get; set; }

        [XmlAttribute]
        public string Equip_Rental_Rate_Page { get; set; }

        [XmlAttribute]
        public string Max_Hours { get; set; }

        [XmlAttribute]
        public string Extend_Hours { get; set; }

        [XmlAttribute]
        public string Max_Hours_Sub { get; set; }

        [XmlAttribute]
        public string Second_Blk { get; set; }
        [XmlAttribute]
        public string Created_Dt { get; set; }
      


    }
}
