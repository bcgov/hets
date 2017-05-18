using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class HETS_District
    {
        [XmlElement("District_Id")]
        public int District_Id { get; set; }

        [XmlElement("Ministry_District_Id")]
        public int Ministry_District_Id { get; set; }

        [XmlElement("District_Name")]
        public string Name { get; set; }

        [XmlElement("Region_ID")]
        public int Region_ID { get; set; }

        [XmlElement("District_Number")]
        public string District_Number { get; set; }
    }
}

