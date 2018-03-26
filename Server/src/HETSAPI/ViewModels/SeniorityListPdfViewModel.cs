using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace HETSAPI.ViewModels
{
    public class SeniorityListRecord
    {
        public string LocalAreaName { get; set; }
        public string DistrictEquipmentTypeName { get; set; }
        public List<SeniorityViewModel> SeniorityList { get; set; }
    }

    /// <summary>
    /// Seniority List Pdf View Model
    /// </summary>
    [DataContract]
    public sealed class SeniorityListPdfViewModel : IEquatable<SeniorityListPdfViewModel>
    {
        /// <summary>
        /// Seniority List Pdf View Model Constructor
        /// </summary>
        public SeniorityListPdfViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeniorityListPdfViewModel" /> class.
        /// </summary>
        /// <param name="seniorityListRecords">List of seniority list records</param>
        public SeniorityListPdfViewModel(List<SeniorityListRecord> seniorityListRecords)
        {
            SeniorityListRecords = seniorityListRecords;
        }

        /// <summary>
        /// Gets or Sets Seniority List Records
        /// </summary>
        [DataMember(Name= "seniorityListRecords")]
        public List<SeniorityListRecord> SeniorityListRecords { get; set; }        

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class SeniorityListPdfViewModel {\n");
            sb.Append("  SeniorityListRecords: ").Append(SeniorityListRecords).Append("\n");            
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
            return obj.GetType() == GetType() && Equals((SeniorityListPdfViewModel)obj);
        }

        /// <summary>
        /// Returns true if SeniorityListPdfViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of SeniorityListPdfViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(SeniorityListPdfViewModel other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    SeniorityListRecords == other.SeniorityListRecords ||
                    SeniorityListRecords.Equals(other.SeniorityListRecords)
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
                hash = hash * 59 + SeniorityListRecords.GetHashCode();
                
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
        public static bool operator ==(SeniorityListPdfViewModel left, SeniorityListPdfViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(SeniorityListPdfViewModel left, SeniorityListPdfViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
