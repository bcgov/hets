using System.Xml.Serialization;

namespace HETSAPI.ImportModels
{
    /// <summary>
    /// Equip(ment) Import Model
    /// </summary>
    public class Equip
    {
        /// <summary>
        /// Equipment Id
        /// </summary>
        [XmlElement("Equip_Id")]
        public int Equip_Id { get; set; }

        /// <summary>
        /// Area Id
        /// </summary>
        [XmlElement("Area_Id")]
        public int? Area_Id { get; set; }

        /// <summary>
        /// Equipment Type Id
        /// </summary>
        [XmlElement("Equip_Type_Id")]
        public int? Equip_Type_Id { get; set; }

        /// <summary>
        /// Owner Id
        /// </summary>
        [XmlElement("Owner_Popt_Id")]
        public int Owner_Popt_Id { get; set; }

        /// <summary>
        /// Equipment Code
        /// </summary>
        [XmlElement("Equip_Cd")]
        public string Equip_Cd { get; set; }

        /// <summary>
        /// Approved Date
        /// </summary>
        [XmlElement("Approved_Dt")]
        public string Approved_Dt { get; set; }

        /// <summary>
        /// Received Date
        /// </summary>
        [XmlElement("Received_Dt")]
        public string Received_Dt { get; set; }

        /// <summary>
        /// Address 1
        /// </summary>
        [XmlElement("Addr1")]
        public string Addr1 { get; set; }

        /// <summary>
        /// Address 2
        /// </summary>
        [XmlElement("Addr2")]
        public string Addr2 { get; set; }

        /// <summary>
        /// Address 3
        /// </summary>
        [XmlElement("Addr3")]
        public string Addr3 { get; set; }

        /// <summary>
        /// Address 4
        /// </summary>
        [XmlElement("Addr4")]
        public string Addr4 { get; set; }

        /// <summary>
        /// City
        /// </summary>
        [XmlElement("City")]
        public string City { get; set; }

        /// <summary>
        /// Postal
        /// </summary>
        [XmlElement("Postal")]
        public string Postal { get; set; }

        /// <summary>
        /// Block Number
        /// </summary>
        [XmlElement("Block_Num")]
        public string Block_Num { get; set; }

        /// <summary>
        /// Comment
        /// </summary>
        [XmlElement("Comment")]
        public string Comment { get; set; }

        /// <summary>
        /// Cycle Hours Work
        /// </summary>
        [XmlElement("Cycle_Hrs_Wrk")]
        public string Cycle_Hrs_Wrk { get; set; }

        /// <summary>
        /// Frozen Out
        /// </summary>
        [XmlElement("Frozen_Out")]
        public string Frozen_Out { get; set; }

        /// <summary>
        /// Last Date
        /// </summary>
        [XmlElement("Last_Dt")]
        public string Last_Dt { get; set; }

        /// <summary>
        /// License
        /// </summary>
        [XmlElement("Licence")]
        public string Licence { get; set; }

        /// <summary>
        /// Make
        /// </summary>
        [XmlElement("Make")]
        public string Make { get; set; }

        /// <summary>
        /// Model
        /// </summary>
        [XmlElement("Model")]
        public string Model { get; set; }

        /// <summary>
        /// Year
        /// </summary>
        [XmlElement("Year")]
        public string Year { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [XmlElement("Type")]
        public string Type { get; set; }

        /// <summary>
        /// Number of Years
        /// </summary>
        [XmlElement("Num_Years")]
        public string Num_Years { get; set; }

        /// <summary>
        /// Operator
        /// </summary>
        [XmlElement("Operator")]
        public string Operator { get; set; }

        /// <summary>
        /// Pay Rate
        /// </summary>
        [XmlElement("Pay_Rate")]
        public string Pay_Rate { get; set; }

        /// <summary>
        /// Project Id
        /// </summary>
        [XmlElement("Project_Id")]
        public string Project_Id { get; set; }

        /// <summary>
        /// Refuse Rate
        /// </summary>
        [XmlElement("Refuse_Rate")]
        public string Refuse_Rate { get; set; }

        /// <summary>
        /// Seniority
        /// </summary>
        [XmlElement("Seniority")]
        public string Seniority { get; set; }

        /// <summary>
        /// Serial Number
        /// </summary>
        [XmlElement("Serial_Num")]
        public string Serial_Num { get; set; }

        /// <summary>
        /// Size
        /// </summary>
        [XmlElement("Size")]
        public string Size { get; set; }

        /// <summary>
        /// Working
        /// </summary>
        [XmlElement("Working")]
        public string Working { get; set; }

        /// <summary>
        /// Year End Reg Area
        /// </summary>
        [XmlElement("Year_End_Reg")]
        public string Year_End_Reg { get; set; }

        /// <summary>
        /// Previous Reg Area
        /// </summary>
        [XmlElement("Prev_Reg_Area")]
        public string Prev_Reg_Area { get; set; }

        /// <summary>
        /// Service Hours YTD
        /// </summary>
        [XmlElement("YTD")]
        public string YTD { get; set; }

        /// <summary>
        /// Service Hours Y - 1
        /// </summary>
        [XmlElement("YTD1")]
        public string YTD1 { get; set; }

        /// <summary>
        /// Service Hours Y - 2
        /// </summary>
        [XmlElement("YTD2")]
        public string YTD2 { get; set; }

        /// <summary>
        /// Service Hours Y - 3
        /// </summary>
        [XmlElement("YTD3")]
        public string YTD3 { get; set; }

        /// <summary>
        /// Status Code
        /// </summary>
        [XmlElement("Status_Cd")]
        public string Status_Cd { get; set; }

        /// <summary>
        /// Archive Code
        /// </summary>
        [XmlElement("Archive_Cd")]
        public string Archive_Cd { get; set; }

        /// <summary>
        /// Archive Reason
        /// </summary>
        [XmlElement("Archive_Reason")]
        public string Archive_Reason { get; set; }

        /// <summary>
        /// Reg Dump Truck
        /// </summary>
        [XmlElement("Reg_Dump_Trk")]
        public string Reg_Dump_Trk { get; set; }

        /// <summary>
        /// Created Date
        /// </summary>
        [XmlElement("Created_Dt")]
        public string Created_Dt { get; set; }

        /// <summary>
        /// Created By
        /// </summary>
        [XmlElement("Created_By")]
        public string Created_By { get; set; }

        /// <summary>
        /// Modified Date
        /// </summary>
        [XmlElement("Modified_Dt")]
        public string Modified_Dt { get; set; }

        /// <summary>
        /// Modified By
        /// </summary>
        [XmlElement("Modified_By")]
        public string Modified_By { get; set; }
    }
}
