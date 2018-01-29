using System;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace HETSAPI.ViewModels
{
    /// <summary>
    /// Role Permission View Model
    /// </summary>
    [DataContract]
    public sealed class RolePermissionViewModel : IEquatable<RolePermissionViewModel>
    {
        /// <summary>
        /// Role Permission View Model Constructor
        /// </summary>
        public RolePermissionViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RolePermissionViewModel" /> class.
        /// </summary>
        /// <param name="roleId">RoleId (required).</param>
        /// <param name="permissionId">PermissionId (required).</param>
        /// <param name="id">Id.</param>
        public RolePermissionViewModel(int roleId, int permissionId, int? id = null)
        {   
            RoleId = roleId;
            PermissionId = permissionId;
            Id = id;
        }

        /// <summary>
        /// Gets or Sets RoleId
        /// </summary>
        [DataMember(Name="roleId")]
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or Sets PermissionId
        /// </summary>
        [DataMember(Name="permissionId")]
        public int PermissionId { get; set; }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id")]
        public int? Id { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class RolePermissionViewModel {\n");
            sb.Append("  RoleId: ").Append(RoleId).Append("\n");
            sb.Append("  PermissionId: ").Append(PermissionId).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
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

            return Equals((RolePermissionViewModel)obj);
        }

        /// <summary>
        /// Returns true if RolePermissionViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of RolePermissionViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RolePermissionViewModel other)
        {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    RoleId == other.RoleId ||
                    RoleId.Equals(other.RoleId)
                ) &&                 
                (
                    PermissionId == other.PermissionId ||
                    PermissionId.Equals(other.PermissionId)
                ) &&                 
                (
                    Id == other.Id ||
                    Id != null &&
                    Id.Equals(other.Id)
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
                hash = hash * 59 + RoleId.GetHashCode();                                   
                hash = hash * 59 + PermissionId.GetHashCode();

                if (Id != null)
                {
                    hash = hash * 59 + Id.GetHashCode();
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
        public static bool operator ==(RolePermissionViewModel left, RolePermissionViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(RolePermissionViewModel left, RolePermissionViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
