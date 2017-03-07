using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SchoolBusAPI.Import
{
    public class HETS_City
    {
        [XmlAttribute]
        public string Service_Area_Id { get; set; }

        [XmlAttribute]
        public string Seq_Num { get; set; }

        [XmlAttribute]
        public string City { get; set; }               
    }
}
