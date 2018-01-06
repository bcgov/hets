using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// Local Area Database Model
    /// </summary>
    [MetaData (Description = "A HETS-application defined area that is within a Service Area.")]
    public sealed class LocalArea : AuditableEntity, IEquatable<LocalArea>
    {
        /// <summary>
        /// Local Area Database Model Constructor (required by entity framework)
        /// </summary>
        public LocalArea()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalArea" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a LocalArea (required).</param>
        /// <param name="localAreaNumber">A system-generated, visible to the user number for the Local Area (required).</param>
        /// <param name="name">The full name of the Local Area (required).</param>
        /// <param name="serviceArea">The Service Area in which the Local Area is found. (required).</param>
        /// <param name="startDate">The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM (required).</param>
        /// <param name="endDate">The DATE the business information ceased to be in effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM.</param>
        public LocalArea(int id, int localAreaNumber, string name, ServiceArea serviceArea, DateTime startDate, DateTime? endDate = null)
        {   
            Id = id;
            LocalAreaNumber = localAreaNumber;
            Name = name;
            ServiceArea = serviceArea;
            StartDate = startDate;
            EndDate = endDate;
        }

        /// <summary>
        /// A system-generated unique identifier for a LocalArea
        /// </summary>
        /// <value>A system-generated unique identifier for a LocalArea</value>
        [MetaData (Description = "A system-generated unique identifier for a LocalArea")]
        public int Id { get; set; }
        
        /// <summary>
        /// A system-generated, visible to the user number for the Local Area
        /// </summary>
        /// <value>A system-generated, visible to the user number for the Local Area</value>
        [MetaData (Description = "A system-generated, visible to the user number for the Local Area")]
        public int LocalAreaNumber { get; set; }
        
        /// <summary>
        /// The full name of the Local Area
        /// </summary>
        /// <value>The full name of the Local Area</value>
        [MetaData (Description = "The full name of the Local Area")]
        [MaxLength(150)]        
        public string Name { get; set; }
        
        /// <summary>
        /// The Service Area in which the Local Area is found.
        /// </summary>
        /// <value>The Service Area in which the Local Area is found.</value>
        [MetaData (Description = "The Service Area in which the Local Area is found.")]
        public ServiceArea ServiceArea { get; set; }
        
        /// <summary>
        /// Foreign key for ServiceArea 
        /// </summary>   
        [ForeignKey("ServiceArea")]
		[JsonIgnore]
		[MetaData (Description = "The Service Area in which the Local Area is found.")]
        public int? ServiceAreaId { get; set; }
        
        /// <summary>
        /// The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM
        /// </summary>
        /// <value>The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM</value>
        [MetaData (Description = "The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM")]
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// The DATE the business information ceased to be in effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM
        /// </summary>
        /// <value>The DATE the business information ceased to be in effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM</value>
        [MetaData (Description = "The DATE the business information ceased to be in effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM")]
        public DateTime? EndDate { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class LocalArea {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  LocalAreaNumber: ").Append(LocalAreaNumber).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  ServiceArea: ").Append(ServiceArea).Append("\n");
            sb.Append("  StartDate: ").Append(StartDate).Append("\n");
            sb.Append("  EndDate: ").Append(EndDate).Append("\n");
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
            return obj.GetType() == GetType() && Equals((LocalArea)obj);
        }

        /// <summary>
        /// Returns true if LocalArea instances are equal
        /// </summary>
        /// <param name="other">Instance of LocalArea to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(LocalArea other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    LocalAreaNumber == other.LocalAreaNumber ||
                    LocalAreaNumber.Equals(other.LocalAreaNumber)
                ) &&                 
                (
                    Name == other.Name ||
                    Name != null &&
                    Name.Equals(other.Name)
                ) &&                 
                (
                    ServiceArea == other.ServiceArea ||
                    ServiceArea != null &&
                    ServiceArea.Equals(other.ServiceArea)
                ) &&                 
                (
                    StartDate == other.StartDate ||
                    StartDate.Equals(other.StartDate)
                ) &&                 
                (
                    EndDate == other.EndDate ||
                    EndDate != null &&
                    EndDate.Equals(other.EndDate)
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
                hash = hash * 59 + LocalAreaNumber.GetHashCode();

                if (Name != null)
                {
                    hash = hash * 59 + Name.GetHashCode();
                }                
                                   
                if (ServiceArea != null)
                {
                    hash = hash * 59 + ServiceArea.GetHashCode();
                }         
                                
                hash = hash * 59 + StartDate.GetHashCode();               

                if (EndDate != null)
                {
                    hash = hash * 59 + EndDate.GetHashCode();
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
        public static bool operator ==(LocalArea left, LocalArea right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(LocalArea left, LocalArea right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
