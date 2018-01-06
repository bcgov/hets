using System.Xml.Serialization;

namespace HETSAPI.ImportModels
{
    /// <summary>
    /// Rotation Document Import Model
    /// </summary>
    public class Rotation_Doc
    {
        /// <summary>
        /// Equipoment Id
        /// </summary>
        [XmlElement("Equip_Id")]
        public int Equip_Id { get; set; }

        /// <summary>
        /// Note Date
        /// </summary>
        [XmlElement("Note_Dt")]
        public string Note_Dt { get; set; }

        /// <summary>
        /// Created Date
        /// </summary>
        [XmlElement("Created_Dt")]
        public string Created_Dt { get; set; }

        /// <summary>
        /// Service Area Id
        /// </summary>
        [XmlElement("Service_Area_Id")]
        public string Service_Area_Id { get; set; }

        /// <summary>
        /// Project Id
        /// </summary>
        [XmlElement("Project_Id")]
        public int Project_Id { get; set; }

        /// <summary>
        /// Note Type
        /// </summary>
        [XmlElement("Note_Type")]
        public string Note_Type { get; set; }

        /// <summary>
        /// Reason
        /// </summary>
        [XmlElement("Reason")]
        public string Reason { get; set; }

        /// <summary>
        /// NOte Id
        /// </summary>
        [XmlElement("Note_Id")]
        public string Note_Id { get; set; }

        /// <summary>
        /// Created By
        /// </summary>
        [XmlElement("Created_By")]
        public string Created_By { get; set; }
    }
}
