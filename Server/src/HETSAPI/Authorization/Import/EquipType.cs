using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class EquipType
    {
        [XmlElement("Equip_Type_Id")]
        public int Equip_Type_Id { get; set; }

        [XmlElement("SubSystem_Id")]
        public int SubSystem_Id { get; set; }

        [XmlElement("Service_Area_Id")]
        public int Service_Area_Id { get; set; }

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

        [XmlElement("Modified_Dt")]
        public string Modified_Dt { get; set; }

        [XmlElement("Modified_By")]
        public string Modified_By { get; set; }

        public string ToDelimString(string delim)
        {
            string result = this.Equip_Type_Id.ToString();
            result += delim + (this.SubSystem_Id.ToString() == null ? " " : this.SubSystem_Id.ToString());
            result += delim + (this.Service_Area_Id.ToString() == null ? " " : this.Service_Area_Id.ToString());
            result += delim + (this.Equip_Type_Cd == null ? " " : this.Equip_Type_Cd);
            result += delim + (this.Equip_Type_Desc == null ? " " : this.Equip_Type_Desc);
            result += delim + (this.Equip_Rental_Rate_No == null ? " " : this.Equip_Rental_Rate_No);
            result += delim + (this.Equip_Rental_Rate_Page == null ? " " : this.Equip_Rental_Rate_Page);
            result += delim + (this.Max_Hours == null ? " " : this.Max_Hours);
            result += delim + (this.Extend_Hours == null ? " " : this.Extend_Hours);
            result += delim + (this.Max_Hours_Sub == null ? " " : this.Max_Hours_Sub);
            result += delim + (this.Second_Blk == null ? " " : this.Second_Blk);
            result += delim + (this.Created_Dt == null ? " " : this.Created_Dt);
            result += delim + (this.Created_By == null ? " " : this.Created_By);
            result += delim + (this.Modified_Dt == null ? " " : this.Modified_Dt);
            result += delim + (this.Modified_By == null ? " " : this.Modified_By);
            return result;
        }
    }
}
