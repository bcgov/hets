using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SchoolBusAPI.Import
{
    public class User_HETS
    {
        [XmlAttribute]
        public string Popt_ID { get; set; }
        [XmlAttribute]
        public string Service_Area_Id { get; set; }
        [XmlAttribute]
        public string User_Cd { get; set; }
        [XmlAttribute]
        public string Authority { get; set; }
        [XmlAttribute]
        public string Default_Service_Area { get; set; }
        [XmlAttribute]
        public string Created_Dt { get; set; }
        [XmlAttribute]
        public string Created_By { get; set; }
        [XmlAttribute]
        public string Modified_Dt { get; set; }
        [XmlAttribute]
        public string Modified_By { get; set; }
    }
}
