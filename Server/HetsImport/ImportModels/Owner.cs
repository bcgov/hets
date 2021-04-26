using System.Xml.Serialization;

namespace HetsImport.ImportModels
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
        /// Owner Code
        /// </summary>
        [XmlElement("Company_Name")]
        public string Company_Name { get; set; }

        /// <summary>
        /// DBA Name
        /// </summary>
        [XmlElement("Operating_AS")]
        public string Operating_AS { get; set; }

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
        /// Owner Phone Country Code 
        /// </summary>
        [XmlElement("Ph_Country_Code")]
        public string Ph_Country_Code { get; set; }

        /// <summary>
        /// Owner Phone Area Code 
        /// </summary>
        [XmlElement("Ph_Area_Code")]
        public string Ph_Area_Code { get; set; }

        /// <summary>
        /// Owner Phone Number 
        /// </summary>
        [XmlElement("Ph_Number")]
        public string Ph_Number { get; set; }

        /// <summary>
        /// Owner Phone Extension
        /// </summary>
        [XmlElement("Ph_Extension")]
        public string Ph_Extension { get; set; }

        /// <summary>
        /// Owner Fax Country Code 
        /// </summary>
        [XmlElement("Fax_Country_Code")]
        public string Fax_Country_Code { get; set; }

        /// <summary>
        /// Owner Fax Area Code 
        /// </summary>
        [XmlElement("Fax_Area_Code")]
        public string Fax_Area_Code { get; set; }

        /// <summary>
        /// Owner Fax Number 
        /// </summary>
        [XmlElement("Fax_Number")]
        public string Fax_Number { get; set; }

        /// <summary>
        /// Owner Fax Extension
        /// </summary>
        [XmlElement("Fax_Extension")]
        public string Fax_Extension { get; set; }

        /// <summary>
        /// Owner Cell Country Code 
        /// </summary>
        [XmlElement("Cell_Country_Code")]
        public string Cell_Country_Code { get; set; }

        /// <summary>
        /// Owner Cell Area Code 
        /// </summary>
        [XmlElement("Cell_Area_Code")]
        public string Cell_Area_Code { get; set; }

        /// <summary>
        /// Owner Cell Number 
        /// </summary>
        [XmlElement("Cell_Number")]
        public string Cell_Number { get; set; }

        /// <summary>
        /// Owner Cell Extension
        /// </summary>
        [XmlElement("Cell_Extension")]
        public string Cell_Extension { get; set; }

        /// <summary>
        /// Contact Country Code 
        /// </summary>
        [XmlElement("Contact_Country_Code")]
        public string Contact_Country_Code { get; set; }

        /// <summary>
        /// Contact Area Code 
        /// </summary>
        [XmlElement("Contact_Area_Code")]
        public string Contact_Area_Code { get; set; }

        /// <summary>
        /// Contact Number 
        /// </summary>
        [XmlElement("Contact_Number")]
        public string Contact_Number { get; set; }

        /// <summary>
        /// Contact Extension
        /// </summary>
        [XmlElement("Contact_Extension")]
        public string Contact_Extension { get; set; }         

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
        /// Archive Reason
        /// </summary>
        [XmlElement("Archive_Reason")]
        public string Archive_Reason { get; set; }

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
