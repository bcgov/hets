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

namespace HETSAPI.Models
{
    /// <summary>
    /// The history of all changes to the seniority of a piece of equipment. The current seniority information (underlying data elements and the calculation result) is in the equipment record. Every time that information changes, the old values are copied to here, with a start date, end date range. In the normal case, an annual update triggers the old values being copied here and the new values put into the equipment record. If a user manually changes the values, the existing values are copied into a record added here.
    /// </summary>
        [MetaDataExtension (Description = "The history of all changes to the seniority of a piece of equipment. The current seniority information (underlying data elements and the calculation result) is in the equipment record. Every time that information changes, the old values are copied to here, with a start date, end date range. In the normal case, an annual update triggers the old values being copied here and the new values put into the equipment record. If a user manually changes the values, the existing values are copied into a record added here.")]

    public partial class SeniorityAudit : IEquatable<SeniorityAudit>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public SeniorityAudit()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeniorityAudit" /> class.
        /// </summary>
        /// <param name="Id">A system-generated unique identifier for a SeniorityAudit (required).</param>
        /// <param name="GeneratedTime">The effective starting date that these the Seniority data in this record went into effect..</param>
        /// <param name="LocalArea">LocalArea.</param>
        /// <param name="Equipment">Equipment.</param>
        /// <param name="BlockNumber">The block number for the piece of equipment as calculated by the Seniority Algorthm for this equipment type in the local area. As currently defined by the business - 1, 2 or Open.</param>
        /// <param name="EquipCd">A human-visible unique code for the piece of equipment, referenced for convenience by the system users - HETS Clerks and Equipment Owners. Generated based on a unique Equipment owner prefix (e.g. EDW) and a zero-filled unique number - resulting in a code like EDW-0083..</param>
        /// <param name="Owner">Owner.</param>
        /// <param name="OwnerName">The name of the owner from the Owner Record, captured at the time this record was created..</param>
        /// <param name="Seniority">The seniority calculation result for this piece of equipment. The calculation is based on the &amp;quot;numYears&amp;quot; of service + average hours of service over the last three fiscal years - as stored in the related fields (serviceHoursLastYear, serviceHoursTwoYearsAgo serviceHoursThreeYearsAgo)..</param>
        /// <param name="YTD">Sum of hours in the current fiscal year&amp;#39;s time cards captured at the time this record was created..</param>
        /// <param name="ServiceHoursLastYear">Number of hours of service by this piece of equipment in the previous fiscal year.</param>
        /// <param name="ServiceHoursTwoYearsAgo">Number of hours of service by this piece of equipment in the fiscal year before the last one - e.g. if current year is FY2018 then hours in FY2016.</param>
        /// <param name="ServiceHoursThreeYearsAgo">Number of hours of service by this piece of equipment in the fiscal year three years ago - e.g. if current year is FY2018 then hours in FY2015.</param>
        /// <param name="CycleHrsWrk">TO BE REMOVED - Not sure why this would be needed.</param>
        /// <param name="FrozenOut">TO BE REMOVED - not to be supported in new version of HETS.</param>
        /// <param name="Project">TO BE REMOVED - Not sure why this would be needed.</param>
        /// <param name="Working">TO BE REMOVED - Not sure why this would be needed.</param>
        /// <param name="YearEndReg">TO BE REMOVED - Not sure why this would be needed.</param>
        public SeniorityAudit(int Id, DateTime? GeneratedTime = null, LocalArea LocalArea = null, Equipment Equipment = null, float? BlockNumber = null, string EquipCd = null, Owner Owner = null, string OwnerName = null, float? Seniority = null, float? YTD = null, float? ServiceHoursLastYear = null, float? ServiceHoursTwoYearsAgo = null, float? ServiceHoursThreeYearsAgo = null, float? CycleHrsWrk = null, string FrozenOut = null, Project Project = null, string Working = null, string YearEndReg = null)
        {   
            this.Id = Id;
            this.GeneratedTime = GeneratedTime;
            this.LocalArea = LocalArea;
            this.Equipment = Equipment;
            this.BlockNumber = BlockNumber;
            this.EquipCd = EquipCd;
            this.Owner = Owner;
            this.OwnerName = OwnerName;
            this.Seniority = Seniority;
            this.YTD = YTD;
            this.ServiceHoursLastYear = ServiceHoursLastYear;
            this.ServiceHoursTwoYearsAgo = ServiceHoursTwoYearsAgo;
            this.ServiceHoursThreeYearsAgo = ServiceHoursThreeYearsAgo;
            this.CycleHrsWrk = CycleHrsWrk;
            this.FrozenOut = FrozenOut;
            this.Project = Project;
            this.Working = Working;
            this.YearEndReg = YearEndReg;
        }

