using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SchoolBusAPI.Import
{
    public class HETS_AUDIT
    {
        [XmlAttribute]
        public string Created_By { get; set; }

        [XmlAttribute]
        public string Created_Dt { get; set; }

        [XmlAttribute]
        public string Action { get; set; }

        [XmlAttribute]
        public string Reason { get; set; }
        
    }
}
