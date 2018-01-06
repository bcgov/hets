using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace HETSAPI.ViewModels
{
    /// <summary>
    /// User Detail View Model
    /// </summary>
    [DataContract]
    public sealed class UserDetailsViewModel : IEquatable<UserDetailsViewModel>
    {
        /// <summary>
        /// User Detail View Model Constructor
        /// </summary>
        public UserDetailsViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDetailsViewModel" /> class.
        /// </summary>
        /// <param name="id">Id (required).</param>
        /// <param name="active">Active (required).</param>
        /// <param name="givenName">GivenName.</param>
        /// <param name="surname">Surname.</param>
        /// <param name="initials">Initials.</param>
        /// <param name="email">Email.</param>
        /// <param name="permissions">Permissions.</param>
        public UserDetailsViewModel(int id, bool active, string givenName = null, string surname = null, string initials = null, string email = null, List<PermissionViewModel> permissions = null)
        {   
            Id = id;
            Active = active;
            GivenName = givenName;
            Surname = surname;
            Initials = initials;
            Email = email;
            Permissions = permissions;
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
        /// Gets or Sets Initials
        /// </summary>
        [DataMember(Name="initials")]
        public string Initials { get; set; }

        /// <summary>
        /// Gets or Sets Email
        /// </summary>
        [DataMember(Name="email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or Sets Permissions
        /// </summary>
        [DataMember(Name="permissions")]
        public List<PermissionViewModel> Permissions { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class UserDetailsViewModel {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Active: ").Append(Active).Append("\n");
            sb.Append("  GivenName: ").Append(GivenName).Append("\n");
            sb.Append("  Surname: ").Append(Surname).Append("\n");
            sb.Append("  Initials: ").Append(Initials).Append("\n");
            sb.Append("  Email: ").Append(Email).Append("\n");
            sb.Append("  Permissions: ").Append(Permissions).Append("\n");
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

            return Equals((UserDetailsViewModel)obj);
        }

        /// <summary>
        /// Returns true if UserDetailsViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of UserDetailsViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UserDetailsViewModel other)
        {
            if (ReferenceEquals(null, other)) { return false; }
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
                    Initials == other.Initials ||
                    Initials != null &&
                    Initials.Equals(other.Initials)
                ) &&                 
                (
                    Email == other.Email ||
                    Email != null &&
                    Email.Equals(other.Email)
                ) && 
                (
                    Permissions == other.Permissions ||
                    Permissions != null &&
                    Permissions.SequenceEqual(other.Permissions)
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

                if (Initials != null)
                {
                    hash = hash * 59 + Initials.GetHashCode();
                }

                if (Email != null)
                {
                    hash = hash * 59 + Email.GetHashCode();
                }                
                                   
                if (Permissions != null)
                {
                    hash = hash * 59 + Permissions.GetHashCode();
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
        public static bool operator ==(UserDetailsViewModel left, UserDetailsViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(UserDetailsViewModel left, UserDetailsViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
