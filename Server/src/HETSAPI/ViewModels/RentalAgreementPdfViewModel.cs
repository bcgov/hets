using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using HETSAPI.Models;

namespace HETSAPI.ViewModels
{
    /// <summary>
    /// Rental Agreement Pdf View Model
    /// </summary>
    [DataContract]
    public sealed class RentalAgreementPdfViewModel : IEquatable<RentalAgreementPdfViewModel>
    {
        /// <summary>
        /// /// Rental Agreement Pdf View Model Constructor
        /// </summary>
        public RentalAgreementPdfViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalAgreementPdfViewModel" /> class.
        /// </summary>
        /// <param name="id">Id (required).</param>
        /// <param name="number">A system-generated unique rental agreement number in a format defined by the business as suitable for the business and client to see and use.</param>
        /// <param name="status">The current status of the Rental Agreement, such as Active or Complete.</param>
        /// <param name="equipment">A foreign key reference to the system-generated unique identifier for an Equipment.</param>
        /// <param name="project">A foreign key reference to the system-generated unique identifier for a Project.</param>
        /// <param name="rentalAgreementRates">RentalAgreementRates.</param>
        /// <param name="rentalAgreementConditions">RentalAgreementConditions.</param>
        /// <param name="note">An optional note to be placed onto the Rental Agreement.</param>
        /// <param name="estimateStartWork">The estimated start date of the work to be placed on the rental agreement.</param>
        /// <param name="datedOn">The dated on date to put on the Rental Agreement.</param>
        /// <param name="estimateHours">The estimated number of hours of work to be put onto the Rental Agreement.</param>
        /// <param name="equipmentRate">The dollar rate for the piece of equipment itself for this Rental Agreement. Other rates associated with the Rental Agreement are in the Rental Agreement Rate table.</param>
        /// <param name="ratePeriod">The period of the rental rate. The vast majority will be hourly, but the rate could apply across a different period, e.g. daily.</param>
        /// <param name="rateComment">A comment about the rate for the piece of equipment.</param>
        /// <param name="conditionsPresent">Are there any conditions in this agreement</param>
        public RentalAgreementPdfViewModel(int id, string number = null, string status = null, Equipment equipment = null, 
            Project project = null, List<RentalAgreementRate> rentalAgreementRates = null, 
            List<RentalAgreementCondition> rentalAgreementConditions = null,  
            string note = null, string estimateStartWork = null, string datedOn = null, int? estimateHours = null, 
            float? equipmentRate = null, string ratePeriod = null, string rateComment = null, bool conditionsPresent = false)
        {   
            Id = id;
            Number = number;
            Status = status;
            Equipment = equipment;
            Project = project;
            RentalAgreementRates = rentalAgreementRates;
            RentalAgreementConditions = rentalAgreementConditions;
            Note = note;
            EstimateStartWork = estimateStartWork;
            DatedOn = datedOn;
            EstimateHours = estimateHours;
            EquipmentRate = equipmentRate;
            RatePeriod = ratePeriod;
            RateComment = rateComment;
            ConditionsPresent = conditionsPresent;
        }

        /// <summary>
        /// Look for "Other" and replace with Comment text
        /// </summary>
        public void FixOther()
        {
            foreach (RentalAgreementRate rentalRate in RentalAgreementRatesWithTotal)
            {
                if (rentalRate.ComponentName.Equals("Other", StringComparison.InvariantCultureIgnoreCase))
                {
                    rentalRate.ComponentName = rentalRate.Comment;
                    rentalRate.Comment = "";
                }
            }            

            foreach (RentalAgreementRate rentalRate in RentalAgreementRatesWithoutTotal)
            {
                if (rentalRate.ComponentName.Equals("Other", StringComparison.InvariantCultureIgnoreCase))
                {
                    rentalRate.ComponentName = rentalRate.Comment;
                    rentalRate.Comment = "";
                }
            }

            foreach (RentalAgreementCondition condition in RentalAgreementConditions)
            {
                if (condition.ConditionName.Equals("Other", StringComparison.InvariantCultureIgnoreCase))
                {
                    condition.ConditionName = condition.Comment;
                    condition.Comment = "";
                }
            }
        }

