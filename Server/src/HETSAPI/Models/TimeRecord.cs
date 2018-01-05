using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// Time Record Database Model
    /// </summary>
    [MetaDataExtension (Description = "A record of time worked for a piece of equipment hired for a specific project within a Local Area.")]
    public class TimeRecord : AuditableEntity, IEquatable<TimeRecord>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public TimeRecord()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeRecord" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a TimeRecord (required).</param>
        /// <param name="rentalAgreement">A foreign key reference to the system-generated unique identifier for a Rental Agreement (required).</param>
        /// <param name="workedDate">The date of the time record entry - the day of the entry if it is a daily entry, or a date in the week in which the work occurred if tracked weekly. (required).</param>
        /// <param name="hours">The number of hours worked by the equipment. (required).</param>
        /// <param name="rentalAgreementRate">The Rental Agreement Rate component to which this Rental Agreement applies. If null, this time applies to the equipment itself..</param>
        /// <param name="enteredDate">The date-time the time record information was entered..</param>
        /// <param name="timePeriod">The time period of the entry - either day or week. HETS Clerk have the option of entering time records on a day-by-day or week-by-week basis..</param>
        public TimeRecord(int id, RentalAgreement rentalAgreement, DateTime workedDate, float? hours, 
            RentalAgreementRate rentalAgreementRate = null, DateTime? enteredDate = null, string timePeriod = null)
        {   
            Id = id;
            RentalAgreement = rentalAgreement;
            WorkedDate = workedDate;
            Hours = hours;
            RentalAgreementRate = rentalAgreementRate;
            EnteredDate = enteredDate;
            TimePeriod = timePeriod;
        }

        /// <summary>
        /// A system-generated unique identifier for a TimeRecord
        /// </summary>
        /// <value>A system-generated unique identifier for a TimeRecord</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a TimeRecord")]
        public int Id { get; set; }
        
        /// <summary>
        /// A foreign key reference to the system-generated unique identifier for a Rental Agreement
        /// </summary>
        /// <value>A foreign key reference to the system-generated unique identifier for a Rental Agreement</value>
        [MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for a Rental Agreement")]
        public RentalAgreement RentalAgreement { get; set; }
        
        /// <summary>
        /// Foreign key for RentalAgreement 
        /// </summary>   
        [ForeignKey("RentalAgreement")]
		[JsonIgnore]
		[MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for a Rental Agreement")]
        public int? RentalAgreementId { get; set; }
        
        /// <summary>
        /// The date of the time record entry - the day of the entry if it is a daily entry, or a date in the week in which the work occurred if tracked weekly.
        /// </summary>
        /// <value>The date of the time record entry - the day of the entry if it is a daily entry, or a date in the week in which the work occurred if tracked weekly.</value>
        [MetaDataExtension (Description = "The date of the time record entry - the day of the entry if it is a daily entry, or a date in the week in which the work occurred if tracked weekly.")]
        public DateTime WorkedDate { get; set; }
        
        /// <summary>
        /// The number of hours worked by the equipment.
        /// </summary>
        /// <value>The number of hours worked by the equipment.</value>
        [MetaDataExtension (Description = "The number of hours worked by the equipment.")]
        public float? Hours { get; set; }
        
        /// <summary>
        /// The Rental Agreement Rate component to which this Rental Agreement applies. If null, this time applies to the equipment itself.
        /// </summary>
        /// <value>The Rental Agreement Rate component to which this Rental Agreement applies. If null, this time applies to the equipment itself.</value>
        [MetaDataExtension (Description = "The Rental Agreement Rate component to which this Rental Agreement applies. If null, this time applies to the equipment itself.")]
        public RentalAgreementRate RentalAgreementRate { get; set; }
        
        /// <summary>
        /// Foreign key for RentalAgreementRate 
        /// </summary>   
        [ForeignKey("RentalAgreementRate")]
		[JsonIgnore]
		[MetaDataExtension (Description = "The Rental Agreement Rate component to which this Rental Agreement applies. If null, this time applies to the equipment itself.")]
        public int? RentalAgreementRateId { get; set; }
        
        /// <summary>
        /// The date-time the time record information was entered.
        /// </summary>
        /// <value>The date-time the time record information was entered.</value>
        [MetaDataExtension (Description = "The date-time the time record information was entered.")]
        public DateTime? EnteredDate { get; set; }
        
        /// <summary>
        /// The time period of the entry - either day or week. HETS Clerk have the option of entering time records on a day-by-day or week-by-week basis.
        /// </summary>
        /// <value>The time period of the entry - either day or week. HETS Clerk have the option of entering time records on a day-by-day or week-by-week basis.</value>
        [MetaDataExtension (Description = "The time period of the entry - either day or week. HETS Clerk have the option of entering time records on a day-by-day or week-by-week basis.")]
        [MaxLength(20)]        
        public string TimePeriod { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class TimeRecord {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  RentalAgreement: ").Append(RentalAgreement).Append("\n");
            sb.Append("  WorkedDate: ").Append(WorkedDate).Append("\n");
            sb.Append("  Hours: ").Append(Hours).Append("\n");
            sb.Append("  RentalAgreementRate: ").Append(RentalAgreementRate).Append("\n");
            sb.Append("  EnteredDate: ").Append(EnteredDate).Append("\n");
            sb.Append("  TimePeriod: ").Append(TimePeriod).Append("\n");
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

            return Equals((TimeRecord)obj);
        }

        /// <summary>
        /// Returns true if TimeRecord instances are equal
        /// </summary>
        /// <param name="other">Instance of TimeRecord to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(TimeRecord other)
        {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    RentalAgreement == other.RentalAgreement ||
                    RentalAgreement != null &&
                    RentalAgreement.Equals(other.RentalAgreement)
                ) &&                 
                (
                    WorkedDate == other.WorkedDate ||
                    WorkedDate.Equals(other.WorkedDate)
                ) &&                 
                (
                    Hours == other.Hours ||
                    Hours != null &&
                    Hours.Equals(other.Hours)
                ) &&                 
                (
                    RentalAgreementRate == other.RentalAgreementRate ||
                    RentalAgreementRate != null &&
                    RentalAgreementRate.Equals(other.RentalAgreementRate)
                ) &&                 
                (
                    EnteredDate == other.EnteredDate ||
                    EnteredDate != null &&
                    EnteredDate.Equals(other.EnteredDate)
                ) &&                 
                (
                    TimePeriod == other.TimePeriod ||
                    TimePeriod != null &&
                    TimePeriod.Equals(other.TimePeriod)
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

                if (RentalAgreement != null)
                {
                    hash = hash * 59 + RentalAgreement.GetHashCode();
                }

                hash = hash * 59 + WorkedDate.GetHashCode();

                if (Hours != null)
                {
                    hash = hash * 59 + Hours.GetHashCode();
                }

                if (RentalAgreementRate != null)
                {
                    hash = hash * 59 + RentalAgreementRate.GetHashCode();
                }

                if (EnteredDate != null)
                {
                    hash = hash * 59 + EnteredDate.GetHashCode();
                }

                if (TimePeriod != null)
                {
                    hash = hash * 59 + TimePeriod.GetHashCode();
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
        public static bool operator ==(TimeRecord left, TimeRecord right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(TimeRecord left, TimeRecord right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
