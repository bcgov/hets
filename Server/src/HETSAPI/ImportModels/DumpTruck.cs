using System.Xml.Serialization;

namespace HETSAPI.ImportModels
{
    /// <summary>
    /// Dump Track Import Model
    /// </summary>
    public class DumpTruck 
    {
        /// <summary>
        /// Equipment Id
        /// </summary>
        [XmlElement("Equip_Id")]
        public int Equip_Id { get; set; }

        /// <summary>
        /// Single Axle
        /// </summary>
        [XmlElement("Single_Axle")]
        public string Single_Axle { get; set; }

        /// <summary>
        /// Tandem Axle
        /// </summary>
        [XmlElement("Tandem_Axle")]
        public string Tandem_Axle { get; set; }

        /// <summary>
        /// PUP
        /// </summary>
        [XmlElement("PUP")]
        public string PUP { get; set; }

        /// <summary>
        /// Belly Dump
        /// </summary>
        [XmlElement("Belly_Dump")]
        public string Belly_Dump { get; set; }

        /// <summary>
        /// Tridem
        /// </summary>
        [XmlElement("Tridem")]
        public string Tridem { get; set; }

        /// <summary>
        /// Rock Box
        /// </summary>
        [XmlElement("Rock_Box")]
        public string Rock_Box { get; set; }

        /// <summary>
        /// Hilift Gate
        /// </summary>
        [XmlElement("Hilift_Gate")]
        public string Hilift_Gate { get; set; }

        /// <summary>
        /// Water Truck
        /// </summary>
        [XmlElement("Water_Truck")]
        public string Water_Truck { get; set; }

        /// <summary>
        /// Seal Coat Hitch
        /// </summary>
        [XmlElement("Seal_Coat_Hitch")]
        public string Seal_Coat_Hitch { get; set; }

        /// <summary>
        /// Rear Axle Spacing
        /// </summary>
        [XmlElement("Rear_Axle_Spacing")]
        public string Rear_Axle_Spacing { get; set; }

        /// <summary>
        /// Front Tire Size
        /// </summary>
        [XmlElement("Front_Tire_Size")]
        public string Front_Tire_Size { get; set; }

        /// <summary>
        /// Front Tire UOM
        /// </summary>
        [XmlElement("Front_Tire_UOM")]
        public string Front_Tire_UOM { get; set; }

        /// <summary>
        /// Front Axle Capacity
        /// </summary>
        [XmlElement("Front_Axle_Capacity")]
        public string Front_Axle_Capacity { get; set; }

        /// <summary>
        /// Rear Axle Capacity
        /// </summary>
        [XmlElement("Rear_Axle_Capacity")]
        public string Rear_Axle_Capacity { get; set; }

        /// <summary>
        /// Legal Load
        /// </summary>
        [XmlElement("Legal_Load")]
        public string Legal_Load { get; set; }

        /// <summary>
        /// Legal Capcity
        /// </summary>
        [XmlElement("Legal_Capacity")]
        public string Legal_Capacity { get; set; }

        /// <summary>
        /// Legal PUP Tare Weight
        /// </summary>
        [XmlElement("Legal_PUP_Tare_Weight")]
        public string Legal_PUP_Tare_Weight { get; set; }

        /// <summary>
        /// Licensed GVW
        /// </summary>
        [XmlElement("Licenced_GVW")]
        public string Licenced_GVW { get; set; }

        /// <summary>
        /// Licensed GVW UOM
        /// </summary>
        [XmlElement("Licenced_GVW_UOM")]
        public string Licenced_GVW_UOM { get; set; }

        /// <summary>
        /// Licensed Tare Weight
        /// </summary>
        [XmlElement("Licenced_Tare_Weight")]
        public string Licenced_Tare_Weight { get; set; }

        /// <summary>
        /// Licensed PUP Tare Weight
        /// </summary>
        [XmlElement("Licenced_PUP_Tare_Weight")]
        public string Licenced_PUP_Tare_Weight { get; set; }

        /// <summary>
        /// Licensed Load
        /// </summary>
        [XmlElement("Licenced_Load")]
        public string Licenced_Load { get; set; }

        /// <summary>
        /// Licensed Capacity
        /// </summary>
        [XmlElement("Licenced_Capacity")]
        public string Licenced_Capacity { get; set; }

        /// <summary>
        /// Box Length
        /// </summary>
        [XmlElement("Box_Length")]
        public string Box_Length { get; set; }

        /// <summary>
        /// Box Width
        /// </summary>
        [XmlElement("Box_Width")]
        public string Box_Width { get; set; }

        /// <summary>
        /// Box Height
        /// </summary>
        [XmlElement("Box_Height")]
        public string Box_Height { get; set; }

        /// <summary>
        /// Box Capacity
        /// </summary>
        [XmlElement("Box_Capacity")]
        public string Box_Capacity { get; set; }

        /// <summary>
        /// Trailer Box Length
        /// </summary>
        [XmlElement("Trailer_Box_Length")]
        public string Trailer_Box_Length { get; set; }

        /// <summary>
        /// Trailer Box Width
        /// </summary>
        [XmlElement("Trailer_Box_Width")]
        public string Trailer_Box_Width { get; set; }

        /// <summary>
        /// Trailer Box Height
        /// </summary>
        [XmlElement("Trailer_Box_Height")]
        public string Trailer_Box_Height { get; set; }

        /// <summary>
        /// Trailer Box Capacity
        /// </summary>
        [XmlElement("Trailer_Box_Capacity")]
        public string Trailer_Box_Capacity { get; set; }
    }
}

