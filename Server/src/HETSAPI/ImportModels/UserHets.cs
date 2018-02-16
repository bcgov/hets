using System.Xml.Serialization;

namespace HETSAPI.ImportModels
{
    /// <summary>
    /// HETS User Import Model
    /// </summary>
    [XmlRoot("UserHETS")]
    public class UserHETS
    {
        // <User_HETS><Popt_ID>2927873</Popt_ID>
        //<Service_Area_Id>1</Service_Area_Id>
        //<User_Cd>idir\srichdal  </User_Cd>
        //<Authority>N</Authority>
        //<Default_Service_Area>N</Default_Service_Area>
        //<Created_Dt>2005-02-27T09:39:31.257</Created_Dt>
        //<Created_By>HETSCONV</Created_By>
        //<Modified_Dt>2010-02-11T00:00:00</Modified_Dt>
        //<Modified_By>Harwood, Ann  (IDIR\aharwood)</Modified_By></User_HETS>


        /// <summary>
        /// Popt Id
        /// </summary>
        [XmlElement("Popt_ID")]
        public string Popt_Id { get; set; }

        /// <summary>
        /// Service Area Id
        /// </summary>
        [XmlElement("Service_Area_Id")]
        public string Service_Area_Id { get; set; }

        /// <summary>
        /// User Code
        /// </summary>
        [XmlElement("User_Cd")]
        public string User_Cd { get; set; }

        /// <summary>
        /// Authority
        /// </summary>
        [XmlElement("Authority")]
        public string Authority { get; set; }

        /// <summary>
        /// Default Service Area
        /// </summary>
        [XmlElement("Default_Service_Area")]
        public string Default_Service_Area { get; set; }

        /// <summary>
        /// Created Date
        /// </summary>
        [XmlElement("Created_Dt")]
        public string Created_Dt { get; set; }

        /// <summary>
        /// Created By
        /// </summary>
        [XmlElement("Created_By")]
        public string Created_By { get; set; }

        /// <summary>
        /// Modified Date
        /// </summary>
        [XmlElement("Modified_Dt")]
        public string Modified_Dt { get; set; }

        /// <summary>
        /// Modified By
        /// </summary>
        [XmlElement("Modified_By")]
        public string Modified_By { get; set; }
    }
}
