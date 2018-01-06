using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace HETSAPI.Models
{
    /// <summary>
    /// Role Permission Database Model
    /// </summary>
    [MetaDataExtension (Description = "A permission that is part of a Role - a component of the authorization provided by the Role to the user to which the Role is assigned.")]
    public sealed class RolePermission : AuditableEntity, IEquatable<RolePermission>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public RolePermission()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RolePermission" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a RolePermission (required).</param>
        /// <param name="role">Role (required).</param>
        /// <param name="permission">A foreign key reference to the system-generated unique identifier for a Permission (required).</param>
        public RolePermission(int id, Role role, Permission permission)
        {   
            Id = id;
            Role = role;
            Permission = permission;
        }

        /// <summary>
        /// A system-generated unique identifier for a RolePermission
        /// </summary>
        /// <value>A system-generated unique identifier for a RolePermission</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a RolePermission")]
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or Sets Role
        /// </summary>
        public Role Role { get; set; }
        
        /// <summary>
        /// Foreign key for Role 
        /// </summary>   
        [ForeignKey("Role")]
		[JsonIgnore]		
        public int? RoleId { get; set; }
        
        /// <summary>
        /// A foreign key reference to the system-generated unique identifier for a Permission
        /// </summary>
        /// <value>A foreign key reference to the system-generated unique identifier for a Permission</value>
        [MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for a Permission")]
        public Permission Permission { get; set; }
        
        /// <summary>
        /// Foreign key for Permission 
        /// </summary>   
        [ForeignKey("Permission")]
		[JsonIgnore]
		[MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for a Permission")]
        public int? PermissionId { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class RolePermission {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Role: ").Append(Role).Append("\n");
            sb.Append("  Permission: ").Append(Permission).Append("\n");
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

            return Equals((RolePermission)obj);
        }

        /// <summary>
        /// Returns true if RolePermission instances are equal
        /// </summary>
        /// <param name="other">Instance of RolePermission to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RolePermission other)
        {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    Role == other.Role ||
                    Role != null &&
                    Role.Equals(other.Role)
                ) &&                 
                (
                    Permission == other.Permission ||
                    Permission != null &&
                    Permission.Equals(other.Permission)
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
                
                if (Role != null)
                {
                    hash = hash * 59 + Role.GetHashCode();
                }

                if (Permission != null)
                {
                    hash = hash * 59 + Permission.GetHashCode();
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
        public static bool operator ==(RolePermission left, RolePermission right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(RolePermission left, RolePermission right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
