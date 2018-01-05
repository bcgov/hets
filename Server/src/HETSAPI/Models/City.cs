using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// City Database Model
    /// </summary>
    [MetaDataExtension (Description = "A list of cities in BC. Authoritative source to be determined.")]
    public class City : AuditableEntity, IEquatable<City>
    {
        /// <summary>
        /// City Constructor (required by entity framework)
        /// </summary>
        public City()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="City" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a City (required).</param>
        /// <param name="name">The name of the City (required).</param>
        public City(int id, string name)
        {   
            Id = id;
            Name = name;
        }

        /// <summary>
        /// A system-generated unique identifier for a City
        /// </summary>
        /// <value>A system-generated unique identifier for a City</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a City")]
        public int Id { get; set; }
        
        /// <summary>
        /// The name of the City
        /// </summary>
        /// <value>The name of the City</value>
        [MetaDataExtension (Description = "The name of the City")]
        [MaxLength(150)]        
        public string Name { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class City {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
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

            return Equals((City)obj);
        }

        /// <summary>
        /// Returns true if City instances are equal
        /// </summary>
        /// <param name="other">Instance of City to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(City other)
        {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    Name == other.Name ||
                    Name != null &&
                    Name.Equals(other.Name)
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

                if (Name != null)
                {
                    hash = hash * 59 + Name.GetHashCode();
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
        public static bool operator ==(City left, City right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(City left, City right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