        /// <summary>
        /// A system-generated unique identifier for a SeniorityAudit
        /// </summary>
        /// <value>A system-generated unique identifier for a SeniorityAudit</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a SeniorityAudit")]
        public int Id { get; set; }
        
        /// <summary>
        /// The effective starting date that these the Seniority data in this record went into effect.
        /// </summary>
        /// <value>The effective starting date that these the Seniority data in this record went into effect.</value>
        [MetaDataExtension (Description = "The effective starting date that these the Seniority data in this record went into effect.")]
        public DateTime? GeneratedTime { get; set; }
        
        /// <summary>
        /// Gets or Sets LocalArea
        /// </summary>
        public LocalArea LocalArea { get; set; }
        
        /// <summary>
        /// Foreign key for LocalArea 
        /// </summary>       
        [ForeignKey("LocalArea")]
        public int? LocalAreaRefId { get; set; }
        
        /// <summary>
        /// Gets or Sets Equipment
        /// </summary>
        public Equipment Equipment { get; set; }
        
        /// <summary>
        /// Foreign key for Equipment 
        /// </summary>       
        [ForeignKey("Equipment")]
        public int? EquipmentRefId { get; set; }
        
        /// <summary>
        /// The block number for the piece of equipment as calculated by the Seniority Algorthm for this equipment type in the local area. As currently defined by the business - 1, 2 or Open
        /// </summary>
        /// <value>The block number for the piece of equipment as calculated by the Seniority Algorthm for this equipment type in the local area. As currently defined by the business - 1, 2 or Open</value>
        [MetaDataExtension (Description = "The block number for the piece of equipment as calculated by the Seniority Algorthm for this equipment type in the local area. As currently defined by the business - 1, 2 or Open")]
        public float? BlockNumber { get; set; }
        
        /// <summary>
        /// A human-visible unique code for the piece of equipment, referenced for convenience by the system users - HETS Clerks and Equipment Owners. Generated based on a unique Equipment owner prefix (e.g. EDW) and a zero-filled unique number - resulting in a code like EDW-0083.
        /// </summary>
        /// <value>A human-visible unique code for the piece of equipment, referenced for convenience by the system users - HETS Clerks and Equipment Owners. Generated based on a unique Equipment owner prefix (e.g. EDW) and a zero-filled unique number - resulting in a code like EDW-0083.</value>
        [MetaDataExtension (Description = "A human-visible unique code for the piece of equipment, referenced for convenience by the system users - HETS Clerks and Equipment Owners. Generated based on a unique Equipment owner prefix (e.g. EDW) and a zero-filled unique number - resulting in a code like EDW-0083.")]
        [MaxLength(255)]
        
        public string EquipCd { get; set; }
        
        /// <summary>
        /// Gets or Sets Owner
        /// </summary>
        public Owner Owner { get; set; }
        
        /// <summary>
        /// Foreign key for Owner 
        /// </summary>       
        [ForeignKey("Owner")]
        public int? OwnerRefId { get; set; }
        
        /// <summary>
        /// The name of the owner from the Owner Record, captured at the time this record was created.
        /// </summary>
        /// <value>The name of the owner from the Owner Record, captured at the time this record was created.</value>
        [MetaDataExtension (Description = "The name of the owner from the Owner Record, captured at the time this record was created.")]
        [MaxLength(255)]
        
