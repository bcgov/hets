using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// Equipment Type Database Model
    /// </summary>
    [MetaData (Description = "A provincial-wide Equipment Type, the related Blue Book Chapter Section and related usage attributes.")]
    public sealed class EquipmentType : AuditableEntity, IEquatable<EquipmentType>
    {
        /// <summary>
        /// Equipment Type Database Model Constructor (required by entity framework)
        /// </summary>
        public EquipmentType()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentType" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for an EquipmentName (required).</param>
        /// <param name="name">The generic name of an equipment type - e.g. Dump Truck, Excavator and so on. (required).</param>
        /// <param name="isDumpTruck">True if the Equipment Type is a Dump Truck. Equipment of this type will have a related Dump Truck record containing dump truck-related attributes. (required).</param>
        /// <param name="blueBookSection">The section of the Blue Book that is related to equipment types of this name. (required).</param>
        /// <param name="numberOfBlocks">The number of blocks defined for the equipment of this name and Blue Book section. In general Dump Truck-class equipment types have 3 blocks, while non-Dump Truck equipment types have 2 blocks. (required).</param>
        /// <param name="blueBookRateNumber">The rate number in the Blue Book that is related to equipment types of this name..</param>
        /// <param name="maximumHours">The maximum number of hours per year that equipment types of this name&amp;#x2F;Blue Book section can work in a year.</param>
        /// <param name="extendHours">The number of extended hours per year that equipment types of this name&amp;#x2F;Blue Book section can work..</param>
        /// <param name="maxHoursSub">The number of substitute hours per year that equipment types of this name&amp;#x2F;Blue Book section can work..</param>
        public EquipmentType(int id, string name, bool isDumpTruck, float? blueBookSection, int numberOfBlocks, 
            float? blueBookRateNumber = null, float? maximumHours = null, float? extendHours = null, float? maxHoursSub = null)
        {   
            Id = id;
            Name = name;
            IsDumpTruck = isDumpTruck;
            BlueBookSection = blueBookSection;
            NumberOfBlocks = numberOfBlocks;
            BlueBookRateNumber = blueBookRateNumber;
            MaximumHours = maximumHours;
            ExtendHours = extendHours;
            MaxHoursSub = maxHoursSub;
        }

        /// <summary>
        /// A system-generated unique identifier for an EquipmentName
        /// </summary>
        /// <value>A system-generated unique identifier for an EquipmentName</value>
        [MetaData (Description = "A system-generated unique identifier for an EquipmentName")]
        public int Id { get; set; }
        
        /// <summary>
        /// The generic name of an equipment type - e.g. Dump Truck, Excavator and so on.
        /// </summary>
        /// <value>The generic name of an equipment type - e.g. Dump Truck, Excavator and so on.</value>
        [MetaData (Description = "The generic name of an equipment type - e.g. Dump Truck, Excavator and so on.")]
        [MaxLength(150)]        
        public string Name { get; set; }
        
        /// <summary>
        /// True if the Equipment Type is a Dump Truck. Equipment of this type will have a related Dump Truck record containing dump truck-related attributes.
        /// </summary>
        /// <value>True if the Equipment Type is a Dump Truck. Equipment of this type will have a related Dump Truck record containing dump truck-related attributes.</value>
        [MetaData (Description = "True if the Equipment Type is a Dump Truck. Equipment of this type will have a related Dump Truck record containing dump truck-related attributes.")]
        public bool IsDumpTruck { get; set; }
        
        /// <summary>
        /// The section of the Blue Book that is related to equipment types of this name.
        /// </summary>
        /// <value>The section of the Blue Book that is related to equipment types of this name.</value>
        [MetaData (Description = "The section of the Blue Book that is related to equipment types of this name.")]
        public float? BlueBookSection { get; set; }
        
        /// <summary>
        /// The number of blocks defined for the equipment of this name and Blue Book section. In general Dump Truck-class equipment types have 3 blocks, while non-Dump Truck equipment types have 2 blocks.
        /// </summary>
        /// <value>The number of blocks defined for the equipment of this name and Blue Book section. In general Dump Truck-class equipment types have 3 blocks, while non-Dump Truck equipment types have 2 blocks.</value>
        [MetaData (Description = "The number of blocks defined for the equipment of this name and Blue Book section. In general Dump Truck-class equipment types have 3 blocks, while non-Dump Truck equipment types have 2 blocks.")]
        public int NumberOfBlocks { get; set; }
        
        /// <summary>
        /// The rate number in the Blue Book that is related to equipment types of this name.
        /// </summary>
        /// <value>The rate number in the Blue Book that is related to equipment types of this name.</value>
        [MetaData (Description = "The rate number in the Blue Book that is related to equipment types of this name.")]
        public float? BlueBookRateNumber { get; set; }
        
        /// <summary>
        /// The maximum number of hours per year that equipment types of this name&#x2F;Blue Book section can work in a year
        /// </summary>
        /// <value>The maximum number of hours per year that equipment types of this name&#x2F;Blue Book section can work in a year</value>
        [MetaData (Description = "The maximum number of hours per year that equipment types of this name&#x2F;Blue Book section can work in a year")]
        public float? MaximumHours { get; set; }
        
        /// <summary>
        /// The number of extended hours per year that equipment types of this name&#x2F;Blue Book section can work.
        /// </summary>
        /// <value>The number of extended hours per year that equipment types of this name&#x2F;Blue Book section can work.</value>
        [MetaData (Description = "The number of extended hours per year that equipment types of this name&#x2F;Blue Book section can work.")]
        public float? ExtendHours { get; set; }
        
        /// <summary>
        /// The number of substitute hours per year that equipment types of this name&#x2F;Blue Book section can work.
        /// </summary>
        /// <value>The number of substitute hours per year that equipment types of this name&#x2F;Blue Book section can work.</value>
        [MetaData (Description = "The number of substitute hours per year that equipment types of this name&#x2F;Blue Book section can work.")]
        public float? MaxHoursSub { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class EquipmentType {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  IsDumpTruck: ").Append(IsDumpTruck).Append("\n");
            sb.Append("  BlueBookSection: ").Append(BlueBookSection).Append("\n");
            sb.Append("  NumberOfBlocks: ").Append(NumberOfBlocks).Append("\n");
            sb.Append("  BlueBookRateNumber: ").Append(BlueBookRateNumber).Append("\n");
            sb.Append("  MaximumHours: ").Append(MaximumHours).Append("\n");
            sb.Append("  ExtendHours: ").Append(ExtendHours).Append("\n");
            sb.Append("  MaxHoursSub: ").Append(MaxHoursSub).Append("\n");
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
            return obj.GetType() == GetType() && Equals((EquipmentType)obj);
        }

        /// <summary>
        /// Returns true if EquipmentType instances are equal
        /// </summary>
        /// <param name="other">Instance of EquipmentType to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(EquipmentType other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    Name == other.Name ||
                    Name != null &&
                    Name.Equals(other.Name)
                ) &&                 
                (
                    IsDumpTruck == other.IsDumpTruck ||
                    IsDumpTruck.Equals(other.IsDumpTruck)
                ) &&                 
                (
                    BlueBookSection == other.BlueBookSection ||
                    BlueBookSection != null &&
                    BlueBookSection.Equals(other.BlueBookSection)
                ) &&                 
                (
                    NumberOfBlocks == other.NumberOfBlocks ||
                    NumberOfBlocks.Equals(other.NumberOfBlocks)
                ) &&                 
                (
                    BlueBookRateNumber == other.BlueBookRateNumber ||
                    BlueBookRateNumber != null &&
                    BlueBookRateNumber.Equals(other.BlueBookRateNumber)
                ) &&                 
                (
                    MaximumHours == other.MaximumHours ||
                    MaximumHours != null &&
                    MaximumHours.Equals(other.MaximumHours)
                ) &&                 
                (
                    ExtendHours == other.ExtendHours ||
                    ExtendHours != null &&
                    ExtendHours.Equals(other.ExtendHours)
                ) &&                 
                (
                    MaxHoursSub == other.MaxHoursSub ||
                    MaxHoursSub != null &&
                    MaxHoursSub.Equals(other.MaxHoursSub)
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

                if (Name != null)
                {
                    hash = hash * 59 + Name.GetHashCode();
                }                
                                   
                hash = hash * 59 + IsDumpTruck.GetHashCode();
                                   
                if (BlueBookSection != null)
                {
                    hash = hash * 59 + BlueBookSection.GetHashCode();
                }

                hash = hash * 59 + NumberOfBlocks.GetHashCode();

                if (BlueBookRateNumber != null)
                {
                    hash = hash * 59 + BlueBookRateNumber.GetHashCode();
                }

                if (MaximumHours != null)
                {
                    hash = hash * 59 + MaximumHours.GetHashCode();
                }

                if (ExtendHours != null)
                {
                    hash = hash * 59 + ExtendHours.GetHashCode();
                }

                if (MaxHoursSub != null)
                {
                    hash = hash * 59 + MaxHoursSub.GetHashCode();
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
        public static bool operator ==(EquipmentType left, EquipmentType right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(EquipmentType left, EquipmentType right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
