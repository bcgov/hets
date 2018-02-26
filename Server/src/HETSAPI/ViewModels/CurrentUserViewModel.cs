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
    /// Current User View Model
    /// </summary>
    [DataContract]
    public sealed class CurrentUserViewModel : IEquatable<CurrentUserViewModel>
    {
        /// <summary>
        ///  Current User View Model Constructor
        /// </summary>
        public CurrentUserViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentUserViewModel" /> class.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="givenName">GivenName.</param>
        /// <param name="surname">Surname.</param>
        /// <param name="email">Email.</param>
        /// <param name="active">Active.</param>
        /// <param name="smUserId">SmUserId.</param>
        /// <param name="smAuthorizationDirectory">SmAuthorizationDirectory.</param>
        /// <param name="userRoles">UserRoles.</param>
        /// <param name="district">The District to which this User is affliated..</param>
        public CurrentUserViewModel(int id, string givenName = null, string surname = null, string email = null, bool? active = null, 
            string smUserId = null, string smAuthorizationDirectory = null, List<UserRole> userRoles = null, 
            District district = null)
        {
            Id = id;
            GivenName = givenName;
            Surname = surname;
            Email = email;
            Active = active;
            SmUserId = smUserId;
            SmAuthorizationDirectory = smAuthorizationDirectory;
            UserRoles = userRoles;
            District = district;
        }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id")]
        public int? Id { get; set; }

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
        /// Gets or Sets Active
        /// </summary>
        [DataMember(Name="active")]
        public bool? Active { get; set; }

        /// <summary>
        /// Gets or Sets SmUserId
        /// </summary>
        [DataMember(Name="smUserId")]
        public string SmUserId { get; set; }

        /// <summary>
        /// Gets or Sets SmAuthorizationDirectory
        /// </summary>
        [DataMember(Name="smAuthorizationDirectory")]
        public string SmAuthorizationDirectory { get; set; }

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

            sb.Append("class CurrentUserViewModel {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  GivenName: ").Append(GivenName).Append("\n");
            sb.Append("  Surname: ").Append(Surname).Append("\n");
            sb.Append("  Email: ").Append(Email).Append("\n");
            sb.Append("  Active: ").Append(Active).Append("\n");
            sb.Append("  SmUserId: ").Append(SmUserId).Append("\n");
            sb.Append("  SmAuthorizationDirectory: ").Append(SmAuthorizationDirectory).Append("\n");
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
            return obj.GetType() == GetType() && Equals((CurrentUserViewModel)obj);
        }

        /// <summary>
        /// Returns true if CurrentUserViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of CurrentUserViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(CurrentUserViewModel other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id != null &&
                    Id.Equals(other.Id)
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
                    Active == other.Active ||
                    Active != null &&
                    Active.Equals(other.Active)
                ) &&                 
                (
                    SmUserId == other.SmUserId ||
                    SmUserId != null &&
                    SmUserId.Equals(other.SmUserId)
                ) &&                 
                (
                    SmAuthorizationDirectory == other.SmAuthorizationDirectory ||
                    SmAuthorizationDirectory != null &&
                    SmAuthorizationDirectory.Equals(other.SmAuthorizationDirectory)
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
                if (Id != null)
                {
                    hash = hash * 59 + Id.GetHashCode();
                }

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

                if (Active != null)
                {
                    hash = hash * 59 + Active.GetHashCode();
                }

                if (SmUserId != null)
                {
                    hash = hash * 59 + SmUserId.GetHashCode();
                }

                if (SmAuthorizationDirectory != null)
                {
                    hash = hash * 59 + SmAuthorizationDirectory.GetHashCode();
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
        public static bool operator ==(CurrentUserViewModel left, CurrentUserViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(CurrentUserViewModel left, CurrentUserViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
