using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class Equip
    {
        [XmlElement("Equip_Id")]
        public string Equip_Id { get; set; }

        [XmlElement("Area_Id")]
        public string Area_Id { get; set; }

        [XmlElement("Equip_Type_Id")]
        public string Equip_Type_Id { get; set; }

        [XmlElement("Owner_Popt_Id")]
        public string Owner_Popt_Id { get; set; }

        [XmlElement("Equip_Cd")]
        public string Equip_Cd { get; set; }

        [XmlElement("Approved_Dt")]
        public string Approved_Dt { get; set; }

        [XmlElement("Received_Dt")]
        public string Received_Dt { get; set; }

        [XmlElement("Addr1")]
        public string Addr1 { get; set; }

        [XmlElement("Addr2")]
        public string Addr2 { get; set; }

        [XmlElement("Addr3")]
        public string Addr3 { get; set; }

        [XmlElement("Addr4")]
        public string Addr4 { get; set; }

        [XmlElement("City")]
        public string City { get; set; }

        [XmlElement("Postal")]
        public string Postal { get; set; }

        [XmlElement("Block_Num")]
        public string Block_Num { get; set; }

        [XmlElement("Comment")]
        public string Comment { get; set; }

        [XmlElement("Cycle_Hrs_Wrk")]
        public string Cycle_Hrs_Wrk { get; set; }

        [XmlElement("Frozen_Out")]
        public string Frozen_Out { get; set; }

        [XmlElement("Last_Dt")]
        public string Last_Dt { get; set; }

        [XmlElement("Licence")]
        public string Licence { get; set; }

        [XmlElement("Make")]
        public string Make { get; set; }

        [XmlElement("Model")]
        public string Model { get; set; }

        [XmlElement("Year")]
        public string Year { get; set; }

        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlElement("Num_Years")]
        public string Num_Years { get; set; }

        [XmlElement("Operator")]
        public string Operator { get; set; }

        [XmlElement("Pay_Rate")]
        public string Pay_Rate { get; set; }

        [XmlElement("Project_Id")]
        public string Project_Id { get; set; }

        [XmlElement("Refuse_Rate")]
        public string Refuse_Rate { get; set; }

        [XmlElement("Seniorityd")]
        public string Seniority { get; set; }

        [XmlElement("Serial_Num")]
        public string Serial_Num { get; set; }

        [XmlElement("Size")]
        public string Size { get; set; }

        [XmlElement("Working")]
        public string Working { get; set; }

        [XmlElement("Year_End_Reg")]
        public string Year_End_Reg { get; set; }

        [XmlElement("Prev_Reg_Area")]
        public string Prev_Reg_Area { get; set; }

        [XmlElement("YTD")]
        public string YTD { get; set; }

        [XmlElement("YTD1")]
        public string YTD1 { get; set; }

        [XmlElement("YTD2")]
        public string YTD2 { get; set; }

        [XmlElement("YTD3")]
        public string YTD3 { get; set; }

        [XmlElement("Status_Cd")]
        public string Status_Cd { get; set; }

        [XmlElement("Archive_Cd")]
        public string Archive_Cd { get; set; }

        [XmlElement("Archive_Reason")]
        public string Archive_Reason { get; set; }

        [XmlElement("Reg_Dump_Trk")]
        public string Reg_Dump_Trk { get; set; }

        [XmlElement("Created_Dt")]
        public string Created_Dt { get; set; }

        [XmlElement("Created_By")]
        public string Created_By { get; set; }

        [XmlElement("Modified_Dt")]
        public string Modified_Dt { get; set; }

        [XmlElement("Modified_By")]
        public string Modified_By { get; set; }
    }
}
