using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using HETSAPI.Models;

namespace HETSAPI.ViewModels
{
    /// <summary>
    /// Owner Verification Pdf View Model
    /// </summary>
    [DataContract]
    public sealed class OwnerVerificationPdfViewModel : IEquatable<OwnerVerificationPdfViewModel>
    {
        /// <summary>
        /// /// Rental Agreement Pdf View Model Constructor
        /// </summary>
        public OwnerVerificationPdfViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnerVerificationPdfViewModel" /> class.
        /// </summary>
        /// <param name="reportDate">Pdf Report Date</param>
        /// <param name="title">Pdf Document Title</param>
        /// <param name="districtId">The District Id</param>
        /// <param name="ministryDistrictId">The Ministry District Id</param>
        /// <param name="districtName">The District Name</param>
        /// <param name="districtAddress">The District Address String</param>
        /// <param name="districtContact">The District Contact String</param>
        /// <param name="owners">A list of owner records (including equipment)</param>
        public OwnerVerificationPdfViewModel(string reportDate, string title, int districtId, int ministryDistrictId, string districtName,
            string districtAddress, string districtContact, List<Owner> owners)
        {
            ReportDate = reportDate;
            Title = title;
            DistrictId = districtId;
            MinistryDistrictId = ministryDistrictId;
            DistrictName = districtName;
            DistrictAddress = districtAddress;
            DistrictContact = districtContact;
            Owners = owners;
        }

        /// <summary>
        /// Pdf Report Date
        /// </summary>
        [DataMember(Name = "reportDate")]
        public string ReportDate { get; set; }

        /// <summary>
        /// Pdf Document Title
        /// </summary>
        [DataMember(Name= "title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or Sets the District Id
        /// </summary>
        [DataMember(Name = "districtId")]
        public int DistrictId { get; set; }

        /// <summary>
        /// Gets or Sets the Ministry District Id
        /// </summary>
        [DataMember(Name = "ministryDistrictId")]
        public int MinistryDistrictId { get; set; }

        /// <summary>
        /// Gets or Sets the District Name
        /// </summary>
        [DataMember(Name = "districtName")]
        public string DistrictName { get; set; }

        /// <summary>
        /// Gets or Sets the District Address String
        /// </summary>
        [DataMember(Name = "districtAddress")]
        public string DistrictAddress { get; set; }

        /// <summary>
        /// Gets or Sets the District Contact (phone) String
        /// </summary>
        [DataMember(Name = "districtContact")]
        public string DistrictContact { get; set; }

        /// <summary>
        /// Gets or Sets Owner records
        /// </summary>
        [DataMember(Name = "owners")]
        public List<Owner> Owners { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();            

            sb.Append("class RentalAgreementPdfViewModel {\n");            
            sb.Append("  ReportDate: ").Append(ReportDate).Append("\n");
            sb.Append("  Title: ").Append(Title).Append("\n");
            sb.Append("  DistrictId: ").Append(DistrictId).Append("\n");
            sb.Append("  MinistryDistrictId: ").Append(MinistryDistrictId).Append("\n");
            sb.Append("  DistrictName: ").Append(DistrictName).Append("\n");
            sb.Append("  DistrictAddress: ").Append(DistrictAddress).Append("\n");
            sb.Append("  DistrictContact: ").Append(DistrictContact).Append("\n");
            sb.Append("  Owners: ").Append(Owners).Append("\n");
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
            return obj.GetType() == GetType() && Equals((OwnerVerificationPdfViewModel)obj);
        }

        /// <summary>
        /// Returns true if OwnerVerificationPdfViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of OwnerVerificationPdfViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(OwnerVerificationPdfViewModel other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }            

            return
                (
                    ReportDate == other.ReportDate ||
                    ReportDate.Equals(other.ReportDate)
                ) && (
                    Title == other.Title ||
                    Title.Equals(other.Title)
                ) &&
                (
                    DistrictId == other.DistrictId ||
                    DistrictId.Equals(other.DistrictId)
                ) &&
                (
                    MinistryDistrictId == other.MinistryDistrictId ||
                    MinistryDistrictId.Equals(other.MinistryDistrictId)
                ) &&
                (
                    DistrictName == other.DistrictName ||
                    DistrictName.Equals(other.DistrictName)
                ) &&
                (
                    DistrictAddress == other.DistrictAddress ||
                    DistrictAddress.Equals(other.DistrictAddress)
                ) &&
                (
                    DistrictContact == other.DistrictContact ||
                    DistrictContact.Equals(other.DistrictContact)
                ) &&
                (
                    Owners == other.Owners ||
                    Owners.Equals(other.Owners)
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
                hash = hash * 59 + ReportDate.GetHashCode();
                hash = hash * 59 + Title.GetHashCode();
                hash = hash * 59 + DistrictId.GetHashCode();
                hash = hash * 59 + MinistryDistrictId.GetHashCode();
                hash = hash * 59 + DistrictName.GetHashCode();
                hash = hash * 59 + DistrictAddress.GetHashCode();
                hash = hash * 59 + DistrictContact.GetHashCode();
                hash = hash * 59 + Owners.GetHashCode();

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
        public static bool operator ==(OwnerVerificationPdfViewModel left, OwnerVerificationPdfViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(OwnerVerificationPdfViewModel left, OwnerVerificationPdfViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
