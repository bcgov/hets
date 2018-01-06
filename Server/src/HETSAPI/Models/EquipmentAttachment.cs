using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// Equipment Attachment Database Model
    /// </summary>
    [MetaDataExtension (Description = "An Equipment Attachment associated with a piece of Equipment.")]
    public class EquipmentAttachment : AuditableEntity, IEquatable<EquipmentAttachment>
    {
        /// <summary>
        /// Equipment Attachment Database Model Constructor (required by entity framework)
        /// </summary>
        public EquipmentAttachment()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentAttachment" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for an EquipmentAttachment (required).</param>
        /// <param name="equipment">Equipment (required).</param>
        /// <param name="typeName">The name of the attachment type (required).</param>
        /// <param name="description">A description of the equipment attachment if the Equipment Attachment Type Name is insufficient..</param>
        public EquipmentAttachment(int id, Equipment equipment, string typeName, string description = null)
        {   
            Id = id;
            Equipment = equipment;
            TypeName = typeName;
            Description = description;
        }

        /// <summary>
        /// A system-generated unique identifier for an EquipmentAttachment
        /// </summary>
        /// <value>A system-generated unique identifier for an EquipmentAttachment</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for an EquipmentAttachment")]
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or Sets Equipment
        /// </summary>
        public Equipment Equipment { get; set; }
        
        /// <summary>
        /// Foreign key for Equipment 
        /// </summary>   
        [ForeignKey("Equipment")]
		[JsonIgnore]		
        public int? EquipmentId { get; set; }
        
        /// <summary>
        /// The name of the attachment type
        /// </summary>
        /// <value>The name of the attachment type</value>
        [MetaDataExtension (Description = "The name of the attachment type")]
        [MaxLength(100)]        
        public string TypeName { get; set; }
        
        /// <summary>
        /// A description of the equipment attachment if the Equipment Attachment Type Name is insufficient.
        /// </summary>
        /// <value>A description of the equipment attachment if the Equipment Attachment Type Name is insufficient.</value>
        [MetaDataExtension (Description = "A description of the equipment attachment if the Equipment Attachment Type Name is insufficient.")]
        [MaxLength(2048)]        
        public string Description { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class EquipmentAttachment {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Equipment: ").Append(Equipment).Append("\n");
            sb.Append("  TypeName: ").Append(TypeName).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
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

            return Equals((EquipmentAttachment)obj);
        }

        /// <summary>
        /// Returns true if EquipmentAttachment instances are equal
        /// </summary>
        /// <param name="other">Instance of EquipmentAttachment to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(EquipmentAttachment other)
        {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    Equipment == other.Equipment ||
                    Equipment != null &&
                    Equipment.Equals(other.Equipment)
                ) &&                 
                (
                    TypeName == other.TypeName ||
                    TypeName != null &&
                    TypeName.Equals(other.TypeName)
                ) &&                 
                (
                    Description == other.Description ||
                    Description != null &&
                    Description.Equals(other.Description)
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

                if (Equipment != null)
                {
                    hash = hash * 59 + Equipment.GetHashCode();
                }

                if (TypeName != null)
                {
                    hash = hash * 59 + TypeName.GetHashCode();
                }

                if (Description != null)
                {
                    hash = hash * 59 + Description.GetHashCode();
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
        public static bool operator ==(EquipmentAttachment left, EquipmentAttachment right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(EquipmentAttachment left, EquipmentAttachment right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
