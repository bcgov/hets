/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace HETSAPI.Models
{
    /// <summary>
    /// 
    /// </summary>

    public partial class DumpTruck : IEquatable<DumpTruck>
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
        /// <param name="Id">Primary Key (required).</param>
        /// <param name="SingleAxle">SingleAxle.</param>
        /// <param name="TandemAxle">TandemAxle.</param>
        /// <param name="PUP">PUP.</param>
        /// <param name="BellyDump">BellyDump.</param>
        /// <param name="Tridem">Tridem.</param>
        /// <param name="RockBox">RockBox.</param>
        /// <param name="HiliftGate">HiliftGate.</param>
        /// <param name="WaterTruck">WaterTruck.</param>
        /// <param name="SealCoatHitch">SealCoatHitch.</param>
        /// <param name="RearAxleSpacing">RearAxleSpacing.</param>
        /// <param name="FrontTireSize">FrontTireSize.</param>
        /// <param name="FrontTireUOM">FrontTireUOM.</param>
        /// <param name="FrontAxleCapacity">FrontAxleCapacity.</param>
        /// <param name="RearAxleCapacity">RearAxleCapacity.</param>
        /// <param name="LegalLoad">LegalLoad.</param>
        /// <param name="LegalCapacity">LegalCapacity.</param>
        /// <param name="LegalPUPTareWeight">LegalPUPTareWeight.</param>
        /// <param name="LicencedGVW">LicencedGVW.</param>
        /// <param name="LicencedGVWUOM">LicencedGVWUOM.</param>
        /// <param name="LicencedTareWeight">LicencedTareWeight.</param>
        /// <param name="LicencedPUPTareWeight">LicencedPUPTareWeight.</param>
        /// <param name="LicencedLoad">LicencedLoad.</param>
        /// <param name="LicencedCapacity">LicencedCapacity.</param>
        /// <param name="BoxLength">BoxLength.</param>
        /// <param name="BoxWidth">BoxWidth.</param>
        /// <param name="BoxHeight">BoxHeight.</param>
        /// <param name="BoxCapacity">BoxCapacity.</param>
        /// <param name="TrailerBoxLength">TrailerBoxLength.</param>
        /// <param name="TrailerBoxWidth">TrailerBoxWidth.</param>
        /// <param name="TrailerBoxHeight">TrailerBoxHeight.</param>
        /// <param name="TrailerBoxCapacity">TrailerBoxCapacity.</param>
        public DumpTruck(int Id, string SingleAxle = null, string TandemAxle = null, string PUP = null, string BellyDump = null, string Tridem = null, string RockBox = null, string HiliftGate = null, string WaterTruck = null, string SealCoatHitch = null, string RearAxleSpacing = null, string FrontTireSize = null, string FrontTireUOM = null, string FrontAxleCapacity = null, string RearAxleCapacity = null, string LegalLoad = null, string LegalCapacity = null, string LegalPUPTareWeight = null, string LicencedGVW = null, string LicencedGVWUOM = null, string LicencedTareWeight = null, string LicencedPUPTareWeight = null, string LicencedLoad = null, string LicencedCapacity = null, string BoxLength = null, string BoxWidth = null, string BoxHeight = null, string BoxCapacity = null, string TrailerBoxLength = null, string TrailerBoxWidth = null, string TrailerBoxHeight = null, string TrailerBoxCapacity = null)
        {   
            this.Id = Id;
            this.SingleAxle = SingleAxle;
            this.TandemAxle = TandemAxle;
            this.PUP = PUP;
            this.BellyDump = BellyDump;
            this.Tridem = Tridem;
            this.RockBox = RockBox;
            this.HiliftGate = HiliftGate;
            this.WaterTruck = WaterTruck;
            this.SealCoatHitch = SealCoatHitch;
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
        /// Primary Key
        /// </summary>
        /// <value>Primary Key</value>
        [MetaDataExtension (Description = "Primary Key")]
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or Sets SingleAxle
        /// </summary>
        public string SingleAxle { get; set; }
        
        /// <summary>
        /// Gets or Sets TandemAxle
        /// </summary>
        public string TandemAxle { get; set; }
        
        /// <summary>
        /// Gets or Sets PUP
        /// </summary>
        public string PUP { get; set; }
        
        /// <summary>
        /// Gets or Sets BellyDump
        /// </summary>
        public string BellyDump { get; set; }
        
        /// <summary>
        /// Gets or Sets Tridem
        /// </summary>
        public string Tridem { get; set; }
        
        /// <summary>
        /// Gets or Sets RockBox
        /// </summary>
        public string RockBox { get; set; }
        
        /// <summary>
        /// Gets or Sets HiliftGate
        /// </summary>
        public string HiliftGate { get; set; }
        
        /// <summary>
        /// Gets or Sets WaterTruck
        /// </summary>
        public string WaterTruck { get; set; }
        
        /// <summary>
        /// Gets or Sets SealCoatHitch
        /// </summary>
        public string SealCoatHitch { get; set; }
        
        /// <summary>
        /// Gets or Sets RearAxleSpacing
        /// </summary>
        public string RearAxleSpacing { get; set; }
        
        /// <summary>
        /// Gets or Sets FrontTireSize
        /// </summary>
        public string FrontTireSize { get; set; }
        
        /// <summary>
        /// Gets or Sets FrontTireUOM
        /// </summary>
        public string FrontTireUOM { get; set; }
        
        /// <summary>
        /// Gets or Sets FrontAxleCapacity
        /// </summary>
        public string FrontAxleCapacity { get; set; }
        
        /// <summary>
        /// Gets or Sets RearAxleCapacity
        /// </summary>
        public string RearAxleCapacity { get; set; }
        
        /// <summary>
        /// Gets or Sets LegalLoad
        /// </summary>
        public string LegalLoad { get; set; }
        
        /// <summary>
        /// Gets or Sets LegalCapacity
        /// </summary>
        public string LegalCapacity { get; set; }
        
        /// <summary>
        /// Gets or Sets LegalPUPTareWeight
        /// </summary>
        public string LegalPUPTareWeight { get; set; }
        
        /// <summary>
        /// Gets or Sets LicencedGVW
        /// </summary>
        public string LicencedGVW { get; set; }
        
        /// <summary>
        /// Gets or Sets LicencedGVWUOM
        /// </summary>
        public string LicencedGVWUOM { get; set; }
        
        /// <summary>
        /// Gets or Sets LicencedTareWeight
        /// </summary>
        public string LicencedTareWeight { get; set; }
        
        /// <summary>
        /// Gets or Sets LicencedPUPTareWeight
        /// </summary>
        public string LicencedPUPTareWeight { get; set; }
        
        /// <summary>
        /// Gets or Sets LicencedLoad
        /// </summary>
        public string LicencedLoad { get; set; }
        
        /// <summary>
        /// Gets or Sets LicencedCapacity
        /// </summary>
        public string LicencedCapacity { get; set; }
        
        /// <summary>
        /// Gets or Sets BoxLength
        /// </summary>
        public string BoxLength { get; set; }
        
        /// <summary>
        /// Gets or Sets BoxWidth
        /// </summary>
        public string BoxWidth { get; set; }
        
        /// <summary>
        /// Gets or Sets BoxHeight
        /// </summary>
        public string BoxHeight { get; set; }
        
        /// <summary>
        /// Gets or Sets BoxCapacity
        /// </summary>
        public string BoxCapacity { get; set; }
        
        /// <summary>
        /// Gets or Sets TrailerBoxLength
        /// </summary>
        public string TrailerBoxLength { get; set; }
        
        /// <summary>
        /// Gets or Sets TrailerBoxWidth
        /// </summary>
        public string TrailerBoxWidth { get; set; }
        
        /// <summary>
        /// Gets or Sets TrailerBoxHeight
        /// </summary>
        public string TrailerBoxHeight { get; set; }
        
        /// <summary>
        /// Gets or Sets TrailerBoxCapacity
        /// </summary>
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
            sb.Append("  SingleAxle: ").Append(SingleAxle).Append("\n");
            sb.Append("  TandemAxle: ").Append(TandemAxle).Append("\n");
            sb.Append("  PUP: ").Append(PUP).Append("\n");
            sb.Append("  BellyDump: ").Append(BellyDump).Append("\n");
            sb.Append("  Tridem: ").Append(Tridem).Append("\n");
            sb.Append("  RockBox: ").Append(RockBox).Append("\n");
            sb.Append("  HiliftGate: ").Append(HiliftGate).Append("\n");
            sb.Append("  WaterTruck: ").Append(WaterTruck).Append("\n");
            sb.Append("  SealCoatHitch: ").Append(SealCoatHitch).Append("\n");
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
                    this.SingleAxle == other.SingleAxle ||
                    this.SingleAxle != null &&
                    this.SingleAxle.Equals(other.SingleAxle)
                ) &&                 
                (
                    this.TandemAxle == other.TandemAxle ||
                    this.TandemAxle != null &&
                    this.TandemAxle.Equals(other.TandemAxle)
                ) &&                 
                (
                    this.PUP == other.PUP ||
                    this.PUP != null &&
                    this.PUP.Equals(other.PUP)
                ) &&                 
                (
                    this.BellyDump == other.BellyDump ||
                    this.BellyDump != null &&
                    this.BellyDump.Equals(other.BellyDump)
                ) &&                 
                (
                    this.Tridem == other.Tridem ||
                    this.Tridem != null &&
                    this.Tridem.Equals(other.Tridem)
                ) &&                 
                (
                    this.RockBox == other.RockBox ||
                    this.RockBox != null &&
                    this.RockBox.Equals(other.RockBox)
                ) &&                 
                (
                    this.HiliftGate == other.HiliftGate ||
                    this.HiliftGate != null &&
                    this.HiliftGate.Equals(other.HiliftGate)
                ) &&                 
                (
                    this.WaterTruck == other.WaterTruck ||
                    this.WaterTruck != null &&
                    this.WaterTruck.Equals(other.WaterTruck)
                ) &&                 
                (
                    this.SealCoatHitch == other.SealCoatHitch ||
                    this.SealCoatHitch != null &&
                    this.SealCoatHitch.Equals(other.SealCoatHitch)
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
                                   
                hash = hash * 59 + this.Id.GetHashCode();                if (this.SingleAxle != null)
                {
                    hash = hash * 59 + this.SingleAxle.GetHashCode();
                }                
                                if (this.TandemAxle != null)
                {
                    hash = hash * 59 + this.TandemAxle.GetHashCode();
                }                
                                if (this.PUP != null)
                {
                    hash = hash * 59 + this.PUP.GetHashCode();
                }                
                                if (this.BellyDump != null)
                {
                    hash = hash * 59 + this.BellyDump.GetHashCode();
                }                
                                if (this.Tridem != null)
                {
                    hash = hash * 59 + this.Tridem.GetHashCode();
                }                
                                if (this.RockBox != null)
                {
                    hash = hash * 59 + this.RockBox.GetHashCode();
                }                
                                if (this.HiliftGate != null)
                {
                    hash = hash * 59 + this.HiliftGate.GetHashCode();
                }                
                                if (this.WaterTruck != null)
                {
                    hash = hash * 59 + this.WaterTruck.GetHashCode();
                }                
                                if (this.SealCoatHitch != null)
                {
                    hash = hash * 59 + this.SealCoatHitch.GetHashCode();
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
