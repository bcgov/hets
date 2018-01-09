using System;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace HETSAPI.ViewModels
{
    /// <summary>
    /// User Role View Model
    /// </summary>
    [DataContract]
    public sealed class UserRoleViewModel : IEquatable<UserRoleViewModel>
    {
        /// <summary>
        /// User Role View Model Constructor
        /// </summary>
        public UserRoleViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleViewModel" /> class.
        /// </summary>
        /// <param name="effectiveDate">EffectiveDate (required).</param>
        /// <param name="userId">UserId (required).</param>
        /// <param name="roleId">RoleId (required).</param>
        /// <param name="id">Id.</param>
        /// <param name="expiryDate">ExpiryDate.</param>
        public UserRoleViewModel(DateTime effectiveDate, int userId, int roleId, int? id = null, DateTime? expiryDate = null)
        {   
            EffectiveDate = effectiveDate;
            UserId = userId;
            RoleId = roleId;
            Id = id;
            ExpiryDate = expiryDate;
        }

        /// <summary>
        /// Gets or Sets EffectiveDate
        /// </summary>
        [DataMember(Name="effectiveDate")]
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// Gets or Sets UserId
        /// </summary>
        [DataMember(Name="userId")]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or Sets RoleId
        /// </summary>
        [DataMember(Name="roleId")]
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id")]
        public int? Id { get; set; }

        /// <summary>
        /// Gets or Sets ExpiryDate
        /// </summary>
        [DataMember(Name="expiryDate")]
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class UserRoleViewModel {\n");
            sb.Append("  EffectiveDate: ").Append(EffectiveDate).Append("\n");
            sb.Append("  UserId: ").Append(UserId).Append("\n");
            sb.Append("  RoleId: ").Append(RoleId).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  ExpiryDate: ").Append(ExpiryDate).Append("\n");
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

            return Equals((UserRoleViewModel)obj);
        }

        /// <summary>
        /// Returns true if UserRoleViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of UserRoleViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UserRoleViewModel other)
        {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    EffectiveDate == other.EffectiveDate ||
                    EffectiveDate.Equals(other.EffectiveDate)
                ) &&                 
                (
                    UserId == other.UserId ||
                    UserId.Equals(other.UserId)
                ) &&                 
                (
                    RoleId == other.RoleId ||
                    RoleId.Equals(other.RoleId)
                ) &&                 
                (
                    Id == other.Id ||
                    Id != null &&
                    Id.Equals(other.Id)
                ) &&                 
                (
                    ExpiryDate == other.ExpiryDate ||
                    ExpiryDate != null &&
                    ExpiryDate.Equals(other.ExpiryDate)
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
                hash = hash * 59 + EffectiveDate.GetHashCode();
                hash = hash * 59 + UserId.GetHashCode();                                   
                hash = hash * 59 + RoleId.GetHashCode();

                if (Id != null)
                {
                    hash = hash * 59 + Id.GetHashCode();
                }

                if (ExpiryDate != null)
                {
                    hash = hash * 59 + ExpiryDate.GetHashCode();
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
        public static bool operator ==(UserRoleViewModel left, UserRoleViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(UserRoleViewModel left, UserRoleViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
