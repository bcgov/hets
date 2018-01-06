using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// Seniority Audit Database Model
    /// </summary>
    [MetaDataExtension (Description = "The history of all changes to the seniority of a piece of equipment. The current seniority information (underlying data elements and the calculation result) is in the equipment record. Every time that information changes, the old values are copied to here, with a start date, end date range. In the normal case, an annual update triggers the old values being copied here and the new values put into the equipment record. If a user manually changes the values, the existing values are copied into a record added here.")]
    public sealed class SeniorityAudit : AuditableEntity, IEquatable<SeniorityAudit>
    {
        /// <summary>
        /// Seniority Audit Database Model Constructor (required by entity framework)
        /// </summary>
        public SeniorityAudit()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeniorityAudit" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a SeniorityAudit (required).</param>
        /// <param name="startDate">The effective date that the Seniority data in this record went into effect. (required).</param>
        /// <param name="endDate">The effective date at which the Seniority data in this record ceased to be in effect. (required).</param>
        /// <param name="localArea">A foreign key reference to the system-generated unique identifier for a Local Area (required).</param>
        /// <param name="equipment">A foreign key reference to the system-generated unique identifier for an Equipment (required).</param>
        /// <param name="blockNumber">The block number for the piece of equipment as calculated by the Seniority Algorthm for this equipment type in the local area. As currently defined by the business - 1, 2 or Open.</param>
        /// <param name="owner">A foreign key reference to the system-generated unique identifier for an Owner.</param>
        /// <param name="ownerOrganizationName">The name of the organization of the owner from the Owner Record, captured at the time this record was created..</param>
        /// <param name="seniority">The seniority calculation result for this piece of equipment. The calculation is based on the &amp;quot;numYears&amp;quot; of service + average hours of service over the last three fiscal years - as stored in the related fields (serviceHoursLastYear, serviceHoursTwoYearsAgo serviceHoursThreeYearsAgo)..</param>
        /// <param name="serviceHoursLastYear">Number of hours of service by this piece of equipment in the previous fiscal year.</param>
        /// <param name="serviceHoursTwoYearsAgo">Number of hours of service by this piece of equipment in the fiscal year before the last one - e.g. if current year is FY2018 then hours in FY2016.</param>
        /// <param name="serviceHoursThreeYearsAgo">Number of hours of service by this piece of equipment in the fiscal year three years ago - e.g. if current year is FY2018 then hours in FY2015.</param>
        /// <param name="isSeniorityOverridden">True if the Seniority for the piece of equipment was manually overridden. Set if a user has gone in and explicitly updated the seniority base information. Indicates that underlying numbers were manually overridden..</param>
        /// <param name="seniorityOverrideReason">A text reason for why the piece of equipments underlying data was overridden to change their seniority number..</param>
        public SeniorityAudit(int id, DateTime startDate, DateTime endDate, LocalArea localArea, Equipment equipment, 
            int? blockNumber = null, Owner owner = null, string ownerOrganizationName = null, float? seniority = null, 
            float? serviceHoursLastYear = null, float? serviceHoursTwoYearsAgo = null, float? serviceHoursThreeYearsAgo = null, 
            bool? isSeniorityOverridden = null, string seniorityOverrideReason = null)
        {   
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            LocalArea = localArea;
            Equipment = equipment;
            BlockNumber = blockNumber;
            Owner = owner;
            OwnerOrganizationName = ownerOrganizationName;
            Seniority = seniority;
            ServiceHoursLastYear = serviceHoursLastYear;
            ServiceHoursTwoYearsAgo = serviceHoursTwoYearsAgo;
            ServiceHoursThreeYearsAgo = serviceHoursThreeYearsAgo;
            IsSeniorityOverridden = isSeniorityOverridden;
            SeniorityOverrideReason = seniorityOverrideReason;
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
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// The effective date at which the Seniority data in this record ceased to be in effect.
        /// </summary>
        /// <value>The effective date at which the Seniority data in this record ceased to be in effect.</value>
        [MetaDataExtension (Description = "The effective date at which the Seniority data in this record ceased to be in effect.")]
        public DateTime EndDate { get; set; }
        
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
        public int? BlockNumber { get; set; }
        
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
        /// True if the Seniority for the piece of equipment was manually overridden. Set if a user has gone in and explicitly updated the seniority base information. Indicates that underlying numbers were manually overridden.
        /// </summary>
        /// <value>True if the Seniority for the piece of equipment was manually overridden. Set if a user has gone in and explicitly updated the seniority base information. Indicates that underlying numbers were manually overridden.</value>
        [MetaDataExtension (Description = "True if the Seniority for the piece of equipment was manually overridden. Set if a user has gone in and explicitly updated the seniority base information. Indicates that underlying numbers were manually overridden.")]
        public bool? IsSeniorityOverridden { get; set; }
        
        /// <summary>
        /// A text reason for why the piece of equipments underlying data was overridden to change their seniority number.
        /// </summary>
        /// <value>A text reason for why the piece of equipments underlying data was overridden to change their seniority number.</value>
        [MetaDataExtension (Description = "A text reason for why the piece of equipments underlying data was overridden to change their seniority number.")]
        [MaxLength(2048)]        
        public string SeniorityOverrideReason { get; set; }
        
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
            sb.Append("  ServiceHoursLastYear: ").Append(ServiceHoursLastYear).Append("\n");
            sb.Append("  ServiceHoursTwoYearsAgo: ").Append(ServiceHoursTwoYearsAgo).Append("\n");
            sb.Append("  ServiceHoursThreeYearsAgo: ").Append(ServiceHoursThreeYearsAgo).Append("\n");
            sb.Append("  IsSeniorityOverridden: ").Append(IsSeniorityOverridden).Append("\n");
            sb.Append("  SeniorityOverrideReason: ").Append(SeniorityOverrideReason).Append("\n");
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
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    StartDate == other.StartDate ||
                    StartDate.Equals(other.StartDate)
                ) &&                 
                (
                    EndDate == other.EndDate ||
                    EndDate.Equals(other.EndDate)
                ) &&                 
                (
                    LocalArea == other.LocalArea ||
                    LocalArea != null &&
                    LocalArea.Equals(other.LocalArea)
                ) &&                 
                (
                    Equipment == other.Equipment ||
                    Equipment != null &&
                    Equipment.Equals(other.Equipment)
                ) &&                 
                (
                    BlockNumber == other.BlockNumber ||
                    BlockNumber != null &&
                    BlockNumber.Equals(other.BlockNumber)
                ) &&                 
                (
                    Owner == other.Owner ||
                    Owner != null &&
                    Owner.Equals(other.Owner)
                ) &&                 
                (
                    OwnerOrganizationName == other.OwnerOrganizationName ||
                    OwnerOrganizationName != null &&
                    OwnerOrganizationName.Equals(other.OwnerOrganizationName)
                ) &&                 
                (
                    Seniority == other.Seniority ||
                    Seniority != null &&
                    Seniority.Equals(other.Seniority)
                ) &&                 
                (
                    ServiceHoursLastYear == other.ServiceHoursLastYear ||
                    ServiceHoursLastYear != null &&
                    ServiceHoursLastYear.Equals(other.ServiceHoursLastYear)
                ) &&                 
                (
                    ServiceHoursTwoYearsAgo == other.ServiceHoursTwoYearsAgo ||
                    ServiceHoursTwoYearsAgo != null &&
                    ServiceHoursTwoYearsAgo.Equals(other.ServiceHoursTwoYearsAgo)
                ) &&                 
                (
                    ServiceHoursThreeYearsAgo == other.ServiceHoursThreeYearsAgo ||
                    ServiceHoursThreeYearsAgo != null &&
                    ServiceHoursThreeYearsAgo.Equals(other.ServiceHoursThreeYearsAgo)
                ) &&                 
                (
                    IsSeniorityOverridden == other.IsSeniorityOverridden ||
                    IsSeniorityOverridden != null &&
                    IsSeniorityOverridden.Equals(other.IsSeniorityOverridden)
                ) &&                 
                (
                    SeniorityOverrideReason == other.SeniorityOverrideReason ||
                    SeniorityOverrideReason != null &&
                    SeniorityOverrideReason.Equals(other.SeniorityOverrideReason)
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
                hash = hash * 59 + StartDate.GetHashCode();
                hash = hash * 59 + EndDate.GetHashCode();
                                   
                if (LocalArea != null)
                {
                    hash = hash * 59 + LocalArea.GetHashCode();
                }

                if (Equipment != null)
                {
                    hash = hash * 59 + Equipment.GetHashCode();
                }

                if (BlockNumber != null)
                {
                    hash = hash * 59 + BlockNumber.GetHashCode();
                }                
                                   
                if (Owner != null)
                {
                    hash = hash * 59 + Owner.GetHashCode();
                }

                if (OwnerOrganizationName != null)
                {
                    hash = hash * 59 + OwnerOrganizationName.GetHashCode();
                }

                if (Seniority != null)
                {
                    hash = hash * 59 + Seniority.GetHashCode();
                }

                if (ServiceHoursLastYear != null)
                {
                    hash = hash * 59 + ServiceHoursLastYear.GetHashCode();
                }

                if (ServiceHoursTwoYearsAgo != null)
                {
                    hash = hash * 59 + ServiceHoursTwoYearsAgo.GetHashCode();
                }

                if (ServiceHoursThreeYearsAgo != null)
                {
                    hash = hash * 59 + ServiceHoursThreeYearsAgo.GetHashCode();
                }

                if (IsSeniorityOverridden != null)
                {
                    hash = hash * 59 + IsSeniorityOverridden.GetHashCode();
                }

                if (SeniorityOverrideReason != null)
                {
                    hash = hash * 59 + SeniorityOverrideReason.GetHashCode();
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
