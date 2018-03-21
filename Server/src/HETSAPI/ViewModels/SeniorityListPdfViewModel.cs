using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace HETSAPI.ViewModels
{
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
        /// <param name="seniorityList">List of equipment</param>
        public SeniorityListPdfViewModel(List<SeniorityViewModel> seniorityList)
        {
            SeniorityList = seniorityList;
        }

        /// <summary>
        /// Gets or Sets SeniorityList
        /// </summary>
        [DataMember(Name= "seniorityList")]
        public List<SeniorityViewModel> SeniorityList { get; set; }        

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class SeniorityListPdfViewModel {\n");
            sb.Append("  SeniorityList: ").Append(SeniorityList).Append("\n");            
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
                    SeniorityList == other.SeniorityList ||
                    SeniorityList.Equals(other.SeniorityList)
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
                hash = hash * 59 + SeniorityList.GetHashCode();
                
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
