using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HETSAPI.Import
{
   // [XmlRoot("HETS_City")]
    public class HETS_City
    {
        [XmlElement("Service_Area_Id")]
        public int Service_Area_Id { get; set; }

        [XmlElement("Seq_Num")]
        public int Seq_Num { get; set; }

        [XmlElement("City")]
        public string Name { get; set; }               
    }
}
