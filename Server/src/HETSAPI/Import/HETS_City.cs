using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class HETS_City
    {
        [XmlElement]
        public string Service_Area_Id { get; set; }

        [XmlElement]
        public string Seq_Num { get; set; }

        [XmlElement]
        public string City { get; set; }               
    }
}