        /// <summary>
        /// Uses the rates data to calculate the totals and setup the required data for printing
        /// </summary>
        public void CalculateTotals()
        {
            // **********************************************
            // setup the rates lists -> 
            // 1. overtime records
            // 2. records in the total and 
            // 3. records not included
            // **********************************************
            RentalAgreementRatesOvertime = RentalAgreementRates
                .FindAll(x => x.ComponentName.StartsWith("Overtime", true, CultureInfo.InvariantCulture))
                .OrderBy(x => x.Id).ToList();

            RentalAgreementRatesWithTotal = RentalAgreementRates
                .FindAll(x => x.IsIncludedInTotal &&
                              !x.ComponentName.StartsWith("Overtime", true, CultureInfo.InvariantCulture))
                .OrderBy(x => x.Id).ToList(); 

            RentalAgreementRatesWithoutTotal = RentalAgreementRates
                .FindAll(x => !x.IsIncludedInTotal &&
                              !x.ComponentName.StartsWith("Overtime", true, CultureInfo.InvariantCulture))
                .OrderBy(x => x.Id).ToList();

            // **********************************************
            // calculate the total
            // **********************************************
            float temp = 0.0F;

            foreach (RentalAgreementRate rentalRate in RentalAgreementRatesWithTotal)
            {
                if (rentalRate.PercentOfEquipmentRate != null &&
                    EquipmentRate != null &&
                    rentalRate.PercentOfEquipmentRate > 0)
                {
                    rentalRate.Rate = (float)rentalRate.PercentOfEquipmentRate * ((float)EquipmentRate / 100);
                    temp = temp + (float)rentalRate.Rate;
                }
                else if (rentalRate.Rate != null)
                {
                    temp = temp + (float)rentalRate.Rate;
                }

                // format the rate / percent at the same time
                rentalRate.RateString = FormatRateString(rentalRate);
            }

            // add the base amount to the total too
            if (EquipmentRate != null)
            {
                temp = temp + (float)EquipmentRate;
            }

            AgreementTotal = temp;

            // format the base rate
            BaseRateString = string.Format("$ {0:0.00} / {1}", EquipmentRate, FormatRatePeriod(RatePeriod));

            // format the total
            AgreementTotalString = string.Format("$ {0:0.00} / {1}", AgreementTotal, FormatRatePeriod(RatePeriod));

            // **********************************************
            // format the rate / percent values
            // **********************************************
            foreach (RentalAgreementRate rentalRate in RentalAgreementRatesOvertime)
            {
                if (rentalRate.PercentOfEquipmentRate != null &&
                    EquipmentRate != null &&
                    rentalRate.PercentOfEquipmentRate > 0)
                {
                    rentalRate.Rate = (float)rentalRate.PercentOfEquipmentRate * ((float)EquipmentRate / 100);
                }

                rentalRate.RateString = FormatRateString(rentalRate);
            }

            foreach (RentalAgreementRate rentalRate in RentalAgreementRatesWithoutTotal)
            {
                if (rentalRate.PercentOfEquipmentRate != null &&
                    EquipmentRate != null &&
                    rentalRate.PercentOfEquipmentRate > 0)
                {
                    rentalRate.Rate = (float)rentalRate.PercentOfEquipmentRate * ((float)EquipmentRate / 100);
                }

                rentalRate.RateString = FormatRateString(rentalRate);                
            }
        }

        private static string FormatRatePeriod(string period)
        {
            if (!string.IsNullOrEmpty(period))
            {
                switch (period.ToLower())
                {
                    case "daily":
                    case "dy":
                        return "Dy";

                    case "hourly":
                    case "hr":
                        return "Hr";
                }
            }
            else
            {
                period = "";
            }

            return period;
        }

        private static string FormatRateString(RentalAgreementRate rentalRate)
        {
            string temp = "";

            // format the rate
            if (rentalRate.Rate != null)
            {
                temp = string.Format("$ {0:0.00} / {1}", rentalRate.Rate, FormatRatePeriod(rentalRate.RatePeriod));
            }

            // format the percent
            if (rentalRate.PercentOfEquipmentRate != null &&
                rentalRate.PercentOfEquipmentRate > 0)
            {
                temp = string.Format("({0:0.00}%) ", rentalRate.PercentOfEquipmentRate) + temp;
            }            

            return temp;
        }        

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id")]
        public int Id { get; set; }

