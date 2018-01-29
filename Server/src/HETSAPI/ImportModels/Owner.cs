using System.Xml.Serialization;

namespace HETSAPI.ImportModels
{
    /// <summary>
    /// Owner Import Model
    /// </summary>
    public class Owner
    {
        /// <summary>
        /// Popt Id
        /// </summary>
        [XmlElement("Popt_Id")]
        public int Popt_Id { get; set; }

        /// <summary>
        /// Area Id
        /// </summary>
        [XmlElement("Area_Id")]
        public int Area_Id { get; set; }

        /// <summary>
        /// Owner Code
        /// </summary>
        [XmlElement("Owner_Cd")]
        public string Owner_Cd { get; set; }

        /// <summary>
        /// Owner First Name
        /// </summary>
        [XmlElement("Owner_First_Name")]
        public string Owner_First_Name { get; set; }

        /// <summary>
        /// Owner Last Name
        /// </summary>
        [XmlElement("Owner_Last_Name")]
        public string Owner_Last_Name { get; set; }

        /// <summary>
        /// Owner Contact Person
        /// </summary>
        [XmlElement("Contact_Person")]
        public string Contact_Person { get; set; }

        /// <summary>
        /// Local Area
        /// </summary>
        [XmlElement("Local_To_Area")]
        public string Local_To_Area { get; set; }

        /// <summary>
        /// Maintenance Contractor
        /// </summary>
        [XmlElement("Maintenance_Contractor")]
        public string Maintenance_Contractor { get; set; }

        /// <summary>
        /// Comment
        /// </summary>
        [XmlElement("Comment")]
        public string Comment { get; set; }

        /// <summary>
        /// WCB Number
        /// </summary>
        [XmlElement("WCB_Num")]
        public string WCB_Num { get; set; }

        /// <summary>
        /// WCB Expiry Date
        /// </summary>
        [XmlElement("WCB_Expiry_Dt")]
        public string WCB_Expiry_Dt { get; set; }

        /// <summary>
        /// CGL Company
        /// </summary>
        [XmlElement("CGL_company")]
        public string CGL_Company { get; set; }

        /// <summary>
        /// CGL Policy
        /// </summary>
        [XmlElement("CGL_Policy")]
        public string CGL_Policy { get; set; }

        /// <summary>
        /// CGL Start Date
        /// </summary>
        [XmlElement("CGL_Start_Dt")]
        public string CGL_Start_Dt { get; set; }

        /// <summary>
        /// CGL End Date
        /// </summary>
        [XmlElement("CGL_End_Dt")]
        public string CGL_End_Dt { get; set; }

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
        /// Service Area Id
        /// </summary>
        [XmlElement("Service_Area_Id")]
        public string Service_Area_Id { get; set; }

        /// <summary>
        /// Selected Service Atea Id
        /// </summary>
        [XmlElement("Selected_Service_Area_Id")]
        public string Selected_Service_Area_Id { get; set; }

        /// <summary>
        /// Created By
        /// </summary>
        [XmlElement("Created_By")]
        public string Created_By { get; set; }

        /// <summary>
        /// Created Date
        /// </summary>
        [XmlElement("Created_Dt")]
        public string Created_Dt { get; set; }

        /// <summary>
        /// Modified By
        /// </summary>
        [XmlElement("Modified_By")]
        public string Modified_By { get; set; }

        /// <summary>
        /// Modified Date
        /// </summary>
        [XmlElement("Modified_Dt")]
        public string Modified_Dt { get; set; }
    }
}
