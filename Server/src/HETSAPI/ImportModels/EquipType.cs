using System.Xml.Serialization;

namespace HETSAPI.ImportModels
{
    /// <summary>
    /// Equip(ment) Type Import Mapping
    /// </summary>
    [XmlType(TypeName = "Equip_Type")]
    public class EquipType
    {
        /// <summary>
        /// Equipment Type Id
        /// </summary>
        [XmlElement("Equip_Type_Id")]
        public int Equip_Type_Id { get; set; }

        /// <summary>
        /// Subsystem Id
        /// </summary>
        [XmlElement("SubSystem_Id")]
        public int SubSystem_Id { get; set; }

        /// <summary>
        /// Service Area Id
        /// </summary>
        [XmlElement("Service_Area_Id")]
        public int Service_Area_Id { get; set; }

        /// <summary>
        /// Equipment Type Code
        /// </summary>
        [XmlElement("Equip_Type_Cd")]
        public string Equip_Type_Cd { get; set; }

        /// <summary>
        /// Equipment Type Code
        /// </summary>
        [XmlElement("Equip_Type_Desc")]
        public string Equip_Type_Desc { get; set; }

        /// <summary>
        /// Equipment Rental Rate Number
        /// </summary>
        [XmlElement("Equip_Rental_Rate_No")]
        public string Equip_Rental_Rate_No { get; set; }

        /// <summary>
        /// Equipment Rental Rate Page
        /// </summary>
        [XmlElement("Equip_Rental_Rate_Page")]
        public string Equip_Rental_Rate_Page { get; set; }

        /// <summary>
        /// Maximum Hours
        /// </summary>
        [XmlElement("Max_Hours")]
        public string Max_Hours { get; set; }

        /// <summary>
        /// Extended Hours
        /// </summary>
        [XmlElement("Extend_Hours")]
        public string Extend_Hours { get; set; }

        /// <summary>
        /// Maximum Hours Sub
        /// </summary>
        [XmlElement("Max_Hours_Sub")]
        public string Max_Hours_Sub { get; set; }

        /// <summary>
        /// Second Blk
        /// </summary>
        [XmlElement("Second_Blk")]
        public string Second_Blk { get; set; }

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

        /// <summary>
        /// Convert To String
        /// </summary>
        /// <param name="delim"></param>
        /// <returns></returns>
        public string ToDelimString(string delim)
        {
            string result = Equip_Type_Id.ToString();
            result += delim + (SubSystem_Id.ToString() == null ? " " : SubSystem_Id.ToString());
            result += delim + (Service_Area_Id.ToString() == null ? " " : Service_Area_Id.ToString());
            result += delim + (Equip_Type_Cd ?? " ");
            result += delim + (Equip_Type_Desc ?? " ");
            result += delim + (Equip_Rental_Rate_No ?? " ");
            result += delim + (Equip_Rental_Rate_Page ?? " ");
            result += delim + (Max_Hours ?? " ");
            result += delim + (Extend_Hours ?? " ");
            result += delim + (Max_Hours_Sub ?? " ");
            result += delim + (Second_Blk ?? " ");
            result += delim + (Created_Dt ?? " ");
            result += delim + (Created_By ?? " ");
            result += delim + (Modified_Dt ?? " ");
            result += delim + (Modified_By ?? " ");
            return result;
        }
    }
}