        /// <summary>
        /// A system-generated unique rental agreement number in a format defined by the business as suitable for the business and client to see and use.
        /// </summary>
        /// <value>A system-generated unique rental agreement number in a format defined by the business as suitable for the business and client to see and use.</value>
        [DataMember(Name="number")]
        [MetaData (Description = "A system-generated unique rental agreement number in a format defined by the business as suitable for the business and client to see and use.")]
        public string Number { get; set; }

        /// <summary>
        /// The current status of the Rental Agreement, such as Active or Complete
        /// </summary>
        /// <value>The current status of the Rental Agreement, such as Active or Complete</value>
        [DataMember(Name="status")]
        [MetaData (Description = "The current status of the Rental Agreement, such as Active or Complete")]
        public string Status { get; set; }

        /// <summary>
        /// A foreign key reference to the system-generated unique identifier for an Equipment
        /// </summary>
        /// <value>A foreign key reference to the system-generated unique identifier for an Equipment</value>
        [DataMember(Name="equipment")]
        [MetaData (Description = "A foreign key reference to the system-generated unique identifier for an Equipment")]
        public Equipment Equipment { get; set; }

        /// <summary>
        /// A foreign key reference to the system-generated unique identifier for a Project
        /// </summary>
        /// <value>A foreign key reference to the system-generated unique identifier for a Project</value>
        [DataMember(Name="project")]
        [MetaData (Description = "A foreign key reference to the system-generated unique identifier for a Project")]
        public Project Project { get; set; }

        /// <summary>
        /// Gets or Sets RentalAgreementRates
        /// </summary>
        [DataMember(Name="rentalAgreementRates")]
        public List<RentalAgreementRate> RentalAgreementRates { get; set; }

        /// <summary>
        /// Gets or Sets RentalAgreementRates -> that are included in the total
        /// </summary>
        [DataMember(Name = "rentalAgreementRatesWithTotal")]
        public List<RentalAgreementRate> RentalAgreementRatesWithTotal { get; set; }

        /// <summary>
        /// Gets or Sets RentalAgreementRates -> that are included in the total
        /// </summary>
        [DataMember(Name = "rentalAgreementRatesOvertime")]
        public List<RentalAgreementRate> RentalAgreementRatesOvertime { get; set; }

        /// <summary>
        /// The dollar total -> for all included rental agreement rate records
        /// </summary>
        [DataMember(Name = "agreementTotal")]
        public float? AgreementTotal { get; set; }

        /// <summary>
        /// Used for the Pdf only (formatted version of the agreement total ($))
        /// </summary>
        [DataMember(Name = "agreementTotalString")]
        public string AgreementTotalString { get; set; }

        /// <summary>
        /// Used for the Pdf only (formatted version of the base rate ($))
        /// </summary>
        [DataMember(Name = "baseRateString")]
        public string BaseRateString { get; set; }

        /// <summary>
        /// Gets or Sets RentalAgreementRates -> that aren't included in the total
        /// </summary>
        [DataMember(Name = "rentalAgreementRatesWithoutTotal")]
        public List<RentalAgreementRate> RentalAgreementRatesWithoutTotal { get; set; }

        /// <summary>
        /// Gets or Sets RentalAgreementConditions
        /// </summary>
        [DataMember(Name="rentalAgreementConditions")]
        public List<RentalAgreementCondition> RentalAgreementConditions { get; set; }

        /// <summary>
        /// An optional note to be placed onto the Rental Agreement.
        /// </summary>
        /// <value>An optional note to be placed onto the Rental Agreement.</value>
        [DataMember(Name="note")]
        [MetaData (Description = "An optional note to be placed onto the Rental Agreement.")]
        public string Note { get; set; }

        /// <summary>
        /// The estimated start date of the work to be placed on the rental agreement.
        /// </summary>
        /// <value>The estimated start date of the work to be placed on the rental agreement.</value>
        [DataMember(Name="estimateStartWork")]
        [MetaData (Description = "The estimated start date of the work to be placed on the rental agreement.")]
        public string EstimateStartWork { get; set; }

        /// <summary>
        /// The dated on date to put on the Rental Agreement.
        /// </summary>
        /// <value>The dated on date to put on the Rental Agreement.</value>
        [DataMember(Name="datedOn")]
        [MetaData (Description = "The dated on date to put on the Rental Agreement.")]
        public string DatedOn { get; set; }

