using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace HETSAPI.Models
{
    /// <summary>
    /// User Role Database Model
    /// </summary>
    [MetaDataExtension (Description = "A join table that provides allows each user to have any number of Roles in the system.  At login time the user is given the sum of the permissions of the roles assigned to that user.")]
    public sealed class UserRole : AuditableEntity, IEquatable<UserRole>
    {
        /// <summary>
        /// User Role Database Model Constructor (required by entity framework)
        /// </summary>
        public UserRole()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRole" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a UserRole (required).</param>
        /// <param name="effectiveDate">The date on which the user was given the related role. (required).</param>
        /// <param name="expiryDate">The date on which a role previously assigned to a user was removed from that user..</param>
        /// <param name="role">A foreign key reference to the system-generated unique identifier for a Role.</param>
        public UserRole(int id, DateTime effectiveDate, DateTime? expiryDate = null, Role role = null)
        {   
            Id = id;
            EffectiveDate = effectiveDate;
            ExpiryDate = expiryDate;
            Role = role;
        }

        /// <summary>
        /// A system-generated unique identifier for a UserRole
        /// </summary>
        /// <value>A system-generated unique identifier for a UserRole</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a UserRole")]
        public int Id { get; set; }
        
        /// <summary>
        /// The date on which the user was given the related role.
        /// </summary>
        /// <value>The date on which the user was given the related role.</value>
        [MetaDataExtension (Description = "The date on which the user was given the related role.")]
        public DateTime EffectiveDate { get; set; }
        
        /// <summary>
        /// The date on which a role previously assigned to a user was removed from that user.
        /// </summary>
        /// <value>The date on which a role previously assigned to a user was removed from that user.</value>
        [MetaDataExtension (Description = "The date on which a role previously assigned to a user was removed from that user.")]
        public DateTime? ExpiryDate { get; set; }
        
        /// <summary>
        /// A foreign key reference to the system-generated unique identifier for a Role
        /// </summary>
        /// <value>A foreign key reference to the system-generated unique identifier for a Role</value>
        [MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for a Role")]
        public Role Role { get; set; }
        
        /// <summary>
        /// Foreign key for Role 
        /// </summary>   
        [ForeignKey("Role")]
		[JsonIgnore]
		[MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for a Role")]
        public int? RoleId { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class UserRole {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  EffectiveDate: ").Append(EffectiveDate).Append("\n");
            sb.Append("  ExpiryDate: ").Append(ExpiryDate).Append("\n");
            sb.Append("  Role: ").Append(Role).Append("\n");
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

            return Equals((UserRole)obj);
        }

        /// <summary>
        /// Returns true if UserRole instances are equal
        /// </summary>
        /// <param name="other">Instance of UserRole to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UserRole other)
        {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    EffectiveDate == other.EffectiveDate ||
                    EffectiveDate.Equals(other.EffectiveDate)
                ) &&                 
                (
                    ExpiryDate == other.ExpiryDate ||
                    ExpiryDate != null &&
                    ExpiryDate.Equals(other.ExpiryDate)
                ) &&                 
                (
                    Role == other.Role ||
                    Role != null &&
                    Role.Equals(other.Role)
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
                hash = hash * 59 + EffectiveDate.GetHashCode();

                if (ExpiryDate != null)
                {
                    hash = hash * 59 + ExpiryDate.GetHashCode();
                }                
                                   
                if (Role != null)
                {
                    hash = hash * 59 + Role.GetHashCode();
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
        public static bool operator ==(UserRole left, UserRole right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(UserRole left, UserRole right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
