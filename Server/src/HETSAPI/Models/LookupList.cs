using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// Lookup List Database Model
    /// </summary>
    [MetaData (Description = "Lookup values for various enumerated types in the systems - entity status values, rate types, conditions and others. Used to pull the values out of the code and into the database but without having to have a table for each lookup instance.")]
    public sealed class LookupList : AuditableEntity, IEquatable<LookupList>
    {
        /// <summary>
        /// Lookup List Database Model Constructor (required by entity framework)
        /// </summary>
        public LookupList()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupList" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a LookupList (required).</param>
        /// <param name="contextName">The context within the app in which this lookup list if used. Defined and referenced in the code of the application. (required).</param>
        /// <param name="isDefault">True of the value within the lookup list context should be the default for the lookup instance. (required).</param>
        /// <param name="codeName">The a shorter lookup name to find the value. Can be used at the option of the application to present on the screen a short version of the lookup list value. (required).</param>
        /// <param name="value">The fully spelled out value of the lookup entry. (required).</param>
        /// <param name="displaySortOrder">The sort order for list of values within a list context..</param>
        public LookupList(int id, string contextName, bool isDefault, string codeName, string value, int? displaySortOrder = null)
        {   
            Id = id;
            ContextName = contextName;
            IsDefault = isDefault;
            CodeName = codeName;
            Value = value;
            DisplaySortOrder = displaySortOrder;
        }

        /// <summary>
        /// A system-generated unique identifier for a LookupList
        /// </summary>
        /// <value>A system-generated unique identifier for a LookupList</value>
        [MetaData (Description = "A system-generated unique identifier for a LookupList")]
        public int Id { get; set; }
        
        /// <summary>
        /// The context within the app in which this lookup list if used. Defined and referenced in the code of the application.
        /// </summary>
        /// <value>The context within the app in which this lookup list if used. Defined and referenced in the code of the application.</value>
        [MetaData (Description = "The context within the app in which this lookup list if used. Defined and referenced in the code of the application.")]
        [MaxLength(100)]        
        public string ContextName { get; set; }
        
        /// <summary>
        /// True of the value within the lookup list context should be the default for the lookup instance.
        /// </summary>
        /// <value>True of the value within the lookup list context should be the default for the lookup instance.</value>
        [MetaData (Description = "True of the value within the lookup list context should be the default for the lookup instance.")]
        public bool IsDefault { get; set; }
        
        /// <summary>
        /// The a shorter lookup name to find the value. Can be used at the option of the application to present on the screen a short version of the lookup list value.
        /// </summary>
        /// <value>The a shorter lookup name to find the value. Can be used at the option of the application to present on the screen a short version of the lookup list value.</value>
        [MetaData (Description = "The a shorter lookup name to find the value. Can be used at the option of the application to present on the screen a short version of the lookup list value.")]
        [MaxLength(30)]        
        public string CodeName { get; set; }
        
        /// <summary>
        /// The fully spelled out value of the lookup entry.
        /// </summary>
        /// <value>The fully spelled out value of the lookup entry.</value>
        [MetaData (Description = "The fully spelled out value of the lookup entry.")]
        [MaxLength(100)]        
        public string Value { get; set; }
        
        /// <summary>
        /// The sort order for list of values within a list context.
        /// </summary>
        /// <value>The sort order for list of values within a list context.</value>
        [MetaData (Description = "The sort order for list of values within a list context.")]
        public int? DisplaySortOrder { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class LookupList {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  ContextName: ").Append(ContextName).Append("\n");
            sb.Append("  IsDefault: ").Append(IsDefault).Append("\n");
            sb.Append("  CodeName: ").Append(CodeName).Append("\n");
            sb.Append("  Value: ").Append(Value).Append("\n");
            sb.Append("  DisplaySortOrder: ").Append(DisplaySortOrder).Append("\n");
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
            return obj.GetType() == GetType() && Equals((LookupList)obj);
        }

        /// <summary>
        /// Returns true if LookupList instances are equal
        /// </summary>
        /// <param name="other">Instance of LookupList to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(LookupList other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    ContextName == other.ContextName ||
                    ContextName != null &&
                    ContextName.Equals(other.ContextName)
                ) &&                 
                (
                    IsDefault == other.IsDefault ||
                    IsDefault.Equals(other.IsDefault)
                ) &&                 
                (
                    CodeName == other.CodeName ||
                    CodeName != null &&
                    CodeName.Equals(other.CodeName)
                ) &&                 
                (
                    Value == other.Value ||
                    Value != null &&
                    Value.Equals(other.Value)
                ) &&                 
                (
                    DisplaySortOrder == other.DisplaySortOrder ||
                    DisplaySortOrder != null &&
                    DisplaySortOrder.Equals(other.DisplaySortOrder)
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

                if (ContextName != null)
                {
                    hash = hash * 59 + ContextName.GetHashCode();
                }                
                                   
                hash = hash * 59 + IsDefault.GetHashCode();

                if (CodeName != null)
                {
                    hash = hash * 59 + CodeName.GetHashCode();
                }

                if (Value != null)
                {
                    hash = hash * 59 + Value.GetHashCode();
                }

                if (DisplaySortOrder != null)
                {
                    hash = hash * 59 + DisplaySortOrder.GetHashCode();
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
        public static bool operator ==(LookupList left, LookupList right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(LookupList left, LookupList right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
