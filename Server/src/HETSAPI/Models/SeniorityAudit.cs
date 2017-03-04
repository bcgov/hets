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
    /// The history of all changes to the seniority of a piece of equipment. The current seniority information (underlying data elements and the calculation result) is in the equipment record. Every time that information changes, the old values are copied to here, with a start date, end date range. In the normal case, an annual update triggers the old values being copied here and the new values put into the equipment record. If a user manually changes the values, the existing values are copied into a record added here.
    /// </summary>
        [MetaDataExtension (Description = "The history of all changes to the seniority of a piece of equipment. The current seniority information (underlying data elements and the calculation result) is in the equipment record. Every time that information changes, the old values are copied to here, with a start date, end date range. In the normal case, an annual update triggers the old values being copied here and the new values put into the equipment record. If a user manually changes the values, the existing values are copied into a record added here.")]

    public partial class SeniorityAudit : AuditableEntity, IEquatable<SeniorityAudit>
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
        /// <param name="StartDate">The effective date that the Seniority data in this record went into effect..</param>
        /// <param name="EndDate">The effective date at which the Seniority data in this record ceased to be in effect..</param>
        /// <param name="LocalArea">A foreign key reference to the system-generated unique identifier for a Local Area.</param>
        /// <param name="Equipment">A foreign key reference to the system-generated unique identifier for an Equipment.</param>
        /// <param name="BlockNumber">The block number for the piece of equipment as calculated by the Seniority Algorthm for this equipment type in the local area. As currently defined by the business - 1, 2 or Open.</param>
        /// <param name="Owner">A foreign key reference to the system-generated unique identifier for an Owner.</param>
        /// <param name="OwnerOrganizationName">The name of the organization of the owner from the Owner Record, captured at the time this record was created..</param>
        /// <param name="Seniority">The seniority calculation result for this piece of equipment. The calculation is based on the &amp;quot;numYears&amp;quot; of service + average hours of service over the last three fiscal years - as stored in the related fields (serviceHoursLastYear, serviceHoursTwoYearsAgo serviceHoursThreeYearsAgo)..</param>
        /// <param name="ServiceHoursCurrentYearToDate">Sum of hours in the current fiscal year&amp;#39;s time cards captured at the time this record was created..</param>
        /// <param name="ServiceHoursLastYear">Number of hours of service by this piece of equipment in the previous fiscal year.</param>
        /// <param name="ServiceHoursTwoYearsAgo">Number of hours of service by this piece of equipment in the fiscal year before the last one - e.g. if current year is FY2018 then hours in FY2016.</param>
        /// <param name="ServiceHoursThreeYearsAgo">Number of hours of service by this piece of equipment in the fiscal year three years ago - e.g. if current year is FY2018 then hours in FY2015.</param>
        public SeniorityAudit(int Id, DateTime? StartDate = null, DateTime? EndDate = null, LocalArea LocalArea = null, Equipment Equipment = null, float? BlockNumber = null, Owner Owner = null, string OwnerOrganizationName = null, float? Seniority = null, float? ServiceHoursCurrentYearToDate = null, float? ServiceHoursLastYear = null, float? ServiceHoursTwoYearsAgo = null, float? ServiceHoursThreeYearsAgo = null)
        {   
            this.Id = Id;
            this.StartDate = StartDate;
            this.EndDate = EndDate;
            this.LocalArea = LocalArea;
            this.Equipment = Equipment;
            this.BlockNumber = BlockNumber;
            this.Owner = Owner;
            this.OwnerOrganizationName = OwnerOrganizationName;
            this.Seniority = Seniority;
            this.ServiceHoursCurrentYearToDate = ServiceHoursCurrentYearToDate;
            this.ServiceHoursLastYear = ServiceHoursLastYear;
            this.ServiceHoursTwoYearsAgo = ServiceHoursTwoYearsAgo;
            this.ServiceHoursThreeYearsAgo = ServiceHoursThreeYearsAgo;
        }

        /// <summary>
        /// A system-generated unique identifier for a SeniorityAudit
        /// </summary>
        /// <value>A system-generated unique identifier for a SeniorityAudit</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a SeniorityAudit")]
        public int Id { get; set; }
        
        /// <summary>
        /// The effective date that the Seniority data in this record went into effect.
        /// </summary>
        /// <value>The effective date that the Seniority data in this record went into effect.</value>
        [MetaDataExtension (Description = "The effective date that the Seniority data in this record went into effect.")]
        public DateTime? StartDate { get; set; }
        
        /// <summary>
        /// The effective date at which the Seniority data in this record ceased to be in effect.
        /// </summary>
        /// <value>The effective date at which the Seniority data in this record ceased to be in effect.</value>
        [MetaDataExtension (Description = "The effective date at which the Seniority data in this record ceased to be in effect.")]
        public DateTime? EndDate { get; set; }
        
        /// <summary>
        /// A foreign key reference to the system-generated unique identifier for a Local Area
        /// </summary>
        /// <value>A foreign key reference to the system-generated unique identifier for a Local Area</value>
        [MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for a Local Area")]
        public LocalArea LocalArea { get; set; }
        
        /// <summary>
        /// Foreign key for LocalArea 
        /// </summary>   
        [ForeignKey("LocalArea")]
		[JsonIgnore]
		[MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for a Local Area")]
        public int? LocalAreaId { get; set; }
        
        /// <summary>
        /// A foreign key reference to the system-generated unique identifier for an Equipment
        /// </summary>
        /// <value>A foreign key reference to the system-generated unique identifier for an Equipment</value>
        [MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for an Equipment")]
        public Equipment Equipment { get; set; }
        
        /// <summary>
        /// Foreign key for Equipment 
        /// </summary>   
        [ForeignKey("Equipment")]
		[JsonIgnore]
		[MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for an Equipment")]
        public int? EquipmentId { get; set; }
        
        /// <summary>
        /// The block number for the piece of equipment as calculated by the Seniority Algorthm for this equipment type in the local area. As currently defined by the business - 1, 2 or Open
        /// </summary>
        /// <value>The block number for the piece of equipment as calculated by the Seniority Algorthm for this equipment type in the local area. As currently defined by the business - 1, 2 or Open</value>
        [MetaDataExtension (Description = "The block number for the piece of equipment as calculated by the Seniority Algorthm for this equipment type in the local area. As currently defined by the business - 1, 2 or Open")]
        public float? BlockNumber { get; set; }
        
        /// <summary>
        /// A foreign key reference to the system-generated unique identifier for an Owner
        /// </summary>
        /// <value>A foreign key reference to the system-generated unique identifier for an Owner</value>
        [MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for an Owner")]
        public Owner Owner { get; set; }
        
        /// <summary>
        /// Foreign key for Owner 
        /// </summary>   
        [ForeignKey("Owner")]
		[JsonIgnore]
		[MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for an Owner")]
        public int? OwnerId { get; set; }
        
        /// <summary>
        /// The name of the organization of the owner from the Owner Record, captured at the time this record was created.
        /// </summary>
        /// <value>The name of the organization of the owner from the Owner Record, captured at the time this record was created.</value>
        [MetaDataExtension (Description = "The name of the organization of the owner from the Owner Record, captured at the time this record was created.")]
        [MaxLength(150)]
        
        public string OwnerOrganizationName { get; set; }
        
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
        public float? ServiceHoursCurrentYearToDate { get; set; }
        
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
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class SeniorityAudit {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  StartDate: ").Append(StartDate).Append("\n");
            sb.Append("  EndDate: ").Append(EndDate).Append("\n");
            sb.Append("  LocalArea: ").Append(LocalArea).Append("\n");
            sb.Append("  Equipment: ").Append(Equipment).Append("\n");
            sb.Append("  BlockNumber: ").Append(BlockNumber).Append("\n");
            sb.Append("  Owner: ").Append(Owner).Append("\n");
            sb.Append("  OwnerOrganizationName: ").Append(OwnerOrganizationName).Append("\n");
            sb.Append("  Seniority: ").Append(Seniority).Append("\n");
            sb.Append("  ServiceHoursCurrentYearToDate: ").Append(ServiceHoursCurrentYearToDate).Append("\n");
            sb.Append("  ServiceHoursLastYear: ").Append(ServiceHoursLastYear).Append("\n");
            sb.Append("  ServiceHoursTwoYearsAgo: ").Append(ServiceHoursTwoYearsAgo).Append("\n");
            sb.Append("  ServiceHoursThreeYearsAgo: ").Append(ServiceHoursThreeYearsAgo).Append("\n");
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
                    this.StartDate == other.StartDate ||
                    this.StartDate != null &&
                    this.StartDate.Equals(other.StartDate)
                ) &&                 
                (
                    this.EndDate == other.EndDate ||
                    this.EndDate != null &&
                    this.EndDate.Equals(other.EndDate)
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
                    this.Owner == other.Owner ||
                    this.Owner != null &&
                    this.Owner.Equals(other.Owner)
                ) &&                 
                (
                    this.OwnerOrganizationName == other.OwnerOrganizationName ||
                    this.OwnerOrganizationName != null &&
                    this.OwnerOrganizationName.Equals(other.OwnerOrganizationName)
                ) &&                 
                (
                    this.Seniority == other.Seniority ||
                    this.Seniority != null &&
                    this.Seniority.Equals(other.Seniority)
                ) &&                 
                (
                    this.ServiceHoursCurrentYearToDate == other.ServiceHoursCurrentYearToDate ||
                    this.ServiceHoursCurrentYearToDate != null &&
                    this.ServiceHoursCurrentYearToDate.Equals(other.ServiceHoursCurrentYearToDate)
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
                                   
                hash = hash * 59 + this.Id.GetHashCode();                if (this.StartDate != null)
                {
                    hash = hash * 59 + this.StartDate.GetHashCode();
                }                
                                if (this.EndDate != null)
                {
                    hash = hash * 59 + this.EndDate.GetHashCode();
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
                                   
                if (this.Owner != null)
                {
                    hash = hash * 59 + this.Owner.GetHashCode();
                }                if (this.OwnerOrganizationName != null)
                {
                    hash = hash * 59 + this.OwnerOrganizationName.GetHashCode();
                }                
                                if (this.Seniority != null)
                {
                    hash = hash * 59 + this.Seniority.GetHashCode();
                }                
                                if (this.ServiceHoursCurrentYearToDate != null)
                {
                    hash = hash * 59 + this.ServiceHoursCurrentYearToDate.GetHashCode();
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
