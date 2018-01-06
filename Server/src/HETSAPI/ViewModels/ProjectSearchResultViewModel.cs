using System;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using HETSAPI.Models;

namespace HETSAPI.ViewModels
{
    /// <summary>
    /// Project Search Result View Model
    /// </summary>
    [DataContract]
    public sealed class ProjectSearchResultViewModel : IEquatable<ProjectSearchResultViewModel>
    {
        /// <summary>
        /// Project Search Result View Model Constructor
        /// </summary>
        public ProjectSearchResultViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectSearchResultViewModel" /> class.
        /// </summary>
        /// <param name="id">Id (required).</param>
        /// <param name="district">District.</param>
        /// <param name="name">Name.</param>
        /// <param name="primaryContact">PrimaryContact.</param>
        /// <param name="hires">count of RentalAgreement.status is Active for the project.</param>
        /// <param name="requests">count of RentalRequest.status is Active for the project.</param>
        /// <param name="status">Project status.</param>
        public ProjectSearchResultViewModel(int id, District district = null, string name = null, Contact primaryContact = null, 
            int? hires = null, int? requests = null, string status = null)
        {   
            Id = id;
            District = district;
            Name = name;
            PrimaryContact = primaryContact;
            Hires = hires;
            Requests = requests;
            Status = status;
        }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or Sets District
        /// </summary>
        [DataMember(Name="district")]
        public District District { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets PrimaryContact
        /// </summary>
        [DataMember(Name="primaryContact")]
        public Contact PrimaryContact { get; set; }

        /// <summary>
        /// count of RentalAgreement.status is Active for the project
        /// </summary>
        /// <value>count of RentalAgreement.status is Active for the project</value>
        [DataMember(Name="hires")]
        [MetaData (Description = "count of RentalAgreement.status is Active for the project")]
        public int? Hires { get; set; }

        /// <summary>
        /// count of RentalRequest.status is Active for the project
        /// </summary>
        /// <value>count of RentalRequest.status is Active for the project</value>
        [DataMember(Name="requests")]
        [MetaData (Description = "count of RentalRequest.status is Active for the project")]
        public int? Requests { get; set; }

        /// <summary>
        /// Project status
        /// </summary>
        /// <value>Project status</value>
        [DataMember(Name="status")]
        [MetaData (Description = "Project status")]
        public string Status { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class ProjectSearchResultViewModel {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  District: ").Append(District).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  PrimaryContact: ").Append(PrimaryContact).Append("\n");
            sb.Append("  Hires: ").Append(Hires).Append("\n");
            sb.Append("  Requests: ").Append(Requests).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
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
            return obj.GetType() == GetType() && Equals((ProjectSearchResultViewModel)obj);
        }

        /// <summary>
        /// Returns true if ProjectSearchResultViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of ProjectSearchResultViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ProjectSearchResultViewModel other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    District == other.District ||
                    District != null &&
                    District.Equals(other.District)
                ) &&                 
                (
                    Name == other.Name ||
                    Name != null &&
                    Name.Equals(other.Name)
                ) &&                 
                (
                    PrimaryContact == other.PrimaryContact ||
                    PrimaryContact != null &&
                    PrimaryContact.Equals(other.PrimaryContact)
                ) &&                 
                (
                    Hires == other.Hires ||
                    Hires != null &&
                    Hires.Equals(other.Hires)
                ) &&                 
                (
                    Requests == other.Requests ||
                    Requests != null &&
                    Requests.Equals(other.Requests)
                ) &&                 
                (
                    Status == other.Status ||
                    Status != null &&
                    Status.Equals(other.Status)
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

                if (District != null)
                {
                    hash = hash * 59 + District.GetHashCode();
                }

                if (Name != null)
                {
                    hash = hash * 59 + Name.GetHashCode();
                }                
                                   
                if (PrimaryContact != null)
                {
                    hash = hash * 59 + PrimaryContact.GetHashCode();
                }

                if (Hires != null)
                {
                    hash = hash * 59 + Hires.GetHashCode();
                }

                if (Requests != null)
                {
                    hash = hash * 59 + Requests.GetHashCode();
                }

                if (Status != null)
                {
                    hash = hash * 59 + Status.GetHashCode();
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
        public static bool operator ==(ProjectSearchResultViewModel left, ProjectSearchResultViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(ProjectSearchResultViewModel left, ProjectSearchResultViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
