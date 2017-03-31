using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class Area
    {
        [XmlElement]
        public int Area_Id { get; set; }
     
        [XmlElement]
        public string Area_Cd { get; set; }

        [XmlElement]
        public string Area_Desc { get; set; }

        [XmlElement]
        public int Service_Area_Id { get; set; }

        [XmlElement]
        public string Created_Dt { get; set; }

        [XmlElement]
        public string Created_By { get; set; }        
    }
}