        public string OwnerName { get; set; }
        
        /// <summary>
        /// The seniority calculation result for this piece of equipment. The calculation is based on the &quot;numYears&quot; of service + average hours of service over the last three fiscal years - as stored in the related fields (serviceHoursLastYear, serviceHoursTwoYearsAgo serviceHoursThreeYearsAgo).
        /// </summary>
        /// <value>The seniority calculation result for this piece of equipment. The calculation is based on the &quot;numYears&quot; of service + average hours of service over the last three fiscal years - as stored in the related fields (serviceHoursLastYear, serviceHoursTwoYearsAgo serviceHoursThreeYearsAgo).</value>
        [MetaDataExtension (Description = "The seniority calculation result for this piece of equipment. The calculation is based on the &quot;numYears&quot; of service + average hours of service over the last three fiscal years - as stored in the related fields (serviceHoursLastYear, serviceHoursTwoYearsAgo serviceHoursThreeYearsAgo).")]
        public float? Seniority { get; set; }
        
        /// <summary>
        /// Sum of hours in the current fiscal year&#39;s time cards captured at the time this record was created.
        /// </summary>
        /// <value>Sum of hours in the current fiscal year&#39;s time cards captured at the time this record was created.</value>
        [MetaDataExtension (Description = "Sum of hours in the current fiscal year&#39;s time cards captured at the time this record was created.")]
        public float? YTD { get; set; }
        
        /// <summary>
        /// Number of hours of service by this piece of equipment in the previous fiscal year
        /// </summary>
        /// <value>Number of hours of service by this piece of equipment in the previous fiscal year</value>
        [MetaDataExtension (Description = "Number of hours of service by this piece of equipment in the previous fiscal year")]
        public float? ServiceHoursLastYear { get; set; }
        
        /// <summary>
        /// Number of hours of service by this piece of equipment in the fiscal year before the last one - e.g. if current year is FY2018 then hours in FY2016
        /// </summary>
        /// <value>Number of hours of service by this piece of equipment in the fiscal year before the last one - e.g. if current year is FY2018 then hours in FY2016</value>
        [MetaDataExtension (Description = "Number of hours of service by this piece of equipment in the fiscal year before the last one - e.g. if current year is FY2018 then hours in FY2016")]
        public float? ServiceHoursTwoYearsAgo { get; set; }
        
        /// <summary>
        /// Number of hours of service by this piece of equipment in the fiscal year three years ago - e.g. if current year is FY2018 then hours in FY2015
        /// </summary>
        /// <value>Number of hours of service by this piece of equipment in the fiscal year three years ago - e.g. if current year is FY2018 then hours in FY2015</value>
        [MetaDataExtension (Description = "Number of hours of service by this piece of equipment in the fiscal year three years ago - e.g. if current year is FY2018 then hours in FY2015")]
        public float? ServiceHoursThreeYearsAgo { get; set; }
        
        /// <summary>
        /// TO BE REMOVED - Not sure why this would be needed
        /// </summary>
        /// <value>TO BE REMOVED - Not sure why this would be needed</value>
        [MetaDataExtension (Description = "TO BE REMOVED - Not sure why this would be needed")]
        public float? CycleHrsWrk { get; set; }
        
        /// <summary>
        /// TO BE REMOVED - not to be supported in new version of HETS
        /// </summary>
        /// <value>TO BE REMOVED - not to be supported in new version of HETS</value>
        [MetaDataExtension (Description = "TO BE REMOVED - not to be supported in new version of HETS")]
        [MaxLength(255)]
        
        public string FrozenOut { get; set; }
        
        /// <summary>
        /// TO BE REMOVED - Not sure why this would be needed
        /// </summary>
        /// <value>TO BE REMOVED - Not sure why this would be needed</value>
        [MetaDataExtension (Description = "TO BE REMOVED - Not sure why this would be needed")]
        public Project Project { get; set; }
        
        /// <summary>
        /// Foreign key for Project 
        /// </summary>       
        [ForeignKey("Project")]
        public int? ProjectRefId { get; set; }
        
