using System.Xml.Serialization;

namespace HETSAPI.Import
{
    // [XmlRoot("ArrayOfArea"), XmlType("ArrayOfArea")]
    public class Dump_Truck 
    {
        //1
        [XmlElement("Equip_Id")]
        public int Equip_Id { get; set; }

        [XmlElement("Single_Axle")]
        public string Single_Axle { get; set; }

        [XmlElement("Tandem_Axle")]
        public string Tandem_Axle { get; set; }

        [XmlElement("PUP")]
        public string PUP { get; set; }

        [XmlElement("Belly_Dump")]
        public string Belly_Dump { get; set; }

        //6
        [XmlElement("Tridem")]
        public string Tridem { get; set; }

        [XmlElement("Rock_Box")]
        public string Rock_Box { get; set; }

        [XmlElement("Hilift_Gate")]
        public string Hilift_Gate { get; set; }

        [XmlElement("Water_Truck")]
        public string Water_Truck { get; set; }

        [XmlElement("Seal_Coat_Hitch")]
        public string Seal_Coat_Hitch { get; set; }

        //11
        [XmlElement("Rear_Axle_Spacing")]
        public string Rear_Axle_Spacing { get; set; }

        [XmlElement("Front_Tire_Size")]
        public string Front_Tire_Size { get; set; }

        [XmlElement("Front_Tire_UOM")]
        public string Front_Tire_UOM { get; set; }

        [XmlElement("Front_Axle_Capacity")]
        public string Front_Axle_Capacity { get; set; }

        [XmlElement("Rare_Axle_Capacity")]
        public string Rare_Axle_Capacity { get; set; }

        //16
        [XmlElement("Legal_Load")]
        public string Legal_Load { get; set; }

        [XmlElement("Legal_Capacity")]
        public string Legal_Capacity { get; set; }

        [XmlElement("Legal_PUP_Tare_Weight")]
        public string Legal_PUP_Tare_Weight { get; set; }

        [XmlElement("Licenced_GVW")]
        public string Licenced_GVW { get; set; }

        [XmlElement("Licenced_GVW_UOM")]
        public string Licenced_GVW_UOM { get; set; }

        //21
        [XmlElement("Licenced_Tare_Weight")]
        public string Licenced_Tare_Weight { get; set; }

        [XmlElement("Licenced_PUP_Tare_Weight")]
        public string Licenced_PUP_Tare_Weight { get; set; }

        [XmlElement("Licenced_Load")]
        public string Licenced_Load { get; set; }

        [XmlElement("Licenced_Capacity")]
        public string Licenced_Capacity { get; set; }

        [XmlElement("Box_Length")]
        public string Box_Length { get; set; }

        //26
        [XmlElement("Box_Width")]
        public string Box_Width { get; set; }

        [XmlElement("Box_Height")]
        public string Box_Height { get; set; }

        [XmlElement("Box_Capacity")]
        public string Box_Capacity { get; set; }

        [XmlElement("Trailer_Box_Length")]
        public string Trailer_Box_Length { get; set; }

        [XmlElement("Trailer_Box_Width")]
        public string Trailer_Box_Width { get; set; }

        //31
        [XmlElement("Trailer_Box_Height")]
        public string Trailer_Box_Height { get; set; }

        [XmlElement("Trailer_Box_Capacity")]
        public string Trailer_Box_Capacity { get; set; }
    }
}

