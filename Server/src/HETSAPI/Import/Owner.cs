using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class Owner
    {
        [XmlElement("Popt_Id")]
        public int Popt_Id { get; set; }

        [XmlElement("Area_Id")]
        public int Area_Id { get; set; }

        [XmlElement("Owner_Cd")]
        public string Owner_Cd { get; set; }

        [XmlElement("Owner_First_Name")]
        public string Owner_First_Name { get; set; }

        [XmlElement("Owner_Last_Name")]
        public string Owner_Last_Name { get; set; }

        [XmlElement("Contact_Person")]
        public string Contact_Person { get; set; }

        [XmlElement("Local_To_Area")]
        public string Local_To_Area { get; set; }

        [XmlElement("Maintenance_Contractor")]
        public string Maintenance_Contractor { get; set; }

        [XmlElement("Comment")]
        public string Comment { get; set; }

        [XmlElement("WCB_Num")]
        public string WCB_Num { get; set; }

        [XmlElement("WCB_Expiry_Dt")]
        public string WCB_Expiry_Dt { get; set; }

        [XmlElement("CGL_company")]
        public string CGL_Company { get; set; }

        [XmlElement("CGL_Policy")]
        public string CGL_Policy { get; set; }

        [XmlElement("CGL_Start_Dt")]
        public string CGL_Start_Dt { get; set; }

        [XmlElement("CGL_End_Dt")]
        public string CGL_End_Dt { get; set; }

        [XmlElement("Status_Cd")]
        public string Status_Cd { get; set; }

        [XmlElement("Archive_Cd")]
        public string Archive_Cd { get; set; }

        [XmlElement("Service_Area_Id")]
        public string Service_Area_Id { get; set; }

        [XmlElement("Selected_Service_Area_Id")]
        public string Selected_Service_Area_Id { get; set; }

        [XmlElement("Created_By")]
        public string Created_By { get; set; }

        [XmlElement("Created_Dt")]
        public string Created_Dt { get; set; }

        [XmlElement("Modified_By")]
        public string Modified_By { get; set; }

        [XmlElement("Modified_Dt")]
        public string Modified_Dt { get; set; }
    }
}
