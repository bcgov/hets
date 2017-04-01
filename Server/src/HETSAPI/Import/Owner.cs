using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class Owner
    {
        [XmlElement]
        public int Popt_Id { get; set; }

        [XmlElement]
        public int Area_Id { get; set; }

        [XmlElement]
        public string Owner_Cd { get; set; }

        [XmlElement]
        public string Owner_First_Name { get; set; }

        [XmlElement]
        public string Owner_Last_Name { get; set; }

        [XmlElement]
        public string Contact_Person { get; set; }

        [XmlElement]
        public string Local_To_Area { get; set; }

        [XmlElement]
        public string Maintenance_Contractor { get; set; }

        [XmlElement]
        public string Comment { get; set; }

        [XmlElement]
        public string WCB_Num { get; set; }

        [XmlElement]
        public string WCB_Expiry_Dt { get; set; }
        [XmlElement]
        public string CGL_company { get; set; }
        [XmlElement]
        public string Second_Blk { get; set; }
        [XmlElement]
        public string CGL_Policy { get; set; }
        [XmlElement]
        public string CGL_Start_Dt { get; set; }
        [XmlElement]
        public string CGL_End_Dt { get; set; }
        [XmlElement]
        public string Status_Cd { get; set; }
        [XmlElement]
        public string Archive_Cd { get; set; }
        [XmlElement]
        public int Service_Area_Id { get; set; }
        [XmlElement]
        public int Selected_Service_Area_Id { get; set; }
        [XmlElement]
        public string Created_By { get; set; }
        [XmlElement]
        public string Created_Dt { get; set; }
        [XmlElement]
        public string Modified_By { get; set; }
        [XmlElement]
        public string Modified_Dt { get; set; }
    }
}
