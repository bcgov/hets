using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// Dump Truck Database Model
    /// </summary>
    [MetaData (Description = "Additional attributes about a piece of equipment designated as a Dump Truck. Historically, there was a perceived need to track a lot of informatiion about a dump truck, but in practice, few fields are being completed by users. Places for the attributes are being maintained, but the UI is prompting only for a couple of the fields, and only those fields are likely to be populated. Additional basic information about the attributes can be found on Wikipedia, BC-specific details on MOTI&amp;#39;s CVSE website and www.bclaws.ca&amp;#x2F;EPLibraries&amp;#x2F;bclaws_new&amp;#x2F;document&amp;#x2F;ID&amp;#x2F;freeside&amp;#x2F;30_78#AppendicesAtoK, Appendix A and  B. Any metrics information provided here is from the Equipment Owner.")]
    public sealed class DumpTruck : AuditableEntity, IEquatable<DumpTruck>
    {
        /// <summary>
        /// Dump Truck Database Model Constructor (required by entity framework)
        /// </summary>
        public DumpTruck()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DumpTruck" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a DumpTruck (required).</param>
        /// <param name="isSingleAxle">True if the vehicle has a single axle. Can be false or null..</param>
        /// <param name="isTandemAxle">True if the vehicle has a tandem axle. Can be false or null..</param>
        /// <param name="isTridem">True if the Dump Truck is a tridem - a three axle dump truck. Can be false or null..</param>
        /// <param name="hasPup">True if the Dump Truck has a PUP trailer - a trailer with it&amp;#39;s own hydraulic unloading system. Can be false or null..</param>
        /// <param name="hasBellyDump">True if the Dump Truck has a belly dump capability. Can be false or null..</param>
        /// <param name="hasRockBox">True if the Dump Truck has a rock box. Can be false or null..</param>
        /// <param name="hasHiliftGate">True if the Dump Truck has a high lift gate vs. a traditional gate. Can be false or null..</param>
        /// <param name="isWaterTruck">True if the Dump Truck is a Water Truck. Can be false or null..</param>
        /// <param name="hasSealcoatHitch">True if the Dump Truck has a hitch for using sealcoat trailers. Can be false or null..</param>
        /// <param name="rearAxleSpacing">The spacing of the rear axles, if applicable. Usually in metres..</param>
        /// <param name="frontTireSize">The size of of the Front Tires of the Dump Truck..</param>
        /// <param name="frontTireUom">The Unit of Measure of the Front Tire Size..</param>
        /// <param name="frontAxleCapacity">The rated capacity of the Front Axle..</param>
        /// <param name="rearAxleCapacity">The rated capacity of the Rear Axle..</param>
        /// <param name="legalLoad">The legal load of the vehicle in kg..</param>
        /// <param name="legalCapacity">The legal capacity of the dump truck..</param>
        /// <param name="legalPupTareWeight">The legal Tare Weight (weight when unloaded) of the PUP trailer..</param>
        /// <param name="licencedGvw">The Gross Vehicle Weight for which the vehicle is licensed. GVW includes the vehicle&amp;#39;s chassis, body, engine, engine fluids, fuel, accessories, driver, passengers and cargo but excluding that of any trailers..</param>
        /// <param name="licencedGvwuom">The Unit of Measure (UOM) of the licenced GVW..</param>
        /// <param name="licencedTareWeight">The licenced Tare Weight (weight when unloaded) of the vehicle..</param>
        /// <param name="licencedPupTareWeight">The licenced Tare Weight (weight when unloaded) of the PUP trailer..</param>
        /// <param name="licencedLoad">The licenced maximum load of the vehicle in kg..</param>
        /// <param name="licencedCapacity">The licenced maximum capacity of the vehicle..</param>
        /// <param name="boxLength">The length of the box, in metres. See - http-&amp;#x2F;&amp;#x2F;www.bclaws.ca&amp;#x2F;EPLibraries&amp;#x2F;bclaws_new&amp;#x2F;document&amp;#x2F;ID&amp;#x2F;freeside&amp;#x2F;30_7, appendix B.</param>
        /// <param name="boxWidth">The width of the box, in metres. See - http-&amp;#x2F;&amp;#x2F;www.bclaws.ca&amp;#x2F;EPLibraries&amp;#x2F;bclaws_new&amp;#x2F;document&amp;#x2F;ID&amp;#x2F;freeside&amp;#x2F;30_7, appendix B.</param>
        /// <param name="boxHeight">The height of the box, in metres. See- http-&amp;#x2F;&amp;#x2F;www.bclaws.ca&amp;#x2F;EPLibraries&amp;#x2F;bclaws_new&amp;#x2F;document&amp;#x2F;ID&amp;#x2F;freeside&amp;#x2F;30_7, appendix B.</param>
        /// <param name="boxCapacity">The capacity of the box..</param>
        /// <param name="trailerBoxLength">The length of the trailer box, in metres. See www.bclaws.ca&amp;#x2F;EPLibraries&amp;#x2F;bclaws_new&amp;#x2F;document&amp;#x2F;ID&amp;#x2F;freeside&amp;#x2F;30_7, appendix B.</param>
        /// <param name="trailerBoxWidth">The width of the trailer box, in metres. See www.bclaws.ca&amp;#x2F;EPLibraries&amp;#x2F;bclaws_new&amp;#x2F;document&amp;#x2F;ID&amp;#x2F;freeside&amp;#x2F;30_7, appendix B.</param>
        /// <param name="trailerBoxHeight">The height of the trailer box, in metres. See www.bclaws.ca&amp;#x2F;EPLibraries&amp;#x2F;bclaws_new&amp;#x2F;document&amp;#x2F;ID&amp;#x2F;freeside&amp;#x2F;30_7, appendix B.</param>
        /// <param name="trailerBoxCapacity">The capacity of the trailer box..</param>
        public DumpTruck(int id, bool? isSingleAxle = null, bool? isTandemAxle = null, bool? isTridem = null, bool? hasPup = null, 
            bool? hasBellyDump = null, bool? hasRockBox = null, bool? hasHiliftGate = null, bool? isWaterTruck = null, bool? hasSealcoatHitch = null, 
            string rearAxleSpacing = null, string frontTireSize = null, string frontTireUom = null, string frontAxleCapacity = null, 
            string rearAxleCapacity = null, string legalLoad = null, string legalCapacity = null, string legalPupTareWeight = null, 
            string licencedGvw = null, string licencedGvwuom = null, string licencedTareWeight = null, string licencedPupTareWeight = null, 
            string licencedLoad = null, string licencedCapacity = null, string boxLength = null, string boxWidth = null, string boxHeight = null, 
            string boxCapacity = null, string trailerBoxLength = null, string trailerBoxWidth = null, string trailerBoxHeight = null, 
            string trailerBoxCapacity = null)
        {   
            Id = id;
            IsSingleAxle = isSingleAxle;
            IsTandemAxle = isTandemAxle;
            IsTridem = isTridem;
            HasPUP = hasPup;
            HasBellyDump = hasBellyDump;
            HasRockBox = hasRockBox;
            HasHiliftGate = hasHiliftGate;
            IsWaterTruck = isWaterTruck;
            HasSealcoatHitch = hasSealcoatHitch;
            RearAxleSpacing = rearAxleSpacing;
            FrontTireSize = frontTireSize;
            FrontTireUOM = frontTireUom;
            FrontAxleCapacity = frontAxleCapacity;
            RearAxleCapacity = rearAxleCapacity;
            LegalLoad = legalLoad;
            LegalCapacity = legalCapacity;
            LegalPUPTareWeight = legalPupTareWeight;
            LicencedGVW = licencedGvw;
            LicencedGVWUOM = licencedGvwuom;
            LicencedTareWeight = licencedTareWeight;
            LicencedPUPTareWeight = licencedPupTareWeight;
            LicencedLoad = licencedLoad;
            LicencedCapacity = licencedCapacity;
            BoxLength = boxLength;
            BoxWidth = boxWidth;
            BoxHeight = boxHeight;
            BoxCapacity = boxCapacity;
            TrailerBoxLength = trailerBoxLength;
            TrailerBoxWidth = trailerBoxWidth;
            TrailerBoxHeight = trailerBoxHeight;
            TrailerBoxCapacity = trailerBoxCapacity;
        }

        /// <summary>
        /// A system-generated unique identifier for a DumpTruck
        /// </summary>
        /// <value>A system-generated unique identifier for a DumpTruck</value>
        [MetaData (Description = "A system-generated unique identifier for a DumpTruck")]
        public int Id { get; set; }
        
        /// <summary>
        /// True if the vehicle has a single axle. Can be false or null.
        /// </summary>
        /// <value>True if the vehicle has a single axle. Can be false or null.</value>
        [MetaData (Description = "True if the vehicle has a single axle. Can be false or null.")]
        public bool? IsSingleAxle { get; set; }
        
        /// <summary>
        /// True if the vehicle has a tandem axle. Can be false or null.
        /// </summary>
        /// <value>True if the vehicle has a tandem axle. Can be false or null.</value>
        [MetaData (Description = "True if the vehicle has a tandem axle. Can be false or null.")]
        public bool? IsTandemAxle { get; set; }
        
        /// <summary>
        /// True if the Dump Truck is a tridem - a three axle dump truck. Can be false or null.
        /// </summary>
        /// <value>True if the Dump Truck is a tridem - a three axle dump truck. Can be false or null.</value>
        [MetaData (Description = "True if the Dump Truck is a tridem - a three axle dump truck. Can be false or null.")]
        public bool? IsTridem { get; set; }
        
        /// <summary>
        /// True if the Dump Truck has a PUP trailer - a trailer with it&#39;s own hydraulic unloading system. Can be false or null.
        /// </summary>
        /// <value>True if the Dump Truck has a PUP trailer - a trailer with it&#39;s own hydraulic unloading system. Can be false or null.</value>
        [MetaData (Description = "True if the Dump Truck has a PUP trailer - a trailer with it&#39;s own hydraulic unloading system. Can be false or null.")]
        public bool? HasPUP { get; set; }
        
        /// <summary>
        /// True if the Dump Truck has a belly dump capability. Can be false or null.
        /// </summary>
        /// <value>True if the Dump Truck has a belly dump capability. Can be false or null.</value>
        [MetaData (Description = "True if the Dump Truck has a belly dump capability. Can be false or null.")]
        public bool? HasBellyDump { get; set; }
        
        /// <summary>
        /// True if the Dump Truck has a rock box. Can be false or null.
        /// </summary>
        /// <value>True if the Dump Truck has a rock box. Can be false or null.</value>
        [MetaData (Description = "True if the Dump Truck has a rock box. Can be false or null.")]
        public bool? HasRockBox { get; set; }
        
        /// <summary>
        /// True if the Dump Truck has a high lift gate vs. a traditional gate. Can be false or null.
        /// </summary>
        /// <value>True if the Dump Truck has a high lift gate vs. a traditional gate. Can be false or null.</value>
        [MetaData (Description = "True if the Dump Truck has a high lift gate vs. a traditional gate. Can be false or null.")]
        public bool? HasHiliftGate { get; set; }
        
        /// <summary>
        /// True if the Dump Truck is a Water Truck. Can be false or null.
        /// </summary>
        /// <value>True if the Dump Truck is a Water Truck. Can be false or null.</value>
        [MetaData (Description = "True if the Dump Truck is a Water Truck. Can be false or null.")]
        public bool? IsWaterTruck { get; set; }
        
        /// <summary>
        /// True if the Dump Truck has a hitch for using sealcoat trailers. Can be false or null.
        /// </summary>
        /// <value>True if the Dump Truck has a hitch for using sealcoat trailers. Can be false or null.</value>
        [MetaData (Description = "True if the Dump Truck has a hitch for using sealcoat trailers. Can be false or null.")]
        public bool? HasSealcoatHitch { get; set; }
        
        /// <summary>
        /// The spacing of the rear axles, if applicable. Usually in metres.
        /// </summary>
        /// <value>The spacing of the rear axles, if applicable. Usually in metres.</value>
        [MetaData (Description = "The spacing of the rear axles, if applicable. Usually in metres.")]
        [MaxLength(150)]        
        public string RearAxleSpacing { get; set; }
        
        /// <summary>
        /// The size of of the Front Tires of the Dump Truck.
        /// </summary>
        /// <value>The size of of the Front Tires of the Dump Truck.</value>
        [MetaData (Description = "The size of of the Front Tires of the Dump Truck.")]
        [MaxLength(150)]        
        public string FrontTireSize { get; set; }
        
        /// <summary>
        /// The Unit of Measure of the Front Tire Size.
        /// </summary>
        /// <value>The Unit of Measure of the Front Tire Size.</value>
        [MetaData (Description = "The Unit of Measure of the Front Tire Size.")]
        [MaxLength(150)]        
        public string FrontTireUOM { get; set; }
        
        /// <summary>
        /// The rated capacity of the Front Axle.
        /// </summary>
        /// <value>The rated capacity of the Front Axle.</value>
        [MetaData (Description = "The rated capacity of the Front Axle.")]
        [MaxLength(150)]        
        public string FrontAxleCapacity { get; set; }
        
        /// <summary>
        /// The rated capacity of the Rear Axle.
        /// </summary>
        /// <value>The rated capacity of the Rear Axle.</value>
        [MetaData (Description = "The rated capacity of the Rear Axle.")]
        [MaxLength(150)]        
        public string RearAxleCapacity { get; set; }
        
        /// <summary>
        /// The legal load of the vehicle in kg.
        /// </summary>
        /// <value>The legal load of the vehicle in kg.</value>
        [MetaData (Description = "The legal load of the vehicle in kg.")]
        [MaxLength(150)]        
        public string LegalLoad { get; set; }
        
        /// <summary>
        /// The legal capacity of the dump truck.
        /// </summary>
        /// <value>The legal capacity of the dump truck.</value>
        [MetaData (Description = "The legal capacity of the dump truck.")]
        [MaxLength(150)]        
        public string LegalCapacity { get; set; }
        
        /// <summary>
        /// The legal Tare Weight (weight when unloaded) of the PUP trailer.
        /// </summary>
        /// <value>The legal Tare Weight (weight when unloaded) of the PUP trailer.</value>
        [MetaData (Description = "The legal Tare Weight (weight when unloaded) of the PUP trailer.")]
        [MaxLength(150)]        
        public string LegalPUPTareWeight { get; set; }
        
        /// <summary>
        /// The Gross Vehicle Weight for which the vehicle is licensed. GVW includes the vehicle&#39;s chassis, body, engine, engine fluids, fuel, accessories, driver, passengers and cargo but excluding that of any trailers.
        /// </summary>
        /// <value>The Gross Vehicle Weight for which the vehicle is licensed. GVW includes the vehicle&#39;s chassis, body, engine, engine fluids, fuel, accessories, driver, passengers and cargo but excluding that of any trailers.</value>
        [MetaData (Description = "The Gross Vehicle Weight for which the vehicle is licensed. GVW includes the vehicle&#39;s chassis, body, engine, engine fluids, fuel, accessories, driver, passengers and cargo but excluding that of any trailers.")]
        [MaxLength(150)]        
        public string LicencedGVW { get; set; }
        
        /// <summary>
        /// The Unit of Measure (UOM) of the licenced GVW.
        /// </summary>
        /// <value>The Unit of Measure (UOM) of the licenced GVW.</value>
        [MetaData (Description = "The Unit of Measure (UOM) of the licenced GVW.")]
        [MaxLength(150)]        
        public string LicencedGVWUOM { get; set; }
        
        /// <summary>
        /// The licenced Tare Weight (weight when unloaded) of the vehicle.
        /// </summary>
        /// <value>The licenced Tare Weight (weight when unloaded) of the vehicle.</value>
        [MetaData (Description = "The licenced Tare Weight (weight when unloaded) of the vehicle.")]
        [MaxLength(150)]        
        public string LicencedTareWeight { get; set; }
        
        /// <summary>
        /// The licenced Tare Weight (weight when unloaded) of the PUP trailer.
        /// </summary>
        /// <value>The licenced Tare Weight (weight when unloaded) of the PUP trailer.</value>
        [MetaData (Description = "The licenced Tare Weight (weight when unloaded) of the PUP trailer.")]
        [MaxLength(150)]        
        public string LicencedPUPTareWeight { get; set; }        
        /// <summary>
        /// The licenced maximum load of the vehicle in kg.
        /// </summary>
        /// <value>The licenced maximum load of the vehicle in kg.</value>
        [MetaData (Description = "The licenced maximum load of the vehicle in kg.")]
        [MaxLength(150)]        
        public string LicencedLoad { get; set; }
        
        /// <summary>
        /// The licenced maximum capacity of the vehicle.
        /// </summary>
        /// <value>The licenced maximum capacity of the vehicle.</value>
        [MetaData (Description = "The licenced maximum capacity of the vehicle.")]
        [MaxLength(150)]        
        public string LicencedCapacity { get; set; }
        
        /// <summary>
        /// The length of the box, in metres. See - http-&#x2F;&#x2F;www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B
        /// </summary>
        /// <value>The length of the box, in metres. See - http-&#x2F;&#x2F;www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B</value>
        [MetaData (Description = "The length of the box, in metres. See - http-&#x2F;&#x2F;www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B")]
        [MaxLength(150)]        
        public string BoxLength { get; set; }
        
        /// <summary>
        /// The width of the box, in metres. See - http-&#x2F;&#x2F;www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B
        /// </summary>
        /// <value>The width of the box, in metres. See - http-&#x2F;&#x2F;www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B</value>
        [MetaData (Description = "The width of the box, in metres. See - http-&#x2F;&#x2F;www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B")]
        [MaxLength(150)]        
        public string BoxWidth { get; set; }
        
        /// <summary>
        /// The height of the box, in metres. See- http-&#x2F;&#x2F;www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B
        /// </summary>
        /// <value>The height of the box, in metres. See- http-&#x2F;&#x2F;www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B</value>
        [MetaData (Description = "The height of the box, in metres. See- http-&#x2F;&#x2F;www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B")]
        [MaxLength(150)]        
        public string BoxHeight { get; set; }
        
        /// <summary>
        /// The capacity of the box.
        /// </summary>
        /// <value>The capacity of the box.</value>
        [MetaData (Description = "The capacity of the box.")]
        [MaxLength(150)]        
        public string BoxCapacity { get; set; }
        
        /// <summary>
        /// The length of the trailer box, in metres. See www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B
        /// </summary>
        /// <value>The length of the trailer box, in metres. See www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B</value>
        [MetaData (Description = "The length of the trailer box, in metres. See www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B")]
        [MaxLength(150)]        
        public string TrailerBoxLength { get; set; }
        
        /// <summary>
        /// The width of the trailer box, in metres. See www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B
        /// </summary>
        /// <value>The width of the trailer box, in metres. See www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B</value>
        [MetaData (Description = "The width of the trailer box, in metres. See www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B")]
        [MaxLength(150)]        
        public string TrailerBoxWidth { get; set; }
        
        /// <summary>
        /// The height of the trailer box, in metres. See www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B
        /// </summary>
        /// <value>The height of the trailer box, in metres. See www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B</value>
        [MetaData (Description = "The height of the trailer box, in metres. See www.bclaws.ca&#x2F;EPLibraries&#x2F;bclaws_new&#x2F;document&#x2F;ID&#x2F;freeside&#x2F;30_7, appendix B")]
        [MaxLength(150)]        
        public string TrailerBoxHeight { get; set; }
        
        /// <summary>
        /// The capacity of the trailer box.
        /// </summary>
        /// <value>The capacity of the trailer box.</value>
        [MetaData (Description = "The capacity of the trailer box.")]
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
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            return obj.GetType() == GetType() && Equals((DumpTruck)obj);
        }

        /// <summary>
        /// Returns true if DumpTruck instances are equal
        /// </summary>
        /// <param name="other">Instance of DumpTruck to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(DumpTruck other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    IsSingleAxle == other.IsSingleAxle ||
                    IsSingleAxle != null &&
                    IsSingleAxle.Equals(other.IsSingleAxle)
                ) &&                 
                (
                    IsTandemAxle == other.IsTandemAxle ||
                    IsTandemAxle != null &&
                    IsTandemAxle.Equals(other.IsTandemAxle)
                ) &&                 
                (
                    IsTridem == other.IsTridem ||
                    IsTridem != null &&
                    IsTridem.Equals(other.IsTridem)
                ) &&                 
                (
                    HasPUP == other.HasPUP ||
                    HasPUP != null &&
                    HasPUP.Equals(other.HasPUP)
                ) &&                 
                (
                    HasBellyDump == other.HasBellyDump ||
                    HasBellyDump != null &&
                    HasBellyDump.Equals(other.HasBellyDump)
                ) &&                 
                (
                    HasRockBox == other.HasRockBox ||
                    HasRockBox != null &&
                    HasRockBox.Equals(other.HasRockBox)
                ) &&                 
                (
                    HasHiliftGate == other.HasHiliftGate ||
                    HasHiliftGate != null &&
                    HasHiliftGate.Equals(other.HasHiliftGate)
                ) &&                 
                (
                    IsWaterTruck == other.IsWaterTruck ||
                    IsWaterTruck != null &&
                    IsWaterTruck.Equals(other.IsWaterTruck)
                ) &&                 
                (
                    HasSealcoatHitch == other.HasSealcoatHitch ||
                    HasSealcoatHitch != null &&
                    HasSealcoatHitch.Equals(other.HasSealcoatHitch)
                ) &&                 
                (
                    RearAxleSpacing == other.RearAxleSpacing ||
                    RearAxleSpacing != null &&
                    RearAxleSpacing.Equals(other.RearAxleSpacing)
                ) &&                 
                (
                    FrontTireSize == other.FrontTireSize ||
                    FrontTireSize != null &&
                    FrontTireSize.Equals(other.FrontTireSize)
                ) &&                 
                (
                    FrontTireUOM == other.FrontTireUOM ||
                    FrontTireUOM != null &&
                    FrontTireUOM.Equals(other.FrontTireUOM)
                ) &&                 
                (
                    FrontAxleCapacity == other.FrontAxleCapacity ||
                    FrontAxleCapacity != null &&
                    FrontAxleCapacity.Equals(other.FrontAxleCapacity)
                ) &&                 
                (
                    RearAxleCapacity == other.RearAxleCapacity ||
                    RearAxleCapacity != null &&
                    RearAxleCapacity.Equals(other.RearAxleCapacity)
                ) &&                 
                (
                    LegalLoad == other.LegalLoad ||
                    LegalLoad != null &&
                    LegalLoad.Equals(other.LegalLoad)
                ) &&                 
                (
                    LegalCapacity == other.LegalCapacity ||
                    LegalCapacity != null &&
                    LegalCapacity.Equals(other.LegalCapacity)
                ) &&                 
                (
                    LegalPUPTareWeight == other.LegalPUPTareWeight ||
                    LegalPUPTareWeight != null &&
                    LegalPUPTareWeight.Equals(other.LegalPUPTareWeight)
                ) &&                 
                (
                    LicencedGVW == other.LicencedGVW ||
                    LicencedGVW != null &&
                    LicencedGVW.Equals(other.LicencedGVW)
                ) &&                 
                (
                    LicencedGVWUOM == other.LicencedGVWUOM ||
                    LicencedGVWUOM != null &&
                    LicencedGVWUOM.Equals(other.LicencedGVWUOM)
                ) &&                 
                (
                    LicencedTareWeight == other.LicencedTareWeight ||
                    LicencedTareWeight != null &&
                    LicencedTareWeight.Equals(other.LicencedTareWeight)
                ) &&                 
                (
                    LicencedPUPTareWeight == other.LicencedPUPTareWeight ||
                    LicencedPUPTareWeight != null &&
                    LicencedPUPTareWeight.Equals(other.LicencedPUPTareWeight)
                ) &&                 
                (
                    LicencedLoad == other.LicencedLoad ||
                    LicencedLoad != null &&
                    LicencedLoad.Equals(other.LicencedLoad)
                ) &&                 
                (
                    LicencedCapacity == other.LicencedCapacity ||
                    LicencedCapacity != null &&
                    LicencedCapacity.Equals(other.LicencedCapacity)
                ) &&                 
                (
                    BoxLength == other.BoxLength ||
                    BoxLength != null &&
                    BoxLength.Equals(other.BoxLength)
                ) &&                 
                (
                    BoxWidth == other.BoxWidth ||
                    BoxWidth != null &&
                    BoxWidth.Equals(other.BoxWidth)
                ) &&                 
                (
                    BoxHeight == other.BoxHeight ||
                    BoxHeight != null &&
                    BoxHeight.Equals(other.BoxHeight)
                ) &&                 
                (
                    BoxCapacity == other.BoxCapacity ||
                    BoxCapacity != null &&
                    BoxCapacity.Equals(other.BoxCapacity)
                ) &&                 
                (
                    TrailerBoxLength == other.TrailerBoxLength ||
                    TrailerBoxLength != null &&
                    TrailerBoxLength.Equals(other.TrailerBoxLength)
                ) &&                 
                (
                    TrailerBoxWidth == other.TrailerBoxWidth ||
                    TrailerBoxWidth != null &&
                    TrailerBoxWidth.Equals(other.TrailerBoxWidth)
                ) &&                 
                (
                    TrailerBoxHeight == other.TrailerBoxHeight ||
                    TrailerBoxHeight != null &&
                    TrailerBoxHeight.Equals(other.TrailerBoxHeight)
                ) &&                 
                (
                    TrailerBoxCapacity == other.TrailerBoxCapacity ||
                    TrailerBoxCapacity != null &&
                    TrailerBoxCapacity.Equals(other.TrailerBoxCapacity)
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
                                   
                hash = hash * 59 + Id.GetHashCode();

                if (IsSingleAxle != null)
                {
                    hash = hash * 59 + IsSingleAxle.GetHashCode();
                }

                if (IsTandemAxle != null)
                {
                    hash = hash * 59 + IsTandemAxle.GetHashCode();
                }

                if (IsTridem != null)
                {
                    hash = hash * 59 + IsTridem.GetHashCode();
                }

                if (HasPUP != null)
                {
                    hash = hash * 59 + HasPUP.GetHashCode();
                }

                if (HasBellyDump != null)
                {
                    hash = hash * 59 + HasBellyDump.GetHashCode();
                }

                if (HasRockBox != null)
                {
                    hash = hash * 59 + HasRockBox.GetHashCode();
                }

                if (HasHiliftGate != null)
                {
                    hash = hash * 59 + HasHiliftGate.GetHashCode();
                }

                if (IsWaterTruck != null)
                {
                    hash = hash * 59 + IsWaterTruck.GetHashCode();
                }

                if (HasSealcoatHitch != null)
                {
                    hash = hash * 59 + HasSealcoatHitch.GetHashCode();
                }

                if (RearAxleSpacing != null)
                {
                    hash = hash * 59 + RearAxleSpacing.GetHashCode();
                }

                if (FrontTireSize != null)
                {
                    hash = hash * 59 + FrontTireSize.GetHashCode();
                }

                if (FrontTireUOM != null)
                {
                    hash = hash * 59 + FrontTireUOM.GetHashCode();
                }

                if (FrontAxleCapacity != null)
                {
                    hash = hash * 59 + FrontAxleCapacity.GetHashCode();
                }

                if (RearAxleCapacity != null)
                {
                    hash = hash * 59 + RearAxleCapacity.GetHashCode();
                }

                if (LegalLoad != null)
                {
                    hash = hash * 59 + LegalLoad.GetHashCode();
                }

                if (LegalCapacity != null)
                {
                    hash = hash * 59 + LegalCapacity.GetHashCode();
                }

                if (LegalPUPTareWeight != null)
                {
                    hash = hash * 59 + LegalPUPTareWeight.GetHashCode();
                }

                if (LicencedGVW != null)
                {
                    hash = hash * 59 + LicencedGVW.GetHashCode();
                }

                if (LicencedGVWUOM != null)
                {
                    hash = hash * 59 + LicencedGVWUOM.GetHashCode();
                }

                if (LicencedTareWeight != null)
                {
                    hash = hash * 59 + LicencedTareWeight.GetHashCode();
                }

                if (LicencedPUPTareWeight != null)
                {
                    hash = hash * 59 + LicencedPUPTareWeight.GetHashCode();
                }

                if (LicencedLoad != null)
                {
                    hash = hash * 59 + LicencedLoad.GetHashCode();
                }

                if (LicencedCapacity != null)
                {
                    hash = hash * 59 + LicencedCapacity.GetHashCode();
                }

                if (BoxLength != null)
                {
                    hash = hash * 59 + BoxLength.GetHashCode();
                }

                if (BoxWidth != null)
                {
                    hash = hash * 59 + BoxWidth.GetHashCode();
                }

                if (BoxHeight != null)
                {
                    hash = hash * 59 + BoxHeight.GetHashCode();
                }

                if (BoxCapacity != null)
                {
                    hash = hash * 59 + BoxCapacity.GetHashCode();
                }

                if (TrailerBoxLength != null)
                {
                    hash = hash * 59 + TrailerBoxLength.GetHashCode();
                }

                if (TrailerBoxWidth != null)
                {
                    hash = hash * 59 + TrailerBoxWidth.GetHashCode();
                }

                if (TrailerBoxHeight != null)
                {
                    hash = hash * 59 + TrailerBoxHeight.GetHashCode();
                }

                if (TrailerBoxCapacity != null)
                {
                    hash = hash * 59 + TrailerBoxCapacity.GetHashCode();
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