        /// <summary>
        /// TO BE REMOVED - Not sure why this would be needed
        /// </summary>
        /// <value>TO BE REMOVED - Not sure why this would be needed</value>
        [MetaDataExtension (Description = "TO BE REMOVED - Not sure why this would be needed")]
        [MaxLength(255)]
        
        public string Working { get; set; }
        
        /// <summary>
        /// TO BE REMOVED - Not sure why this would be needed
        /// </summary>
        /// <value>TO BE REMOVED - Not sure why this would be needed</value>
        [MetaDataExtension (Description = "TO BE REMOVED - Not sure why this would be needed")]
        [MaxLength(255)]
        
        public string YearEndReg { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class SeniorityAudit {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  GeneratedTime: ").Append(GeneratedTime).Append("\n");
            sb.Append("  LocalArea: ").Append(LocalArea).Append("\n");
            sb.Append("  Equipment: ").Append(Equipment).Append("\n");
            sb.Append("  BlockNumber: ").Append(BlockNumber).Append("\n");
            sb.Append("  EquipCd: ").Append(EquipCd).Append("\n");
            sb.Append("  Owner: ").Append(Owner).Append("\n");
            sb.Append("  OwnerName: ").Append(OwnerName).Append("\n");
            sb.Append("  Seniority: ").Append(Seniority).Append("\n");
            sb.Append("  YTD: ").Append(YTD).Append("\n");
            sb.Append("  ServiceHoursLastYear: ").Append(ServiceHoursLastYear).Append("\n");
            sb.Append("  ServiceHoursTwoYearsAgo: ").Append(ServiceHoursTwoYearsAgo).Append("\n");
            sb.Append("  ServiceHoursThreeYearsAgo: ").Append(ServiceHoursThreeYearsAgo).Append("\n");
            sb.Append("  CycleHrsWrk: ").Append(CycleHrsWrk).Append("\n");
            sb.Append("  FrozenOut: ").Append(FrozenOut).Append("\n");
            sb.Append("  Project: ").Append(Project).Append("\n");
            sb.Append("  Working: ").Append(Working).Append("\n");
            sb.Append("  YearEndReg: ").Append(YearEndReg).Append("\n");
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
            return Equals((SeniorityAudit)obj);
        }

        /// <summary>
        /// Returns true if SeniorityAudit instances are equal
        /// </summary>
        /// <param name="other">Instance of SeniorityAudit to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(SeniorityAudit other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.GeneratedTime == other.GeneratedTime ||
                    this.GeneratedTime != null &&
                    this.GeneratedTime.Equals(other.GeneratedTime)
                ) &&                 
                (
                    this.LocalArea == other.LocalArea ||
                    this.LocalArea != null &&
                    this.LocalArea.Equals(other.LocalArea)
                ) &&                 
                (
                    this.Equipment == other.Equipment ||
                    this.Equipment != null &&
                    this.Equipment.Equals(other.Equipment)
                ) &&                 
                (
                    this.BlockNumber == other.BlockNumber ||
                    this.BlockNumber != null &&
                    this.BlockNumber.Equals(other.BlockNumber)
                ) &&                 
                (
                    this.EquipCd == other.EquipCd ||
                    this.EquipCd != null &&
                    this.EquipCd.Equals(other.EquipCd)
                ) &&                 
                (
                    this.Owner == other.Owner ||
                    this.Owner != null &&
                    this.Owner.Equals(other.Owner)
                ) &&                 
                (
                    this.OwnerName == other.OwnerName ||
                    this.OwnerName != null &&
                    this.OwnerName.Equals(other.OwnerName)
                ) &&                 
                (
                    this.Seniority == other.Seniority ||
                    this.Seniority != null &&
                    this.Seniority.Equals(other.Seniority)
                ) &&                 
                (
                    this.YTD == other.YTD ||
                    this.YTD != null &&
                    this.YTD.Equals(other.YTD)
                ) &&                 
                (
                    this.ServiceHoursLastYear == other.ServiceHoursLastYear ||
                    this.ServiceHoursLastYear != null &&
                    this.ServiceHoursLastYear.Equals(other.ServiceHoursLastYear)
                ) &&                 
                (
                    this.ServiceHoursTwoYearsAgo == other.ServiceHoursTwoYearsAgo ||
                    this.ServiceHoursTwoYearsAgo != null &&
                    this.ServiceHoursTwoYearsAgo.Equals(other.ServiceHoursTwoYearsAgo)
                ) &&                 
                (
                    this.ServiceHoursThreeYearsAgo == other.ServiceHoursThreeYearsAgo ||
                    this.ServiceHoursThreeYearsAgo != null &&
                    this.ServiceHoursThreeYearsAgo.Equals(other.ServiceHoursThreeYearsAgo)
                ) &&                 
                (
                    this.CycleHrsWrk == other.CycleHrsWrk ||
                    this.CycleHrsWrk != null &&
                    this.CycleHrsWrk.Equals(other.CycleHrsWrk)
                ) &&                 
                (
                    this.FrozenOut == other.FrozenOut ||
                    this.FrozenOut != null &&
                    this.FrozenOut.Equals(other.FrozenOut)
                ) &&                 
                (
                    this.Project == other.Project ||
                    this.Project != null &&
                    this.Project.Equals(other.Project)
                ) &&                 
                (
                    this.Working == other.Working ||
                    this.Working != null &&
                    this.Working.Equals(other.Working)
                ) &&                 
                (
                    this.YearEndReg == other.YearEndReg ||
                    this.YearEndReg != null &&
                    this.YearEndReg.Equals(other.YearEndReg)
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
                                   
                hash = hash * 59 + this.Id.GetHashCode();                if (this.GeneratedTime != null)
                {
                    hash = hash * 59 + this.GeneratedTime.GetHashCode();
                }                
                                   
                if (this.LocalArea != null)
                {
                    hash = hash * 59 + this.LocalArea.GetHashCode();
                }                   
                if (this.Equipment != null)
                {
                    hash = hash * 59 + this.Equipment.GetHashCode();
                }                if (this.BlockNumber != null)
                {
                    hash = hash * 59 + this.BlockNumber.GetHashCode();
                }                
                                if (this.EquipCd != null)
                {
                    hash = hash * 59 + this.EquipCd.GetHashCode();
                }                
                                   
                if (this.Owner != null)
                {
                    hash = hash * 59 + this.Owner.GetHashCode();
                }                if (this.OwnerName != null)
                {
                    hash = hash * 59 + this.OwnerName.GetHashCode();
                }                
                                if (this.Seniority != null)
                {
                    hash = hash * 59 + this.Seniority.GetHashCode();
                }                
                                if (this.YTD != null)
                {
                    hash = hash * 59 + this.YTD.GetHashCode();
                }                
                                if (this.ServiceHoursLastYear != null)
                {
                    hash = hash * 59 + this.ServiceHoursLastYear.GetHashCode();
                }                
                                if (this.ServiceHoursTwoYearsAgo != null)
                {
                    hash = hash * 59 + this.ServiceHoursTwoYearsAgo.GetHashCode();
                }                
                                if (this.ServiceHoursThreeYearsAgo != null)
                {
                    hash = hash * 59 + this.ServiceHoursThreeYearsAgo.GetHashCode();
                }                
                                if (this.CycleHrsWrk != null)
                {
                    hash = hash * 59 + this.CycleHrsWrk.GetHashCode();
                }                
                                if (this.FrozenOut != null)
                {
                    hash = hash * 59 + this.FrozenOut.GetHashCode();
                }                
                                   
                if (this.Project != null)
                {
                    hash = hash * 59 + this.Project.GetHashCode();
                }                if (this.Working != null)
                {
                    hash = hash * 59 + this.Working.GetHashCode();
                }                
                                if (this.YearEndReg != null)
                {
                    hash = hash * 59 + this.YearEndReg.GetHashCode();
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
        public static bool operator ==(SeniorityAudit left, SeniorityAudit right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(SeniorityAudit left, SeniorityAudit right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
