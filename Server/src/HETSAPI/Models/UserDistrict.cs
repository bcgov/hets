using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Newtonsoft.Json;

namespace HETSAPI.Models
{
    /// <summary>
    /// User District Database Model
    /// </summary>
    [MetaData (Description = "Manages users who work in multiple districts.")]
    public sealed class UserDistrict : AuditableEntity, IEquatable<UserDistrict>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public UserDistrict()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDistrict" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a UserDistrict (required).</param>
        /// <param name="user">A User that this userDistrict is associated with</param>
        /// <param name="district">A District that this userDistrict is associated with</param>
        /// <param name="isPrimary">A flag indicating if this is the Primary District for this user (required).</param>        
        public UserDistrict(int id, User user, District district, bool isPrimary = false)
        {   
            Id = id;
            User = user;
            District = district;
            IsPrimary = isPrimary;            
        }

        /// <summary>
        /// A system-generated unique identifier for a UserDistrict
        /// </summary>
        /// <value>A system-generated unique identifier for a UserDistrict</value>
        [MetaData(Description = "A system-generated unique identifier for a UserDistrict.")]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or Sets User.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Foreign key for User.
        /// </summary>   
        [ForeignKey("User")]
        [JsonIgnore]
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or Sets District.
        /// </summary>
        public District District { get; set; }

        /// <summary>
        /// Foreign key for District.
        /// </summary>   
        [ForeignKey("District")]
        [JsonIgnore]
        public int? DistrictId { get; set; }

        /// <summary>
        /// A flag indicating if this is the Primary District for this user.
        /// </summary>
        /// <value>A flag indicating if this is the Primary District for this user.</value>
        [MetaData (Description = "A flag indicating if this is the Primary District for this user.")]
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class UserDistrict {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  User: ").Append(User).Append("\n");
            sb.Append("  District: ").Append(District).Append("\n");
            sb.Append("  IsPrimary: ").Append(IsPrimary).Append("\n");
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
            return obj.GetType() == GetType() && Equals((UserDistrict)obj);
        }

        /// <summary>
        /// Returns true if UserDistrict instances are equal
        /// </summary>
        /// <param name="other">Instance of UserDistrict to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UserDistrict other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    User == other.User ||
                    User.Equals(other.User)
                ) &&
                (
                    District == other.District ||
                    District.Equals(other.District)
                ) &&                 
                (
                    IsPrimary == other.IsPrimary ||
                    IsPrimary.Equals(other.IsPrimary)
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
                hash = hash * 59 + User.GetHashCode();
                hash = hash * 59 + District.GetHashCode();
                hash = hash * 59 + IsPrimary.GetHashCode();
                
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
        public static bool operator ==(UserDistrict left, UserDistrict right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(UserDistrict left, UserDistrict right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
