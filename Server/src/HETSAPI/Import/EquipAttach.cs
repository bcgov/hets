using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SchoolBusAPI.Import
{
    public class EquipAttach
    {
        [XmlAttribute]
        public string Equip_Id { get; set; }

        [XmlAttribute]
        public string Attach_Seq_Num { get; set; }

        [XmlAttribute]
        public string Attach_Desc { get; set; }

        [XmlAttribute]
        public string Created_Dt { get; set; }

        [XmlAttribute]
        public string Created_By { get; set; }

    }
}
