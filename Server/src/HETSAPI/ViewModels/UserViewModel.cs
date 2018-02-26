using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using HETSAPI.Models;

namespace HETSAPI.ViewModels
{
    /// <summary>
    /// User View Model
    /// </summary>
    [DataContract]
    public sealed class UserViewModel : IEquatable<UserViewModel>
    {
        /// <summary>
        /// User View Model Constructor
        /// </summary>
        public UserViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewModel" /> class.
        /// </summary>
        /// <param name="id">Id (required).</param>
        /// <param name="active">Active (required).</param>
        /// <param name="givenName">GivenName.</param>
        /// <param name="surname">Surname.</param>
        /// <param name="email">Email.</param>
        /// <param name="smUserId">SmUserId.</param>
        /// <param name="userRoles">UserRoles.</param>
        /// <param name="district">The District to which this User is affliated..</param>
        public UserViewModel(int id, bool active, string givenName = null, string surname = null, string email = null, 
            string smUserId = null, List<UserRole> userRoles = null, District district = null)
        {   
            Id = id;
            Active = active;
            GivenName = givenName;
            Surname = surname;
            Email = email;
            SmUserId = smUserId;
            UserRoles = userRoles;
            District = district;
        }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or Sets Active
        /// </summary>
        [DataMember(Name="active")]
        public bool Active { get; set; }

        /// <summary>
        /// Gets or Sets GivenName
        /// </summary>
        [DataMember(Name="givenName")]
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or Sets Surname
        /// </summary>
        [DataMember(Name="surname")]
        public string Surname { get; set; }

        /// <summary>
        /// Gets or Sets Email
        /// </summary>
        [DataMember(Name="email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or Sets SmUserId
        /// </summary>
        [DataMember(Name="smUserId")]
        public string SmUserId { get; set; }

        /// <summary>
        /// Gets or Sets UserRoles
        /// </summary>
        [DataMember(Name="userRoles")]
        public List<UserRole> UserRoles { get; set; }

        /// <summary>
        /// The District to which this User is affliated.
        /// </summary>
        /// <value>The District to which this User is affliated.</value>
        [DataMember(Name="district")]
        [MetaData (Description = "The District to which this User is affliated.")]
        public District District { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class UserViewModel {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Active: ").Append(Active).Append("\n");
            sb.Append("  GivenName: ").Append(GivenName).Append("\n");
            sb.Append("  Surname: ").Append(Surname).Append("\n");
            sb.Append("  Email: ").Append(Email).Append("\n");
            sb.Append("  SmUserId: ").Append(SmUserId).Append("\n");
            sb.Append("  UserRoles: ").Append(UserRoles).Append("\n");
            sb.Append("  District: ").Append(District).Append("\n");
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
            return obj.GetType() == GetType() && Equals((UserViewModel)obj);
        }

        /// <summary>
        /// Returns true if UserViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of UserViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UserViewModel other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    Active == other.Active ||
                    Active.Equals(other.Active)
                ) &&                 
                (
                    GivenName == other.GivenName ||
                    GivenName != null &&
                    GivenName.Equals(other.GivenName)
                ) &&                 
                (
                    Surname == other.Surname ||
                    Surname != null &&
                    Surname.Equals(other.Surname)
                ) &&                 
                (
                    Email == other.Email ||
                    Email != null &&
                    Email.Equals(other.Email)
                ) &&                 
                (
                    SmUserId == other.SmUserId ||
                    SmUserId != null &&
                    SmUserId.Equals(other.SmUserId)
                ) && 
                (
                    UserRoles == other.UserRoles ||
                    UserRoles != null &&
                    UserRoles.SequenceEqual(other.UserRoles)
                ) &&                 
                (
                    District == other.District ||
                    District != null &&
                    District.Equals(other.District)
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
                hash = hash * 59 + Active.GetHashCode();

                if (GivenName != null)
                {
                    hash = hash * 59 + GivenName.GetHashCode();
                }

                if (Surname != null)
                {
                    hash = hash * 59 + Surname.GetHashCode();
                }

                if (Email != null)
                {
                    hash = hash * 59 + Email.GetHashCode();
                }

                if (SmUserId != null)
                {
                    hash = hash * 59 + SmUserId.GetHashCode();
                }                
                                   
                if (UserRoles != null)
                {
                    hash = hash * 59 + UserRoles.GetHashCode();
                }

                if (District != null)
                {
                    hash = hash * 59 + District.GetHashCode();
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
        public static bool operator ==(UserViewModel left, UserViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(UserViewModel left, UserViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
