using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class Equip
    {        
        [XmlElement]
        public string Equip_Id { get; set; }

        [XmlElement]
        public string Area_Id { get; set; }

        [XmlElement]
        public string Equip_Type_Id { get; set; }

        [XmlElement]
        public string Owner_Popt_Id { get; set; }

        [XmlElement]
        public string Equip_Cd { get; set; }

        [XmlElement]
        public string Approved_Dt { get; set; }

        [XmlElement]
        public string Received_Dt { get; set; }

        [XmlElement]
        public string Addr1 { get; set; }

        [XmlElement]
        public string Addr2 { get; set; }

        [XmlElement]
        public string Addr3 { get; set; }

        [XmlElement]
        public string Addr4 { get; set; }

        [XmlElement]
        public string City { get; set; }

        [XmlElement]
        public string Postal { get; set; }

        [XmlElement]
        public string Block_Num { get; set; }

        [XmlElement]
        public string Comment { get; set; }

        [XmlElement]
        public string Cycle_Hrs_Wrk { get; set; }

        [XmlElement]
        public string Frozen_Out { get; set; }

        [XmlElement]
        public string Last_Dt { get; set; }

        [XmlElement]
        public string Licence { get; set; }

        [XmlElement]
        public string Make { get; set; }

        [XmlElement]
        public string Model { get; set; }

        [XmlElement]
        public string Year { get; set; }

        [XmlElement]
        public string Type { get; set; }

        [XmlElement]
        public string Num_Years { get; set; }

        [XmlElement]
        public string Operator { get; set; }

        [XmlElement]
        public string Pay_Rate { get; set; }

        [XmlElement]
        public string Project_Id { get; set; }

        [XmlElement]
        public string Refuse_Rate { get; set; }

        [XmlElement]
        public string Seniority { get; set; }

        [XmlElement]
        public string Serial_Num { get; set; }

        [XmlElement]
        public string Size { get; set; }

        [XmlElement]
        public string Working { get; set; }

        [XmlElement]
        public string Year_End_Reg { get; set; }

        [XmlElement]
        public string Prev_Reg_Area { get; set; }

        [XmlElement]
        public string YTD { get; set; }

        [XmlElement]
        public string YTD1 { get; set; }

        [XmlElement]
        public string YTD2 { get; set; }

        [XmlElement]
        public string YTD3 { get; set; }

        [XmlElement]
        public string Status_Cd { get; set; }

        [XmlElement]
        public string Archive_Cd { get; set; }

        [XmlElement]
        public string Archive_Reason { get; set; }

        [XmlElement]
        public string Reg_Dump_Trk { get; set; }

        [XmlElement]
        public string Created_Dt { get; set; }

        [XmlElement]        
        public string Created_By { get; set; }

        [XmlElement]
        public string Modified_Dt { get; set; }

        [XmlElement]
        public string Modified_By { get; set; }
    }
}
