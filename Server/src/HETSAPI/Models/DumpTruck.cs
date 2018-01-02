using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// Additional attributes about a piece of equipment designated as a Dump Truck. Historically, there was a perceived need to track a lot of informatiion about a dump truck, but in practice, few fields are being completed by users. Places for the attributes are being maintained, but the UI is prompting only for a couple of the fields, and only those fields are likely to be populated. Additional basic information about the attributes can be found on Wikipedia, BC-specific details on MOTI&amp;#39;s CVSE website and www.bclaws.ca&amp;#x2F;EPLibraries&amp;#x2F;bclaws_new&amp;#x2F;document&amp;#x2F;ID&amp;#x2F;freeside&amp;#x2F;30_78#AppendicesAtoK, Appendix A and  B. Any metrics information provided here is from the Equipment Owner.
    /// </summary>
        [MetaDataExtension (Description = "Additional attributes about a piece of equipment designated as a Dump Truck. Historically, there was a perceived need to track a lot of informatiion about a dump truck, but in practice, few fields are being completed by users. Places for the attributes are being maintained, but the UI is prompting only for a couple of the fields, and only those fields are likely to be populated. Additional basic information about the attributes can be found on Wikipedia, BC-specific details on MOTI&amp;#39;s CVSE website and www.bclaws.ca&amp;#x2F;EPLibraries&amp;#x2F;bclaws_new&amp;#x2F;document&amp;#x2F;ID&amp;#x2F;freeside&amp;#x2F;30_78#AppendicesAtoK, Appendix A and  B. Any metrics information provided here is from the Equipment Owner.")]

    public partial class DumpTruck : AuditableEntity, IEquatable<DumpTruck>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public DumpTruck()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DumpTruck" /> class.
        /// </summary>
        /// <param name="Id">A system-generated unique identifier for a DumpTruck (required).</param>
        /// <param name="IsSingleAxle">True if the vehicle has a single axle. Can be false or null..</param>
        /// <param name="IsTandemAxle">True if the vehicle has a tandem axle. Can be false or null..</param>
        /// <param name="IsTridem">True if the Dump Truck is a tridem - a three axle dump truck. Can be false or null..</param>
        /// <param name="HasPUP">True if the Dump Truck has a PUP trailer - a trailer with it&amp;#39;s own hydraulic unloading system. Can be false or null..</param>
        /// <param name="HasBellyDump">True if the Dump Truck has a belly dump capability. Can be false or null..</param>
        /// <param name="HasRockBox">True if the Dump Truck has a rock box. Can be false or null..</param>
        /// <param name="HasHiliftGate">True if the Dump Truck has a high lift gate vs. a traditional gate. Can be false or null..</param>
        /// <param name="IsWaterTruck">True if the Dump Truck is a Water Truck. Can be false or null..</param>
        /// <param name="HasSealcoatHitch">True if the Dump Truck has a hitch for using sealcoat trailers. Can be false or null..</param>
        /// <param name="RearAxleSpacing">The spacing of the rear axles, if applicable. Usually in metres..</param>
        /// <param name="FrontTireSize">The size of of the Front Tires of the Dump Truck..</param>
        /// <param name="FrontTireUOM">The Unit of Measure of the Front Tire Size..</param>
        /// <param name="FrontAxleCapacity">The rated capacity of the Front Axle..</param>
        /// <param name="RearAxleCapacity">The rated capacity of the Rear Axle..</param>
        /// <param name="LegalLoad">The legal load of the vehicle in kg..</param>
        /// <param name="LegalCapacity">The legal capacity of the dump truck..</param>
        /// <param name="LegalPUPTareWeight">The legal Tare Weight (weight when unloaded) of the PUP trailer..</param>
        /// <param name="LicencedGVW">The Gross Vehicle Weight for which the vehicle is licensed. GVW includes the vehicle&amp;#39;s chassis, body, engine, engine fluids, fuel, accessories, driver, passengers and cargo but excluding that of any trailers..</param>
        /// <param name="LicencedGVWUOM">The Unit of Measure (UOM) of the licenced GVW..</param>
        /// <param name="LicencedTareWeight">The licenced Tare Weight (weight when unloaded) of the vehicle..</param>
        /// <param name="LicencedPUPTareWeight">The licenced Tare Weight (weight when unloaded) of the PUP trailer..</param>
        /// <param name="LicencedLoad">The licenced maximum load of the vehicle in kg..</param>
        /// <param name="LicencedCapacity">The licenced maximum capacity of the vehicle..</param>
        /// <param name="BoxLength">The length of the box, in metres. See - http-&amp;#x2F;&amp;#x2F;www.bclaws.ca&amp;#x2F;EPLibraries&amp;#x2F;bclaws_new&amp;#x2F;document&amp;#x2F;ID&amp;#x2F;freeside&amp;#x2F;30_7, appendix B.</param>
        /// <param name="BoxWidth">The width of the box, in metres. See - http-&amp;#x2F;&amp;#x2F;www.bclaws.ca&amp;#x2F;EPLibraries&amp;#x2F;bclaws_new&amp;#x2F;document&amp;#x2F;ID&amp;#x2F;freeside&amp;#x2F;30_7, appendix B.</param>
        /// <param name="BoxHeight">The height of the box, in metres. See- http-&amp;#x2F;&amp;#x2F;www.bclaws.ca&amp;#x2F;EPLibraries&amp;#x2F;bclaws_new&amp;#x2F;document&amp;#x2F;ID&amp;#x2F;freeside&amp;#x2F;30_7, appendix B.</param>
        /// <param name="BoxCapacity">The capacity of the box..</param>
        /// <param name="TrailerBoxLength">The length of the trailer box, in metres. See www.bclaws.ca&amp;#x2F;EPLibraries&amp;#x2F;bclaws_new&amp;#x2F;document&amp;#x2F;ID&amp;#x2F;freeside&amp;#x2F;30_7, appendix B.</param>
        /// <param name="TrailerBoxWidth">The width of the trailer box, in metres. See www.bclaws.ca&amp;#x2F;EPLibraries&amp;#x2F;bclaws_new&amp;#x2F;document&amp;#x2F;ID&amp;#x2F;freeside&amp;#x2F;30_7, appendix B.</param>
        /// <param name="TrailerBoxHeight">The height of the trailer box, in metres. See www.bclaws.ca&amp;#x2F;EPLibraries&amp;#x2F;bclaws_new&amp;#x2F;document&amp;#x2F;ID&amp;#x2F;freeside&amp;#x2F;30_7, appendix B.</param>
        /// <param name="TrailerBoxCapacity">The capacity of the trailer box..</param>
        public DumpTruck(int Id, bool? IsSingleAxle = null, bool? IsTandemAxle = null, bool? IsTridem = null, bool? HasPUP = null, bool? HasBellyDump = null, bool? HasRockBox = null, bool? HasHiliftGate = null, bool? IsWaterTruck = null, bool? HasSealcoatHitch = null, string RearAxleSpacing = null, string FrontTireSize = null, string FrontTireUOM = null, string FrontAxleCapacity = null, string RearAxleCapacity = null, string LegalLoad = null, string LegalCapacity = null, string LegalPUPTareWeight = null, string LicencedGVW = null, string LicencedGVWUOM = null, string LicencedTareWeight = null, string LicencedPUPTareWeight = null, string LicencedLoad = null, string LicencedCapacity = null, string BoxLength = null, string BoxWidth = null, string BoxHeight = null, string BoxCapacity = null, string TrailerBoxLength = null, string TrailerBoxWidth = null, string TrailerBoxHeight = null, string TrailerBoxCapacity = null)
        {   
            this.Id = Id;
            this.IsSingleAxle = IsSingleAxle;
            this.IsTandemAxle = IsTandemAxle;
            this.IsTridem = IsTridem;
            this.HasPUP = HasPUP;
            this.HasBellyDump = HasBellyDump;
            this.HasRockBox = HasRockBox;
            this.HasHiliftGate = HasHiliftGate;
            this.IsWaterTruck = IsWaterTruck;
            this.HasSealcoatHitch = HasSealcoatHitch;
            this.RearAxleSpacing = RearAxleSpacing;
            this.FrontTireSize = FrontTireSize;
            this.FrontTireUOM = FrontTireUOM;
            this.FrontAxleCapacity = FrontAxleCapacity;
            this.RearAxleCapacity = RearAxleCapacity;
            this.LegalLoad = LegalLoad;
            this.LegalCapacity = LegalCapacity;
            this.LegalPUPTareWeight = LegalPUPTareWeight;
            this.LicencedGVW = LicencedGVW;
            this.LicencedGVWUOM = LicencedGVWUOM;
            this.LicencedTareWeight = LicencedTareWeight;
            this.LicencedPUPTareWeight = LicencedPUPTareWeight;
            this.LicencedLoad = LicencedLoad;
            this.LicencedCapacity = LicencedCapacity;
            this.BoxLength = BoxLength;
            this.BoxWidth = BoxWidth;
            this.BoxHeight = BoxHeight;
            this.BoxCapacity = BoxCapacity;
            this.TrailerBoxLength = TrailerBoxLength;
            this.TrailerBoxWidth = TrailerBoxWidth;
            this.TrailerBoxHeight = TrailerBoxHeight;
            this.TrailerBoxCapacity = TrailerBoxCapacity;
        }

        /// <summary>
        /// A system-generated unique identifier for a DumpTruck
        /// </summary>
        /// <value>A system-generated unique identifier for a DumpTruck</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a DumpTruck")]
        public int Id { get; set; }
        
        /// <summary>
        /// True if the vehicle has a single axle. Can be false or null.
        /// </summary>
        /// <value>True if the vehicle has a single axle. Can be false or null.</value>
        [MetaDataExtension (Description = "True if the vehicle has a single axle. Can be false or null.")]
        public bool? IsSingleAxle { get; set; }
        
        /// <summary>
        /// True if the vehicle has a tandem axle. Can be false or null.
        /// </summary>
        /// <value>True if the vehicle has a tandem axle. Can be false or null.</value>
        [MetaDataExtension (Description = "True if the vehicle has a tandem axle. Can be false or null.")]
        public bool? IsTandemAxle { get; set; }
        
        /// <summary>
        /// True if the Dump Truck is a tridem - a three axle dump truck. Can be false or null.
        /// </summary>
        /// <value>True if the Dump Truck is a tridem - a three axle dump truck. Can be false or null.</value>
        [MetaDataExtension (Description = "True if the Dump Truck is a tridem - a three axle dump truck. Can be false or null.")]
        public bool? IsTridem { get; set; }
        
        /// <summary>
        /// True if the Dump Truck has a PUP trailer - a trailer with it&#39;s own hydraulic unloading system. Can be false or null.
        /// </summary>
        /// <value>True if the Dump Truck has a PUP trailer - a trailer with it&#39;s own hydraulic unloading system. Can be false or null.</value>
        [MetaDataExtension (Description = "True if the Dump Truck has a PUP trailer - a trailer with it&#39;s own hydraulic unloading system. Can be false or null.")]
        public bool? HasPUP { get; set; }
        
        /// <summary>
        /// True if the Dump Truck has a belly dump capability. Can be false or null.
        /// </summary>
        /// <value>True if the Dump Truck has a belly dump capability. Can be false or null.</value>
        [MetaDataExtension (Description = "True if the Dump Truck has a belly dump capability. Can be false or null.")]
        public bool? HasBellyDump { get; set; }
        
        /// <summary>
        /// True if the Dump Truck has a rock box. Can be false or null.
        /// </summary>
        /// <value>True if the Dump Truck has a rock box. Can be false or null.</value>
        [MetaDataExtension (Description = "True if the Dump Truck has a rock box. Can be false or null.")]
        public bool? HasRockBox { get; set; }
        
        /// <summary>
        /// True if the Dump Truck has a high lift gate vs. a traditional gate. Can be false or null.
        /// </summary>
        /// <value>True if the Dump Truck has a high lift gate vs. a traditional gate. Can be false or null.</value>
        [MetaDataExtension (Description = "True if the Dump Truck has a high lift gate vs. a traditional gate. Can be false or null.")]
        public bool? HasHiliftGate { get; set; }
        
        /// <summary>
        /// True if the Dump Truck is a Water Truck. Can be false or null.
        /// </summary>
        /// <value>True if the Dump Truck is a Water Truck. Can be false or null.</value>
        [MetaDataExtension (Description = "True if the Dump Truck is a Water Truck. Can be false or null.")]
        public bool? IsWaterTruck { get; set; }
        
        /// <summary>
        /// True if the Dump Truck has a hitch for using sealcoat trailers. Can be false or null.
        /// </summary>
        /// <value>True if the Dump Truck has a hitch for using sealcoat trailers. Can be false or null.</value>
        [MetaDataExtension (Description = "True if the Dump Truck has a hitch for using sealcoat trailers. Can be false or null.")]
        public bool? HasSealcoatHitch { get; set; }
        
        /// <summary>
        /// The spacing of the rear axles, if applicable. Usually in metres.
        /// </summary>
        /// <value>The spacing of the rear axles, if applicable. Usually in metres.</value>
        [MetaDataExtension (Description = "The spacing of the rear axles, if applicable. Usually in metres.")]
        [MaxLength(150)]
        
        public string RearAxleSpacing { get; set; }
        
        /// <summary>
        /// The size of of the Front Tires of the Dump Truck.
        /// </summary>
        /// <value>The size of of the Front Tires of the Dump Truck.</value>
        [MetaDataExtension (Description = "The size of of the Front Tires of the Dump Truck.")]
        [MaxLength(150)]
        
        public string FrontTireSize { get; set; }
        
        /// <summary>
        /// The Unit of Measure of the Front Tire Size.
        /// </summary>
        /// <value>The Unit of Measure of the Front Tire Size.</value>
        [MetaDataExtension (Description = "The Unit of Measure of the Front Tire Size.")]
        [MaxLength(150)]
        
        public string FrontTireUOM { get; set; }
        
        /// <summary>
        /// The rated capacity of the Front Axle.
        /// </summary>
        /// <value>The rated capacity of the Front Axle.</value>
        [MetaDataExtension (Description = "The rated capacity of the Front Axle.")]
        [MaxLength(150)]
        
        public string FrontAxleCapacity { get; set; }
        
        /// <summary>
        /// The rated capacity of the Rear Axle.
        /// </summary>
        /// <value>The rated capacity of the Rear Axle.</value>
        [MetaDataExtension (Description = "The rated capacity of the Rear Axle.")]
        [MaxLength(150)]
        
        public string RearAxleCapacity { get; set; }
        
        /// <summary>
        /// The legal load of the vehicle in kg.
        /// </summary>
        /// <value>The legal load of the vehicle in kg.</value>
        [MetaDataExtension (Description = "The legal load of the vehicle in kg.")]
        [MaxLength(150)]
        
        public string LegalLoad { get; set; }
        
        /// <summary>
        /// The legal capacity of the dump truck.
        /// </summary>
        /// <value>The legal capacity of the dump truck.</value>
        [MetaDataExtension (Description = "The legal capacity of the dump truck.")]
        [MaxLength(150)]
        
        public string LegalCapacity { get; set; }
        
        /// <summary>
        /// The legal Tare Weight (weight when unloaded) of the PUP trailer.
        /// </summary>
        /// <value>The legal Tare Weight (weight when unloaded) of the PUP trailer.</value>
        [MetaDataExtension (Description = "The legal Tare Weight (weight when unloaded) of the PUP trailer.")]
        [MaxLength(150)]
        
        public string LegalPUPTareWeight { get; set; }
        
        /// <summary>
        /// The Gross Vehicle Weight for which the vehicle is licensed. GVW includes the vehicle&#39;s chassis, body, engine, engine fluids, fuel, accessories, driver, passengers and cargo but excluding that of any trailers.
        /// </summary>
        /// <value>The Gross Vehicle Weight for which the vehicle is licensed. GVW includes the vehicle&#39;s chassis, body, engine, engine fluids, fuel, accessories, driver, passengers and cargo but excluding that of any trailers.</value>
        [MetaDataExtension (Description = "The Gross Vehicle Weight for which the vehicle is licensed. GVW includes the vehicle&#39;s chassis, body, engine, engine fluids, fuel, accessories, driver, passengers and cargo but excluding that of any trailers.")]
        [MaxLength(150)]
        
        public string LicencedGVW { get; set; }
        
        /// <summary>
        /// The Unit of Measure (UOM) of the licenced GVW.
        /// </summary>
        /// <value>The Unit of Measure (UOM) of the licenced GVW.</value>
        [MetaDataExtension (Description = "The Unit of Measure (UOM) of the licenced GVW.")]
        [MaxLength(150)]
        
        public string LicencedGVWUOM { get; set; }
        
        /// <summary>
        /// The licenced Tare Weight (weight when unloaded) of the vehicle.
        /// </summary>
        /// <value>The licenced Tare Weight (weight when unloaded) of the vehicle.</value>
        [MetaDataExtension (Description = "The licenced Tare Weight (weight when unloaded) of the vehicle.")]
        [MaxLength(150)]
        
        public string LicencedTareWeight { get; set; }
        
        /// <summary>
        /// The licenced Tare Weight (weight when unloaded) of the PUP trailer.
        /// </summary>
        /// <value>The licenced Tare Weight (weight when unloaded) of the PUP trailer.</value>
        [MetaDataExtension (Description = "The licenced Tare Weight (weight when unloaded) of the PUP trailer.")]
        [MaxLength(150)]
        
        public string LicencedPUPTareWeight { get; set; }
        
        /// <summary>
        /// The licenced maximum load of the vehicle in kg.
        /// </summary>
        /// <value>The licenced maximum load of the vehicle in kg.</value>
        [MetaDataExtension (Description = "The licenced maximum load of the vehicle in kg.")]
        [MaxLength(150)]
        
        public string LicencedLoad { get; set; }
        
        /// <summary>
        /// The licenced maximum capacity of the vehicle.
        /// </summary>
        /// <value>The licenced maximum capacity of the vehicle.</value>
        [MetaDataExtension (Description = "The licenced maximum capacity of the vehicle.")]
        [MaxLength(150)]
        
        public string LicencedCapacity { get; set; }
        
        /// <summary>
        /// The length of the box, in metres. See - http-&#x2F;&#x2F;www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B
        /// </summary>
        /// <value>The length of the box, in metres. See - http-&#x2F;&#x2F;www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B</value>
        [MetaDataExtension (Description = "The length of the box, in metres. See - http-&#x2F;&#x2F;www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B")]
        [MaxLength(150)]
        
        public string BoxLength { get; set; }
        
        /// <summary>
        /// The width of the box, in metres. See - http-&#x2F;&#x2F;www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B
        /// </summary>
        /// <value>The width of the box, in metres. See - http-&#x2F;&#x2F;www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B</value>
        [MetaDataExtension (Description = "The width of the box, in metres. See - http-&#x2F;&#x2F;www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B")]
        [MaxLength(150)]
        
        public string BoxWidth { get; set; }
        
        /// <summary>
        /// The height of the box, in metres. See- http-&#x2F;&#x2F;www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B
        /// </summary>
        /// <value>The height of the box, in metres. See- http-&#x2F;&#x2F;www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B</value>
        [MetaDataExtension (Description = "The height of the box, in metres. See- http-&#x2F;&#x2F;www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B")]
        [MaxLength(150)]
        
        public string BoxHeight { get; set; }
        
        /// <summary>
        /// The capacity of the box.
        /// </summary>
        /// <value>The capacity of the box.</value>
        [MetaDataExtension (Description = "The capacity of the box.")]
        [MaxLength(150)]
        
        public string BoxCapacity { get; set; }
        
        /// <summary>
        /// The length of the trailer box, in metres. See www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B
        /// </summary>
        /// <value>The length of the trailer box, in metres. See www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B</value>
        [MetaDataExtension (Description = "The length of the trailer box, in metres. See www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B")]
        [MaxLength(150)]
        
        public string TrailerBoxLength { get; set; }
        
        /// <summary>
        /// The width of the trailer box, in metres. See www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B
        /// </summary>
        /// <value>The width of the trailer box, in metres. See www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B</value>
        [MetaDataExtension (Description = "The width of the trailer box, in metres. See www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B")]
        [MaxLength(150)]
        
        public string TrailerBoxWidth { get; set; }
        
        /// <summary>
        /// The height of the trailer box, in metres. See www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B
        /// </summary>
        /// <value>The height of the trailer box, in metres. See www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B</value>
        [MetaDataExtension (Description = "The height of the trailer box, in metres. See www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B")]
        [MaxLength(150)]
        
        public string TrailerBoxHeight { get; set; }
        
        /// <summary>
        /// The capacity of the trailer box.
        /// </summary>
        /// <value>The capacity of the trailer box.</value>
        [MetaDataExtension (Description = "The capacity of the trailer box.")]
        [MaxLength(150)]
        
        public string TrailerBoxCapacity { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class DumpTruck {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  IsSingleAxle: ").Append(IsSingleAxle).Append("\n");
            sb.Append("  IsTandemAxle: ").Append(IsTandemAxle).Append("\n");
            sb.Append("  IsTridem: ").Append(IsTridem).Append("\n");
            sb.Append("  HasPUP: ").Append(HasPUP).Append("\n");
            sb.Append("  HasBellyDump: ").Append(HasBellyDump).Append("\n");
            sb.Append("  HasRockBox: ").Append(HasRockBox).Append("\n");
            sb.Append("  HasHiliftGate: ").Append(HasHiliftGate).Append("\n");
            sb.Append("  IsWaterTruck: ").Append(IsWaterTruck).Append("\n");
            sb.Append("  HasSealcoatHitch: ").Append(HasSealcoatHitch).Append("\n");
            sb.Append("  RearAxleSpacing: ").Append(RearAxleSpacing).Append("\n");
            sb.Append("  FrontTireSize: ").Append(FrontTireSize).Append("\n");
            sb.Append("  FrontTireUOM: ").Append(FrontTireUOM).Append("\n");
            sb.Append("  FrontAxleCapacity: ").Append(FrontAxleCapacity).Append("\n");
            sb.Append("  RearAxleCapacity: ").Append(RearAxleCapacity).Append("\n");
            sb.Append("  LegalLoad: ").Append(LegalLoad).Append("\n");
            sb.Append("  LegalCapacity: ").Append(LegalCapacity).Append("\n");
            sb.Append("  LegalPUPTareWeight: ").Append(LegalPUPTareWeight).Append("\n");
            sb.Append("  LicencedGVW: ").Append(LicencedGVW).Append("\n");
            sb.Append("  LicencedGVWUOM: ").Append(LicencedGVWUOM).Append("\n");
            sb.Append("  LicencedTareWeight: ").Append(LicencedTareWeight).Append("\n");
            sb.Append("  LicencedPUPTareWeight: ").Append(LicencedPUPTareWeight).Append("\n");
            sb.Append("  LicencedLoad: ").Append(LicencedLoad).Append("\n");
            sb.Append("  LicencedCapacity: ").Append(LicencedCapacity).Append("\n");
            sb.Append("  BoxLength: ").Append(BoxLength).Append("\n");
            sb.Append("  BoxWidth: ").Append(BoxWidth).Append("\n");
            sb.Append("  BoxHeight: ").Append(BoxHeight).Append("\n");
            sb.Append("  BoxCapacity: ").Append(BoxCapacity).Append("\n");
            sb.Append("  TrailerBoxLength: ").Append(TrailerBoxLength).Append("\n");
            sb.Append("  TrailerBoxWidth: ").Append(TrailerBoxWidth).Append("\n");
            sb.Append("  TrailerBoxHeight: ").Append(TrailerBoxHeight).Append("\n");
            sb.Append("  TrailerBoxCapacity: ").Append(TrailerBoxCapacity).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((DumpTruck)obj);
        }

        /// <summary>
        /// Returns true if DumpTruck instances are equal
        /// </summary>
        /// <param name="other">Instance of DumpTruck to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(DumpTruck other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.IsSingleAxle == other.IsSingleAxle ||
                    this.IsSingleAxle != null &&
                    this.IsSingleAxle.Equals(other.IsSingleAxle)
                ) &&                 
                (
                    this.IsTandemAxle == other.IsTandemAxle ||
                    this.IsTandemAxle != null &&
                    this.IsTandemAxle.Equals(other.IsTandemAxle)
                ) &&                 
                (
                    this.IsTridem == other.IsTridem ||
                    this.IsTridem != null &&
                    this.IsTridem.Equals(other.IsTridem)
                ) &&                 
                (
                    this.HasPUP == other.HasPUP ||
                    this.HasPUP != null &&
                    this.HasPUP.Equals(other.HasPUP)
                ) &&                 
                (
                    this.HasBellyDump == other.HasBellyDump ||
                    this.HasBellyDump != null &&
                    this.HasBellyDump.Equals(other.HasBellyDump)
                ) &&                 
                (
                    this.HasRockBox == other.HasRockBox ||
                    this.HasRockBox != null &&
                    this.HasRockBox.Equals(other.HasRockBox)
                ) &&                 
                (
                    this.HasHiliftGate == other.HasHiliftGate ||
                    this.HasHiliftGate != null &&
                    this.HasHiliftGate.Equals(other.HasHiliftGate)
                ) &&                 
                (
                    this.IsWaterTruck == other.IsWaterTruck ||
                    this.IsWaterTruck != null &&
                    this.IsWaterTruck.Equals(other.IsWaterTruck)
                ) &&                 
                (
                    this.HasSealcoatHitch == other.HasSealcoatHitch ||
                    this.HasSealcoatHitch != null &&
                    this.HasSealcoatHitch.Equals(other.HasSealcoatHitch)
                ) &&                 
                (
                    this.RearAxleSpacing == other.RearAxleSpacing ||
                    this.RearAxleSpacing != null &&
                    this.RearAxleSpacing.Equals(other.RearAxleSpacing)
                ) &&                 
                (
                    this.FrontTireSize == other.FrontTireSize ||
                    this.FrontTireSize != null &&
                    this.FrontTireSize.Equals(other.FrontTireSize)
                ) &&                 
                (
                    this.FrontTireUOM == other.FrontTireUOM ||
                    this.FrontTireUOM != null &&
                    this.FrontTireUOM.Equals(other.FrontTireUOM)
                ) &&                 
                (
                    this.FrontAxleCapacity == other.FrontAxleCapacity ||
                    this.FrontAxleCapacity != null &&
                    this.FrontAxleCapacity.Equals(other.FrontAxleCapacity)
                ) &&                 
                (
                    this.RearAxleCapacity == other.RearAxleCapacity ||
                    this.RearAxleCapacity != null &&
                    this.RearAxleCapacity.Equals(other.RearAxleCapacity)
                ) &&                 
                (
                    this.LegalLoad == other.LegalLoad ||
                    this.LegalLoad != null &&
                    this.LegalLoad.Equals(other.LegalLoad)
                ) &&                 
                (
                    this.LegalCapacity == other.LegalCapacity ||
                    this.LegalCapacity != null &&
                    this.LegalCapacity.Equals(other.LegalCapacity)
                ) &&                 
                (
                    this.LegalPUPTareWeight == other.LegalPUPTareWeight ||
                    this.LegalPUPTareWeight != null &&
                    this.LegalPUPTareWeight.Equals(other.LegalPUPTareWeight)
                ) &&                 
                (
                    this.LicencedGVW == other.LicencedGVW ||
                    this.LicencedGVW != null &&
                    this.LicencedGVW.Equals(other.LicencedGVW)
                ) &&                 
                (
                    this.LicencedGVWUOM == other.LicencedGVWUOM ||
                    this.LicencedGVWUOM != null &&
                    this.LicencedGVWUOM.Equals(other.LicencedGVWUOM)
                ) &&                 
                (
                    this.LicencedTareWeight == other.LicencedTareWeight ||
                    this.LicencedTareWeight != null &&
                    this.LicencedTareWeight.Equals(other.LicencedTareWeight)
                ) &&                 
                (
                    this.LicencedPUPTareWeight == other.LicencedPUPTareWeight ||
                    this.LicencedPUPTareWeight != null &&
                    this.LicencedPUPTareWeight.Equals(other.LicencedPUPTareWeight)
                ) &&                 
                (
                    this.LicencedLoad == other.LicencedLoad ||
                    this.LicencedLoad != null &&
                    this.LicencedLoad.Equals(other.LicencedLoad)
                ) &&                 
                (
                    this.LicencedCapacity == other.LicencedCapacity ||
                    this.LicencedCapacity != null &&
                    this.LicencedCapacity.Equals(other.LicencedCapacity)
                ) &&                 
                (
                    this.BoxLength == other.BoxLength ||
                    this.BoxLength != null &&
                    this.BoxLength.Equals(other.BoxLength)
                ) &&                 
                (
                    this.BoxWidth == other.BoxWidth ||
                    this.BoxWidth != null &&
                    this.BoxWidth.Equals(other.BoxWidth)
                ) &&                 
                (
                    this.BoxHeight == other.BoxHeight ||
                    this.BoxHeight != null &&
                    this.BoxHeight.Equals(other.BoxHeight)
                ) &&                 
                (
                    this.BoxCapacity == other.BoxCapacity ||
                    this.BoxCapacity != null &&
                    this.BoxCapacity.Equals(other.BoxCapacity)
                ) &&                 
                (
                    this.TrailerBoxLength == other.TrailerBoxLength ||
                    this.TrailerBoxLength != null &&
                    this.TrailerBoxLength.Equals(other.TrailerBoxLength)
                ) &&                 
                (
                    this.TrailerBoxWidth == other.TrailerBoxWidth ||
                    this.TrailerBoxWidth != null &&
                    this.TrailerBoxWidth.Equals(other.TrailerBoxWidth)
                ) &&                 
                (
                    this.TrailerBoxHeight == other.TrailerBoxHeight ||
                    this.TrailerBoxHeight != null &&
                    this.TrailerBoxHeight.Equals(other.TrailerBoxHeight)
                ) &&                 
                (
                    this.TrailerBoxCapacity == other.TrailerBoxCapacity ||
                    this.TrailerBoxCapacity != null &&
                    this.TrailerBoxCapacity.Equals(other.TrailerBoxCapacity)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;
                // Suitable nullity checks
                                   
                hash = hash * 59 + this.Id.GetHashCode();                if (this.IsSingleAxle != null)
                {
                    hash = hash * 59 + this.IsSingleAxle.GetHashCode();
                }                
                                if (this.IsTandemAxle != null)
                {
                    hash = hash * 59 + this.IsTandemAxle.GetHashCode();
                }                
                                if (this.IsTridem != null)
                {
                    hash = hash * 59 + this.IsTridem.GetHashCode();
                }                
                                if (this.HasPUP != null)
                {
                    hash = hash * 59 + this.HasPUP.GetHashCode();
                }                
                                if (this.HasBellyDump != null)
                {
                    hash = hash * 59 + this.HasBellyDump.GetHashCode();
                }                
                                if (this.HasRockBox != null)
                {
                    hash = hash * 59 + this.HasRockBox.GetHashCode();
                }                
                                if (this.HasHiliftGate != null)
                {
                    hash = hash * 59 + this.HasHiliftGate.GetHashCode();
                }                
                                if (this.IsWaterTruck != null)
                {
                    hash = hash * 59 + this.IsWaterTruck.GetHashCode();
                }                
                                if (this.HasSealcoatHitch != null)
                {
                    hash = hash * 59 + this.HasSealcoatHitch.GetHashCode();
                }                
                                if (this.RearAxleSpacing != null)
                {
                    hash = hash * 59 + this.RearAxleSpacing.GetHashCode();
                }                
                                if (this.FrontTireSize != null)
                {
                    hash = hash * 59 + this.FrontTireSize.GetHashCode();
                }                
                                if (this.FrontTireUOM != null)
                {
                    hash = hash * 59 + this.FrontTireUOM.GetHashCode();
                }                
                                if (this.FrontAxleCapacity != null)
                {
                    hash = hash * 59 + this.FrontAxleCapacity.GetHashCode();
                }                
                                if (this.RearAxleCapacity != null)
                {
                    hash = hash * 59 + this.RearAxleCapacity.GetHashCode();
                }                
                                if (this.LegalLoad != null)
                {
                    hash = hash * 59 + this.LegalLoad.GetHashCode();
                }                
                                if (this.LegalCapacity != null)
                {
                    hash = hash * 59 + this.LegalCapacity.GetHashCode();
                }                
                                if (this.LegalPUPTareWeight != null)
                {
                    hash = hash * 59 + this.LegalPUPTareWeight.GetHashCode();
                }                
                                if (this.LicencedGVW != null)
                {
                    hash = hash * 59 + this.LicencedGVW.GetHashCode();
                }                
                                if (this.LicencedGVWUOM != null)
                {
                    hash = hash * 59 + this.LicencedGVWUOM.GetHashCode();
                }                
                                if (this.LicencedTareWeight != null)
                {
                    hash = hash * 59 + this.LicencedTareWeight.GetHashCode();
                }                
                                if (this.LicencedPUPTareWeight != null)
                {
                    hash = hash * 59 + this.LicencedPUPTareWeight.GetHashCode();
                }                
                                if (this.LicencedLoad != null)
                {
                    hash = hash * 59 + this.LicencedLoad.GetHashCode();
                }                
                                if (this.LicencedCapacity != null)
                {
                    hash = hash * 59 + this.LicencedCapacity.GetHashCode();
                }                
                                if (this.BoxLength != null)
                {
                    hash = hash * 59 + this.BoxLength.GetHashCode();
                }                
                                if (this.BoxWidth != null)
                {
                    hash = hash * 59 + this.BoxWidth.GetHashCode();
                }                
                                if (this.BoxHeight != null)
                {
                    hash = hash * 59 + this.BoxHeight.GetHashCode();
                }                
                                if (this.BoxCapacity != null)
                {
                    hash = hash * 59 + this.BoxCapacity.GetHashCode();
                }                
                                if (this.TrailerBoxLength != null)
                {
                    hash = hash * 59 + this.TrailerBoxLength.GetHashCode();
                }                
                                if (this.TrailerBoxWidth != null)
                {
                    hash = hash * 59 + this.TrailerBoxWidth.GetHashCode();
                }                
                                if (this.TrailerBoxHeight != null)
                {
                    hash = hash * 59 + this.TrailerBoxHeight.GetHashCode();
                }                
                                if (this.TrailerBoxCapacity != null)
                {
                    hash = hash * 59 + this.TrailerBoxCapacity.GetHashCode();
                }                
                
                return hash;
            }
        }

        #region Operators
        
        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(DumpTruck left, DumpTruck right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(DumpTruck left, DumpTruck right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
