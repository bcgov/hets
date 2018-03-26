using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using HETSAPI.Models;

namespace HETSAPI.ViewModels
{
    /// <summary>
    /// Time Record View Model
    /// </summary>
    [DataContract]
    public sealed class TimeRecordViewModel : IEquatable<TimeRecordViewModel>
    {
        /// <summary>
        /// Time Record View Model Constructor
        /// </summary>
        public TimeRecordViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeRecordViewModel" /> class.
        /// </summary>
        /// <param name="equipmentCode">EquipmentCode (from Equipment Record)</param>
        /// <param name="projectName">Project Name (from the project/agreement)</param>
        /// <param name="provincialProjectNumber">Provincial Project Number</param>
        /// <param name="maximumHours">Returns the Maxumum Hours for this Type of Equipment</param>
        /// <param name="hoursYtd">Hours YTD (calculated)</param>
        /// <param name="timeRecords">The TimeRecords for this agreement</param>
        public TimeRecordViewModel(string equipmentCode, string projectName, string provincialProjectNumber, int maximumHours,
            float? hoursYtd = null, List<TimeRecord> timeRecords = null)
        {
            EquipmentCode = equipmentCode;
            ProjectName = projectName;
            ProvincialProjectNumber = provincialProjectNumber;
            MaximumHours = maximumHours;
            HoursYtd = hoursYtd;            
            TimeRecords = timeRecords;
        }

        /// <summary>
        /// EquipmentCode (from Equipment Record)
        /// </summary>
        [DataMember(Name = "equipmentCode")]
        public string EquipmentCode { get; set; }        

        /// <summary>
        /// Project Name (from the project/agreement)
        /// </summary>
        [DataMember(Name = "projectName")]
        public string ProjectName { get; set; }

        /// <summary>
        /// Project Number
        /// </summary>
        [DataMember(Name = "provincialProjectNumber")]
        public string ProvincialProjectNumber { get; set; }

        /// <summary>
        /// Hours YTD (calculated)
        /// </summary>
        [DataMember(Name = "hoursYtd")]
        public float? HoursYtd { get; set; }

        /// <summary>
        /// Returns the Maxumum Hours for this Type of Equipment
        /// </summary>
        [DataMember(Name = "maximumHours")]
        public int MaximumHours { get; set; }
       
        /// <summary>
        /// Gets or Sets TimeRecords
        /// </summary>
        [DataMember(Name="timeRecords")]
        public List<TimeRecord> TimeRecords { get; set; }        

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();            

            sb.Append("class TimeRecordViewModel {\n");
            sb.Append("  EquipmentCode: ").Append(EquipmentCode).Append("\n");
            sb.Append("  ProjectName: ").Append(ProjectName).Append("\n");
            sb.Append("  ProvincialProjectNumber: ").Append(ProvincialProjectNumber).Append("\n");
            sb.Append("  MaximumHours: ").Append(MaximumHours).Append("\n");
            sb.Append("  HoursYtd: ").Append(HoursYtd).Append("\n");
            sb.Append("  TimeRecords: ").Append(TimeRecords).Append("\n");
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
            return obj.GetType() == GetType() && Equals((TimeRecordViewModel)obj);
        }

        /// <summary>
        /// Returns true if TimeRecordViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of TimeRecordViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(TimeRecordViewModel other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    EquipmentCode == other.EquipmentCode ||
                    EquipmentCode.Equals(other.EquipmentCode)
                ) &&                 
                (
                    ProjectName == other.ProjectName ||
                    ProjectName.Equals(other.ProjectName)
                ) &&
                (
                    ProvincialProjectNumber == other.ProvincialProjectNumber ||
                    ProvincialProjectNumber.Equals(other.ProvincialProjectNumber)
                ) &&
                (
                    MaximumHours == other.MaximumHours ||
                    MaximumHours.Equals(other.MaximumHours)
                ) &&
                (
                    HoursYtd == other.HoursYtd ||
                    HoursYtd != null &&
                    HoursYtd.Equals(other.HoursYtd)
                )  &&                 
                (
                    TimeRecords == other.TimeRecords ||
                    TimeRecords != null &&
                    TimeRecords.Equals(other.TimeRecords)
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
                hash = hash * 59 + EquipmentCode.GetHashCode();                   
                hash = hash * 59 + ProjectName.GetHashCode();
                hash = hash * 59 + ProvincialProjectNumber.GetHashCode();
                hash = hash * 59 + MaximumHours.GetHashCode();

                if (HoursYtd != null)
                {
                    hash = hash * 59 + HoursYtd.GetHashCode();
                }

                if (TimeRecords != null)
                {
                    hash = hash * 59 + TimeRecords.GetHashCode();
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
        public static bool operator ==(TimeRecordViewModel left, TimeRecordViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(TimeRecordViewModel left, TimeRecordViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