        /// <summary>
        /// The estimated number of hours of work to be put onto the Rental Agreement.
        /// </summary>
        /// <value>The estimated number of hours of work to be put onto the Rental Agreement.</value>
        [DataMember(Name="estimateHours")]
        [MetaData (Description = "The estimated number of hours of work to be put onto the Rental Agreement.")]
        public int? EstimateHours { get; set; }

        /// <summary>
        /// The dollar rate for the piece of equipment itself for this Rental Agreement. Other rates associated with the Rental Agreement are in the Rental Agreement Rate table.
        /// </summary>
        /// <value>The dollar rate for the piece of equipment itself for this Rental Agreement. Other rates associated with the Rental Agreement are in the Rental Agreement Rate table.</value>
        [DataMember(Name="equipmentRate")]
        [MetaData (Description = "The dollar rate for the piece of equipment itself for this Rental Agreement. Other rates associated with the Rental Agreement are in the Rental Agreement Rate table.")]
        public float? EquipmentRate { get; set; }

        /// <summary>
        /// The period of the rental rate. The vast majority will be hourly, but the rate could apply across a different period, e.g. daily.
        /// </summary>
        /// <value>The period of the rental rate. The vast majority will be hourly, but the rate could apply across a different period, e.g. daily.</value>
        [DataMember(Name="ratePeriod")]
        [MetaData (Description = "The period of the rental rate. The vast majority will be hourly, but the rate could apply across a different period, e.g. daily.")]
        public string RatePeriod { get; set; }

        /// <summary>
        /// A comment about the rate for the piece of equipment.
        /// </summary>
        /// <value>A comment about the rate for the piece of equipment.</value>
        [DataMember(Name="rateComment")]
        [MetaData (Description = "A comment about the rate for the piece of equipment.")]
        public string RateComment { get; set; }

        /// <summary>
        /// Are there any conditions in this agreement
        /// </summary>
        /// <value>Are there any conditions in this agreement</value>
        [DataMember(Name = "conditionsPresent")]
        [MetaData(Description = "Are there any conditions in this agreement.")]
        public bool ConditionsPresent { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class RentalAgreementPdfViewModel {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Number: ").Append(Number).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  Equipment: ").Append(Equipment).Append("\n");
            sb.Append("  Project: ").Append(Project).Append("\n");
            sb.Append("  RentalAgreementRates: ").Append(RentalAgreementRates).Append("\n");
            sb.Append("  RentalAgreementRatesWithTotal: ").Append(RentalAgreementRatesWithTotal).Append("\n");
            sb.Append("  AgreementTotal: ").Append(AgreementTotal).Append("\n");
            sb.Append("  RentalAgreementRatesWithoutTotal: ").Append(RentalAgreementRatesWithoutTotal).Append("\n");
            sb.Append("  RentalAgreementConditions: ").Append(RentalAgreementConditions).Append("\n");
            sb.Append("  Note: ").Append(Note).Append("\n");
            sb.Append("  EstimateStartWork: ").Append(EstimateStartWork).Append("\n");
            sb.Append("  DatedOn: ").Append(DatedOn).Append("\n");
            sb.Append("  EstimateHours: ").Append(EstimateHours).Append("\n");
            sb.Append("  EquipmentRate: ").Append(EquipmentRate).Append("\n");
            sb.Append("  RatePeriod: ").Append(RatePeriod).Append("\n");
            sb.Append("  RateComment: ").Append(RateComment).Append("\n");
            sb.Append("  ConditionsPresent: ").Append(ConditionsPresent).Append("\n");
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
            return obj.GetType() == GetType() && Equals((RentalAgreementPdfViewModel)obj);
        }

