/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HETSAPI.Models;

namespace HETSAPI.Models
{
    /// <summary>
    /// Lookup values for various enumerated types in the systems - entity status values, rate types, conditions and others. Used to pull the values out of the code and into the database but without having to have a table for each lookup instance.
    /// </summary>
        [MetaDataExtension (Description = "Lookup values for various enumerated types in the systems - entity status values, rate types, conditions and others. Used to pull the values out of the code and into the database but without having to have a table for each lookup instance.")]

    public partial class LookupList : AuditableEntity, IEquatable<LookupList>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public LookupList()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupList" /> class.
        /// </summary>
        /// <param name="Id">A system-generated unique identifier for a LookupList (required).</param>
        /// <param name="ContextName">The context within the app in which this lookup list if used. Defined and referenced in the code of the application. (required).</param>
        /// <param name="IsDefault">True of the value within the lookup list context should be the default for the lookup instance. (required).</param>
        /// <param name="CodeName">The a shorter lookup name to find the value. Can be used at the option of the application to present on the screen a short version of the lookup list value. (required).</param>
        /// <param name="Value">The fully spelled out value of the lookup entry. (required).</param>
        /// <param name="DisplaySortOrder">The sort order for list of values within a list context..</param>
        public LookupList(int Id, string ContextName, bool IsDefault, string CodeName, string Value, int? DisplaySortOrder = null)
        {   
            this.Id = Id;
            this.ContextName = ContextName;
            this.IsDefault = IsDefault;
            this.CodeName = CodeName;
            this.Value = Value;




            this.DisplaySortOrder = DisplaySortOrder;
        }

        /// <summary>
        /// A system-generated unique identifier for a LookupList
        /// </summary>
        /// <value>A system-generated unique identifier for a LookupList</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a LookupList")]
        public int Id { get; set; }
        
        /// <summary>
        /// The context within the app in which this lookup list if used. Defined and referenced in the code of the application.
        /// </summary>
        /// <value>The context within the app in which this lookup list if used. Defined and referenced in the code of the application.</value>
        [MetaDataExtension (Description = "The context within the app in which this lookup list if used. Defined and referenced in the code of the application.")]
        [MaxLength(100)]
        
        public string ContextName { get; set; }
        
        /// <summary>
        /// True of the value within the lookup list context should be the default for the lookup instance.
        /// </summary>
        /// <value>True of the value within the lookup list context should be the default for the lookup instance.</value>
        [MetaDataExtension (Description = "True of the value within the lookup list context should be the default for the lookup instance.")]
        public bool IsDefault { get; set; }
        
        /// <summary>
        /// The a shorter lookup name to find the value. Can be used at the option of the application to present on the screen a short version of the lookup list value.
        /// </summary>
        /// <value>The a shorter lookup name to find the value. Can be used at the option of the application to present on the screen a short version of the lookup list value.</value>
        [MetaDataExtension (Description = "The a shorter lookup name to find the value. Can be used at the option of the application to present on the screen a short version of the lookup list value.")]
        [MaxLength(30)]
        
        public string CodeName { get; set; }
        
        /// <summary>
        /// The fully spelled out value of the lookup entry.
        /// </summary>
        /// <value>The fully spelled out value of the lookup entry.</value>
        [MetaDataExtension (Description = "The fully spelled out value of the lookup entry.")]
        [MaxLength(100)]
        
        public string Value { get; set; }
        
        /// <summary>
        /// The sort order for list of values within a list context.
        /// </summary>
        /// <value>The sort order for list of values within a list context.</value>
        [MetaDataExtension (Description = "The sort order for list of values within a list context.")]
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
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((LookupList)obj);
        }

        /// <summary>
        /// Returns true if LookupList instances are equal
        /// </summary>
        /// <param name="other">Instance of LookupList to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(LookupList other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.ContextName == other.ContextName ||
                    this.ContextName != null &&
                    this.ContextName.Equals(other.ContextName)
                ) &&                 
                (
                    this.IsDefault == other.IsDefault ||
                    this.IsDefault.Equals(other.IsDefault)
                ) &&                 
                (
                    this.CodeName == other.CodeName ||
                    this.CodeName != null &&
                    this.CodeName.Equals(other.CodeName)
                ) &&                 
                (
                    this.Value == other.Value ||
                    this.Value != null &&
                    this.Value.Equals(other.Value)
                ) &&                 
                (
                    this.DisplaySortOrder == other.DisplaySortOrder ||
                    this.DisplaySortOrder != null &&
                    this.DisplaySortOrder.Equals(other.DisplaySortOrder)
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
                                   
                hash = hash * 59 + this.Id.GetHashCode();                if (this.ContextName != null)
                {
                    hash = hash * 59 + this.ContextName.GetHashCode();
                }                
                                   
                hash = hash * 59 + this.IsDefault.GetHashCode();
                                if (this.CodeName != null)
                {
                    hash = hash * 59 + this.CodeName.GetHashCode();
                }                
                                if (this.Value != null)
                {
                    hash = hash * 59 + this.Value.GetHashCode();
                }                
                                if (this.DisplaySortOrder != null)
                {
                    hash = hash * 59 + this.DisplaySortOrder.GetHashCode();
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
