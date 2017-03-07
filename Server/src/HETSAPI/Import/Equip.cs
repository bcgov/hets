using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SchoolBusAPI.Import
{
    public class Equip
    {        
        [XmlAttribute]
        public string Equip_Id { get; set; }

        [XmlAttribute]
        public string Area_Id { get; set; }

        [XmlAttribute]
        public string Equip_Type_Id { get; set; }

        [XmlAttribute]
        public string Owner_Popt_Id { get; set; }

        [XmlAttribute]
        public string Equip_Cd { get; set; }

        [XmlAttribute]
        public string Approved_Dt { get; set; }

        [XmlAttribute]
        public string Received_Dt { get; set; }

        [XmlAttribute]
        public string Addr1 { get; set; }

        [XmlAttribute]
        public string Addr2 { get; set; }

        [XmlAttribute]
        public string Addr3 { get; set; }

        [XmlAttribute]
        public string Addr4 { get; set; }

        [XmlAttribute]
        public string City { get; set; }

        [XmlAttribute]
        public string Postal { get; set; }

        [XmlAttribute]
        public string Block_Num { get; set; }

        [XmlAttribute]
        public string Comment { get; set; }

        [XmlAttribute]
        public string Cycle_Hrs_Wrk { get; set; }

        [XmlAttribute]
        public string Frozen_Out { get; set; }

        [XmlAttribute]
        public string Last_Dt { get; set; }

        [XmlAttribute]
        public string Licence { get; set; }

        [XmlAttribute]
        public string Make { get; set; }

        [XmlAttribute]
        public string Model { get; set; }

        [XmlAttribute]
        public string Year { get; set; }

        [XmlAttribute]
        public string Type { get; set; }

        [XmlAttribute]
        public string Num_Years { get; set; }

        [XmlAttribute]
        public string Operator { get; set; }

        [XmlAttribute]
        public string Pay_Rate { get; set; }

        [XmlAttribute]
        public string Project_Id { get; set; }

        [XmlAttribute]
        public string Refuse_Rate { get; set; }

        [XmlAttribute]
        public string Seniority { get; set; }

        [XmlAttribute]
        public string Serial_Num { get; set; }

        [XmlAttribute]
        public string Size { get; set; }

        [XmlAttribute]
        public string Working { get; set; }

        [XmlAttribute]
        public string Year_End_Reg { get; set; }

        [XmlAttribute]
        public string Prev_Reg_Area { get; set; }

        [XmlAttribute]
        public string YTD { get; set; }

        [XmlAttribute]
        public string YTD1 { get; set; }

        [XmlAttribute]
        public string YTD2 { get; set; }

        [XmlAttribute]
        public string YTD3 { get; set; }

        [XmlAttribute]
        public string Status_Cd { get; set; }

        [XmlAttribute]
        public string Archive_Cd { get; set; }

        [XmlAttribute]
        public string Archive_Reason { get; set; }

        [XmlAttribute]
        public string Reg_Dump_Trk { get; set; }

        [XmlAttribute]
        public string Created_Dt { get; set; }

        [XmlAttribute]        
        public string Created_By { get; set; }

        [XmlAttribute]
        public string Modified_Dt { get; set; }

        [XmlAttribute]
        public string Modified_By { get; set; }
    }
}
