using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class User_HETS
    {
        [XmlElement]
        public string Popt_ID { get; set; }
        [XmlElement]
        public string Service_Area_Id { get; set; }
        [XmlElement]
        public string User_Cd { get; set; }
        [XmlElement]
        public string Authority { get; set; }
        [XmlElement]
        public string Default_Service_Area { get; set; }
        [XmlElement]
        public string Created_Dt { get; set; }
        [XmlElement]
        public string Created_By { get; set; }
        [XmlElement]
        public string Modified_Dt { get; set; }
        [XmlElement]
        public string Modified_By { get; set; }
    }
}
