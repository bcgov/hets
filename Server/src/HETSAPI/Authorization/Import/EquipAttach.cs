using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class EquipAttach
    {
        [XmlElement("Equip_Id")]
        public int? Equip_Id { get; set; }

        [XmlElement("Attach_Seq_Num")]
        public int? Attach_Seq_Num { get; set; }

        [XmlElement("Attach_Desc")]
        public string Attach_Desc { get; set; }

        [XmlElement("Created_Dt")]
        public string Created_Dt { get; set; }

        [XmlElement("Created_By")]
        public string Created_By { get; set; }
    }
}
