using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SchoolBusAPI.Import
{
    public class Owner
    {
        [XmlAttribute]
        public string Popt_Id { get; set; }

        [XmlAttribute]
        public string Area_Id { get; set; }

        [XmlAttribute]
        public string Owner_Cd { get; set; }

        [XmlAttribute]
        public string Owner_First_Name { get; set; }

        [XmlAttribute]
        public string Owner_Last_Name { get; set; }

        [XmlAttribute]
        public string Contact_Person { get; set; }

        [XmlAttribute]
        public string Local_To_Area { get; set; }

        [XmlAttribute]
        public string Maintenance_Contractor { get; set; }

        [XmlAttribute]
        public string Comment { get; set; }

        [XmlAttribute]
        public string WCB_Num { get; set; }

        [XmlAttribute]
        public string WCB_Expiry_Dt { get; set; }
        [XmlAttribute]
        public string CGL_company { get; set; }
        [XmlAttribute]
        public string Second_Blk { get; set; }
        [XmlAttribute]
        public string CGL_Policy { get; set; }
        [XmlAttribute]
        public string CGL_Start_Dt { get; set; }
        [XmlAttribute]
        public string CGL_End_Dt { get; set; }
        [XmlAttribute]
        public string Status_Cd { get; set; }
        [XmlAttribute]
        public string Archive_Cd { get; set; }
        [XmlAttribute]
        public string Service_Area_Id { get; set; }
        [XmlAttribute]
        public string Selected_Service_Area_Id { get; set; }
        [XmlAttribute]
        public string Created_By { get; set; }
        [XmlAttribute]
        public string Created_Dt { get; set; }
        [XmlAttribute]
        public string Modified_By { get; set; }
        [XmlAttribute]
        public string Modified_Dt { get; set; }
    }
}
