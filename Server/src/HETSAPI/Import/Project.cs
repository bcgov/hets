using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SchoolBusAPI.Import
{
    public class Project
    {
        [XmlAttribute]
        public string Project_Id { get; set; }

        [XmlAttribute]
        public string Service_Area_Id { get; set; }

        [XmlAttribute]
        public string Project_Num { get; set; }

        [XmlAttribute]
        public string Job_Desc1 { get; set; }

        [XmlAttribute]
        public string Job_Desc2 { get; set; }
        [XmlAttribute]
        public string Created_Dt { get; set; }
        [XmlAttribute]
        public string Created_By { get; set; }
 
    }
}
