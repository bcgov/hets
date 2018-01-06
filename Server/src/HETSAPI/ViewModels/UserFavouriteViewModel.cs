using System;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using HETSAPI.Models;

namespace HETSAPI.ViewModels
{
    /// <summary>
    /// User Favourite View Model
    /// </summary>
    [DataContract]
    public sealed class UserFavouriteViewModel : IEquatable<UserFavouriteViewModel>
    {
        /// <summary>
        /// User Favourite View Model Constructor
        /// </summary>
        public UserFavouriteViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserFavouriteViewModel" /> class.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="name">Context Name.</param>
        /// <param name="value">Saved search.</param>
        /// <param name="isDefault">IsDefault.</param>
        /// <param name="type">Type of favourite.</param>
        public UserFavouriteViewModel(int id, string name = null, string value = null, bool? isDefault = null, string type = null)
        {
            Id = id;
            Name = name;
            Value = value;
            IsDefault = isDefault;
            Type = type;
        }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id")]
        public int? Id { get; set; }

        /// <summary>
        /// Context Name
        /// </summary>
        /// <value>Context Name</value>
        [DataMember(Name="name")]
        [MetaDataExtension (Description = "Context Name")]
        public string Name { get; set; }

        /// <summary>
        /// Saved search
        /// </summary>
        /// <value>Saved search</value>
        [DataMember(Name="value")]
        [MetaDataExtension (Description = "Saved search")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or Sets IsDefault
        /// </summary>
        [DataMember(Name="isDefault")]
        public bool? IsDefault { get; set; }

        /// <summary>
        /// Type of favourite
        /// </summary>
        /// <value>Type of favourite</value>
        [DataMember(Name="type")]
        [MetaDataExtension (Description = "Type of favourite")]
        public string Type { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class UserFavouriteViewModel {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Value: ").Append(Value).Append("\n");
            sb.Append("  IsDefault: ").Append(IsDefault).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
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

            return Equals((UserFavouriteViewModel)obj);
        }

        /// <summary>
        /// Returns true if UserFavouriteViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of UserFavouriteViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UserFavouriteViewModel other)
        {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id != null &&
                    Id.Equals(other.Id)
                ) &&                 
                (
                    Name == other.Name ||
                    Name != null &&
                    Name.Equals(other.Name)
                ) &&                 
                (
                    Value == other.Value ||
                    Value != null &&
                    Value.Equals(other.Value)
                ) &&                 
                (
                    IsDefault == other.IsDefault ||
                    IsDefault != null &&
                    IsDefault.Equals(other.IsDefault)
                ) &&                 
                (
                    Type == other.Type ||
                    Type != null &&
                    Type.Equals(other.Type)
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

                if (Name != null)
                {
                    hash = hash * 59 + Name.GetHashCode();
                }

                if (Value != null)
                {
                    hash = hash * 59 + Value.GetHashCode();
                }

                if (IsDefault != null)
                {
                    hash = hash * 59 + IsDefault.GetHashCode();
                }

                if (Type != null)
                {
                    hash = hash * 59 + Type.GetHashCode();
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
        public static bool operator ==(UserFavouriteViewModel left, UserFavouriteViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(UserFavouriteViewModel left, UserFavouriteViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
