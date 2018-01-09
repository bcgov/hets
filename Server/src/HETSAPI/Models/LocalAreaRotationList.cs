using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace HETSAPI.Models
{
    /// <summary>
    /// Local Area Rotation List Database Model
    /// </summary>
    public sealed class LocalAreaRotationList : AuditableEntity, IEquatable<LocalAreaRotationList>
    {
        /// <summary>
        /// Local Area Rotation List Database Model Constructor (required by entity framework)
        /// </summary>
        public LocalAreaRotationList()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalAreaRotationList" /> class.
        /// </summary>
        /// <param name="id">Id (required).</param>
        /// <param name="districtEquipmentType">A foreign key reference to the system-generated unique identifier for an Equipment Type (required).</param>
        /// <param name="localArea">LocalArea (required).</param>
        /// <param name="askNextBlock1">The id of the next piece of Block 1 Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block 1..</param>
        /// <param name="askNextBlock1Seniority">The seniority score of the piece of equipment that is the next to be asked in Block 1..</param>
        /// <param name="askNextBlock2">The id of the next piece of Block 2 Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block 2..</param>
        /// <param name="askNextBlock2Seniority">The seniority score of the piece of equipment that is the next to be asked in Block 1..</param>
        /// <param name="askNextBlockOpen">The id of the next piece of Block Open Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block Open..</param>
        public LocalAreaRotationList(int id, DistrictEquipmentType districtEquipmentType, LocalArea localArea, 
            Equipment askNextBlock1 = null, float? askNextBlock1Seniority = null, Equipment askNextBlock2 = null, 
            float? askNextBlock2Seniority = null, Equipment askNextBlockOpen = null)
        {   
            Id = id;
            DistrictEquipmentType = districtEquipmentType;
            LocalArea = localArea;
            AskNextBlock1 = askNextBlock1;
            AskNextBlock1Seniority = askNextBlock1Seniority;
            AskNextBlock2 = askNextBlock2;
            AskNextBlock2Seniority = askNextBlock2Seniority;
            AskNextBlockOpen = askNextBlockOpen;
        }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// A foreign key reference to the system-generated unique identifier for an Equipment Type
        /// </summary>
        /// <value>A foreign key reference to the system-generated unique identifier for an Equipment Type</value>
        [MetaData (Description = "A foreign key reference to the system-generated unique identifier for an Equipment Type")]
        public DistrictEquipmentType DistrictEquipmentType { get; set; }
        
        /// <summary>
        /// Foreign key for DistrictEquipmentType 
        /// </summary>   
        [ForeignKey("DistrictEquipmentType")]
		[JsonIgnore]
		[MetaData (Description = "A foreign key reference to the system-generated unique identifier for an Equipment Type")]
        public int? DistrictEquipmentTypeId { get; set; }
        
        /// <summary>
        /// Gets or Sets LocalArea
        /// </summary>
        public LocalArea LocalArea { get; set; }
        
        /// <summary>
        /// Foreign key for LocalArea 
        /// </summary>   
        [ForeignKey("LocalArea")]
		[JsonIgnore]		
        public int? LocalAreaId { get; set; }
        
        /// <summary>
        /// The id of the next piece of Block 1 Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block 1.
        /// </summary>
        /// <value>The id of the next piece of Block 1 Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block 1.</value>
        [MetaData (Description = "The id of the next piece of Block 1 Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block 1.")]
        public Equipment AskNextBlock1 { get; set; }
        
        /// <summary>
        /// Foreign key for AskNextBlock1 
        /// </summary>   
        [ForeignKey("AskNextBlock1")]
		[JsonIgnore]
		[MetaData (Description = "The id of the next piece of Block 1 Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block 1.")]
        public int? AskNextBlock1Id { get; set; }
        
        /// <summary>
        /// The seniority score of the piece of equipment that is the next to be asked in Block 1.
        /// </summary>
        /// <value>The seniority score of the piece of equipment that is the next to be asked in Block 1.</value>
        [MetaData (Description = "The seniority score of the piece of equipment that is the next to be asked in Block 1.")]
        public float? AskNextBlock1Seniority { get; set; }
        
        /// <summary>
        /// The id of the next piece of Block 2 Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block 2.
        /// </summary>
        /// <value>The id of the next piece of Block 2 Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block 2.</value>
        [MetaData (Description = "The id of the next piece of Block 2 Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block 2.")]
        public Equipment AskNextBlock2 { get; set; }
        
        /// <summary>
        /// Foreign key for AskNextBlock2 
        /// </summary>   
        [ForeignKey("AskNextBlock2")]
		[JsonIgnore]
		[MetaData (Description = "The id of the next piece of Block 2 Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block 2.")]
        public int? AskNextBlock2Id { get; set; }
        
        /// <summary>
        /// The seniority score of the piece of equipment that is the next to be asked in Block 1.
        /// </summary>
        /// <value>The seniority score of the piece of equipment that is the next to be asked in Block 1.</value>
        [MetaData (Description = "The seniority score of the piece of equipment that is the next to be asked in Block 1.")]
        public float? AskNextBlock2Seniority { get; set; }
        
        /// <summary>
        /// The id of the next piece of Block Open Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block Open.
        /// </summary>
        /// <value>The id of the next piece of Block Open Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block Open.</value>
        [MetaData (Description = "The id of the next piece of Block Open Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block Open.")]
        public Equipment AskNextBlockOpen { get; set; }
        
        /// <summary>
        /// Foreign key for AskNextBlockOpen 
        /// </summary>   
        [ForeignKey("AskNextBlockOpen")]
		[JsonIgnore]
		[MetaData (Description = "The id of the next piece of Block Open Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block Open.")]
        public int? AskNextBlockOpenId { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class LocalAreaRotationList {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  DistrictEquipmentType: ").Append(DistrictEquipmentType).Append("\n");
            sb.Append("  LocalArea: ").Append(LocalArea).Append("\n");
            sb.Append("  AskNextBlock1: ").Append(AskNextBlock1).Append("\n");
            sb.Append("  AskNextBlock1Seniority: ").Append(AskNextBlock1Seniority).Append("\n");
            sb.Append("  AskNextBlock2: ").Append(AskNextBlock2).Append("\n");
            sb.Append("  AskNextBlock2Seniority: ").Append(AskNextBlock2Seniority).Append("\n");
            sb.Append("  AskNextBlockOpen: ").Append(AskNextBlockOpen).Append("\n");
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
            return obj.GetType() == GetType() && Equals((LocalAreaRotationList)obj);
        }

        /// <summary>
        /// Returns true if LocalAreaRotationList instances are equal
        /// </summary>
        /// <param name="other">Instance of LocalAreaRotationList to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(LocalAreaRotationList other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    DistrictEquipmentType == other.DistrictEquipmentType ||
                    DistrictEquipmentType != null &&
                    DistrictEquipmentType.Equals(other.DistrictEquipmentType)
                ) &&                 
                (
                    LocalArea == other.LocalArea ||
                    LocalArea != null &&
                    LocalArea.Equals(other.LocalArea)
                ) &&                 
                (
                    AskNextBlock1 == other.AskNextBlock1 ||
                    AskNextBlock1 != null &&
                    AskNextBlock1.Equals(other.AskNextBlock1)
                ) &&                 
                (
                    AskNextBlock1Seniority == other.AskNextBlock1Seniority ||
                    AskNextBlock1Seniority != null &&
                    AskNextBlock1Seniority.Equals(other.AskNextBlock1Seniority)
                ) &&                 
                (
                    AskNextBlock2 == other.AskNextBlock2 ||
                    AskNextBlock2 != null &&
                    AskNextBlock2.Equals(other.AskNextBlock2)
                ) &&                 
                (
                    AskNextBlock2Seniority == other.AskNextBlock2Seniority ||
                    AskNextBlock2Seniority != null &&
                    AskNextBlock2Seniority.Equals(other.AskNextBlock2Seniority)
                ) &&                 
                (
                    AskNextBlockOpen == other.AskNextBlockOpen ||
                    AskNextBlockOpen != null &&
                    AskNextBlockOpen.Equals(other.AskNextBlockOpen)
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
                
                if (DistrictEquipmentType != null)
                {
                    hash = hash * 59 + DistrictEquipmentType.GetHashCode();
                }

                if (LocalArea != null)
                {
                    hash = hash * 59 + LocalArea.GetHashCode();
                }

                if (AskNextBlock1 != null)
                {
                    hash = hash * 59 + AskNextBlock1.GetHashCode();
                }

                if (AskNextBlock1Seniority != null)
                {
                    hash = hash * 59 + AskNextBlock1Seniority.GetHashCode();
                }                
                                   
                if (AskNextBlock2 != null)
                {
                    hash = hash * 59 + AskNextBlock2.GetHashCode();
                }

                if (AskNextBlock2Seniority != null)
                {
                    hash = hash * 59 + AskNextBlock2Seniority.GetHashCode();
                }                
                                   
                if (AskNextBlockOpen != null)
                {
                    hash = hash * 59 + AskNextBlockOpen.GetHashCode();
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
        public static bool operator ==(LocalAreaRotationList left, LocalAreaRotationList right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(LocalAreaRotationList left, LocalAreaRotationList right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
