using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class Project
    {
        [XmlElement]
        public string Project_Id { get; set; }

        [XmlElement]
        public string Service_Area_Id { get; set; }

        [XmlElement]
        public string Project_Num { get; set; }

        [XmlElement]
        public string Job_Desc1 { get; set; }

        [XmlElement]
        public string Job_Desc2 { get; set; }
        [XmlElement]
        public string Created_Dt { get; set; }
        [XmlElement]
        public string Created_By { get; set; }
 
    }
}
