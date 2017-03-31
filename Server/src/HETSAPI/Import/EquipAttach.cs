using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class EquipAttach
    {
        [XmlElement]
        public string Equip_Id { get; set; }

        [XmlElement]
        public string Attach_Seq_Num { get; set; }

        [XmlElement]
        public string Attach_Desc { get; set; }

        [XmlElement]
        public string Created_Dt { get; set; }

        [XmlElement]
        public string Created_By { get; set; }

    }
}