        /// <summary>
        /// Returns true if RentalAgreementPdfViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of RentalAgreementPdfViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RentalAgreementPdfViewModel other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    Number == other.Number ||
                    Number != null &&
                    Number.Equals(other.Number)
                ) &&                 
                (
                    Status == other.Status ||
                    Status != null &&
                    Status.Equals(other.Status)
                ) &&                 
                (
                    Equipment == other.Equipment ||
                    Equipment != null &&
                    Equipment.Equals(other.Equipment)
                ) &&                 
                (
                    Project == other.Project ||
                    Project != null &&
                    Project.Equals(other.Project)
                ) && 
                (
                    RentalAgreementRates == other.RentalAgreementRates ||
                    RentalAgreementRates != null &&
                    RentalAgreementRates.SequenceEqual(other.RentalAgreementRates)
                ) &&
                (
                    RentalAgreementRatesWithTotal == other.RentalAgreementRatesWithTotal ||
                    RentalAgreementRatesWithTotal != null &&
                    RentalAgreementRatesWithTotal.SequenceEqual(other.RentalAgreementRatesWithTotal)
                ) &&
                (
                    AgreementTotal == other.AgreementTotal ||
                    AgreementTotal != null &&
                    AgreementTotal.Equals(other.AgreementTotal)
                ) &&
                (
                    RentalAgreementRatesWithoutTotal == other.RentalAgreementRatesWithoutTotal ||
                    RentalAgreementRatesWithoutTotal != null &&
                    RentalAgreementRates.SequenceEqual(other.RentalAgreementRatesWithoutTotal)
                ) && 
                (
                    RentalAgreementConditions == other.RentalAgreementConditions ||
                    RentalAgreementConditions != null &&
                    RentalAgreementConditions.SequenceEqual(other.RentalAgreementConditions)
                ) &&                 
                (
                    Note == other.Note ||
                    Note != null &&
                    Note.Equals(other.Note)
                ) &&                 
                (
                    EstimateStartWork == other.EstimateStartWork ||
                    EstimateStartWork != null &&
                    EstimateStartWork.Equals(other.EstimateStartWork)
                ) &&                 
                (
                    DatedOn == other.DatedOn ||
                    DatedOn != null &&
                    DatedOn.Equals(other.DatedOn)
                ) &&                 
                (
                    EstimateHours == other.EstimateHours ||
                    EstimateHours != null &&
                    EstimateHours.Equals(other.EstimateHours)
                ) &&                 
                (
                    EquipmentRate == other.EquipmentRate ||
                    EquipmentRate != null &&
                    EquipmentRate.Equals(other.EquipmentRate)
                ) &&                 
                (
                    RatePeriod == other.RatePeriod ||
                    RatePeriod != null &&
                    RatePeriod.Equals(other.RatePeriod)
                ) &&                 
                (
                    RateComment == other.RateComment ||
                    RateComment != null &&
                    RateComment.Equals(other.RateComment)
                ) &&
                (
                    ConditionsPresent == other.ConditionsPresent ||
                    ConditionsPresent.Equals(other.ConditionsPresent)
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

                if (Number != null)
                {
                    hash = hash * 59 + Number.GetHashCode();
                }

                if (Status != null)
                {
                    hash = hash * 59 + Status.GetHashCode();
                }                
                                   
                if (Equipment != null)
                {
                    hash = hash * 59 + Equipment.GetHashCode();
                }

                if (Project != null)
                {
                    hash = hash * 59 + Project.GetHashCode();
                }

                if (RentalAgreementRates != null)
                {
                    hash = hash * 59 + RentalAgreementRates.GetHashCode();
                }

                if (RentalAgreementRatesWithTotal != null)
                {
                    hash = hash * 59 + RentalAgreementRatesWithTotal.GetHashCode();
                }

                if (RentalAgreementRatesWithoutTotal != null)
                {
                    hash = hash * 59 + RentalAgreementRatesWithoutTotal.GetHashCode();
                }

                if (RentalAgreementConditions != null)
                {
                    hash = hash * 59 + RentalAgreementConditions.GetHashCode();
                }
                
                if (Note != null)
                {
                    hash = hash * 59 + Note.GetHashCode();
                }

                if (EstimateStartWork != null)
                {
                    hash = hash * 59 + EstimateStartWork.GetHashCode();
                }

                if (DatedOn != null)
                {
                    hash = hash * 59 + DatedOn.GetHashCode();
                }

                if (EstimateHours != null)
                {
                    hash = hash * 59 + EstimateHours.GetHashCode();
                }

                if (EquipmentRate != null)
                {
                    hash = hash * 59 + EquipmentRate.GetHashCode();
                }

                if (RatePeriod != null)
                {
                    hash = hash * 59 + RatePeriod.GetHashCode();
                }

                if (RateComment != null)
                {
                    hash = hash * 59 + RateComment.GetHashCode();
                }

                hash = hash * 59 + ConditionsPresent.GetHashCode();

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
        public static bool operator ==(RentalAgreementPdfViewModel left, RentalAgreementPdfViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(RentalAgreementPdfViewModel left, RentalAgreementPdfViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
