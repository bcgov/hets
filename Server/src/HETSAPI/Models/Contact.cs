using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// Contact Database Model
    /// </summary>
    [MetaData (Description = "A person and their related contact information linked to one or more entities in the system. For examples, there are contacts for Owners, Projects.")]
    public sealed class Contact : AuditableEntity, IEquatable<Contact>
    {
        /// <summary>
        /// Contact Database Model Constructor (required by entity framework)
        /// </summary>
        public Contact()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Contact" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a Contact (required).</param>
        /// <param name="givenName">The given name of the contact..</param>
        /// <param name="surname">The surname of the contact..</param>
        /// <param name="organizationName">The organization name of the contact..</param>
        /// <param name="role">The role of the contact. UI controlled as to whether it is free form or selected from an enumerated list - for initial implementation, the field is freeform..</param>
        /// <param name="notes">A note about the contact maintained by the users..</param>
        /// <param name="emailAddress">The email address for the contact..</param>
        /// <param name="workPhoneNumber">The work phone number for the contact..</param>
        /// <param name="mobilePhoneNumber">The mobile phone number for the contact..</param>
        /// <param name="faxPhoneNumber">The fax phone number for the contact..</param>
        /// <param name="address1">Address 1 line of the address..</param>
        /// <param name="address2">Address 2 line of the address..</param>
        /// <param name="city">The City of the address..</param>
        /// <param name="province">The Province of the address..</param>
        /// <param name="postalCode">The postal code of the address..</param>
        public Contact(int id, string givenName = null, string surname = null, string organizationName = null, string role = null, 
            string notes = null, string emailAddress = null, string workPhoneNumber = null, string mobilePhoneNumber = null, 
            string faxPhoneNumber = null, string address1 = null, string address2 = null, string city = null, string province = null, 
            string postalCode = null)
        {
            Id = id;
            GivenName = givenName;
            Surname = surname;
            OrganizationName = organizationName;
            Role = role;
            Notes = notes;
            EmailAddress = emailAddress;
            WorkPhoneNumber = workPhoneNumber;
            MobilePhoneNumber = mobilePhoneNumber;
            FaxPhoneNumber = faxPhoneNumber;
            Address1 = address1;
            Address2 = address2;
            City = city;
            Province = province;
            PostalCode = postalCode;
        }

        /// <summary>
        /// A system-generated unique identifier for a Contact
        /// </summary>
        /// <value>A system-generated unique identifier for a Contact</value>
        [MetaData (Description = "A system-generated unique identifier for a Contact")]
        public int Id { get; set; }

        /// <summary>
        /// The given name of the contact.
        /// </summary>
        /// <value>The given name of the contact.</value>
        [MetaData (Description = "The given name of the contact.")]
        [MaxLength(50)]
        public string GivenName { get; set; }

        /// <summary>
        /// The surname of the contact.
        /// </summary>
        /// <value>The surname of the contact.</value>
        [MetaData (Description = "The surname of the contact.")]
        [MaxLength(50)]
        public string Surname { get; set; }

        /// <summary>
        /// The organization name of the contact.
        /// </summary>
        /// <value>The organization name of the contact.</value>
        [MetaData (Description = "The organization name of the contact.")]
        [MaxLength(150)]
        public string OrganizationName { get; set; }

        /// <summary>
        /// The role of the contact. UI controlled as to whether it is free form or selected from an enumerated list - for initial implementation, the field is freeform.
        /// </summary>
        /// <value>The role of the contact. UI controlled as to whether it is free form or selected from an enumerated list - for initial implementation, the field is freeform.</value>
        [MetaData (Description = "The role of the contact. UI controlled as to whether it is free form or selected from an enumerated list - for initial implementation, the field is freeform.")]
        [MaxLength(100)]
        public string Role { get; set; }

        /// <summary>
        /// A note about the contact maintained by the users.
        /// </summary>
        /// <value>A note about the contact maintained by the users.</value>
        [MetaData (Description = "A note about the contact maintained by the users.")]
        [MaxLength(512)]
        public string Notes { get; set; }

        /// <summary>
        /// The email address for the contact.
        /// </summary>
        /// <value>The email address for the contact.</value>
        [MetaData (Description = "The email address for the contact.")]
        [MaxLength(255)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// The work phone number for the contact.
        /// </summary>
        /// <value>The work phone number for the contact.</value>
        [MetaData (Description = "The work phone number for the contact.")]
        [MaxLength(20)]
        public string WorkPhoneNumber { get; set; }

        /// <summary>
        /// The mobile phone number for the contact.
        /// </summary>
        /// <value>The mobile phone number for the contact.</value>
        [MetaData (Description = "The mobile phone number for the contact.")]
        [MaxLength(20)]
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// The fax phone number for the contact.
        /// </summary>
        /// <value>The fax phone number for the contact.</value>
        [MetaData (Description = "The fax phone number for the contact.")]
        [MaxLength(20)]
        public string FaxPhoneNumber { get; set; }

        /// <summary>
        /// Address 1 line of the address.
        /// </summary>
        /// <value>Address 1 line of the address.</value>
        [MetaData (Description = "Address 1 line of the address.")]
        [MaxLength(80)]
        public string Address1 { get; set; }

        /// <summary>
        /// Address 2 line of the address.
        /// </summary>
        /// <value>Address 2 line of the address.</value>
        [MetaData (Description = "Address 2 line of the address.")]
        [MaxLength(80)]
        public string Address2 { get; set; }

        /// <summary>
        /// The City of the address.
        /// </summary>
        /// <value>The City of the address.</value>
        [MetaData (Description = "The City of the address.")]
        [MaxLength(100)]
        public string City { get; set; }

        /// <summary>
        /// The Province of the address.
        /// </summary>
        /// <value>The Province of the address.</value>
        [MetaData (Description = "The Province of the address.")]
        [MaxLength(50)]
        public string Province { get; set; }

        /// <summary>
        /// The postal code of the address.
        /// </summary>
        /// <value>The postal code of the address.</value>
        [MetaData (Description = "The postal code of the address.")]
        [MaxLength(15)]
        public string PostalCode { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class Contact {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  GivenName: ").Append(GivenName).Append("\n");
            sb.Append("  Surname: ").Append(Surname).Append("\n");
            sb.Append("  OrganizationName: ").Append(OrganizationName).Append("\n");
            sb.Append("  Role: ").Append(Role).Append("\n");
            sb.Append("  Notes: ").Append(Notes).Append("\n");
            sb.Append("  EmailAddress: ").Append(EmailAddress).Append("\n");
            sb.Append("  WorkPhoneNumber: ").Append(WorkPhoneNumber).Append("\n");
            sb.Append("  MobilePhoneNumber: ").Append(MobilePhoneNumber).Append("\n");
            sb.Append("  FaxPhoneNumber: ").Append(FaxPhoneNumber).Append("\n");
            sb.Append("  Address1: ").Append(Address1).Append("\n");
            sb.Append("  Address2: ").Append(Address2).Append("\n");
            sb.Append("  City: ").Append(City).Append("\n");
            sb.Append("  Province: ").Append(Province).Append("\n");
            sb.Append("  PostalCode: ").Append(PostalCode).Append("\n");
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
            return obj.GetType() == GetType() && Equals((Contact)obj);
        }

        /// <summary>
        /// Returns true if Contact instances are equal
        /// </summary>
        /// <param name="other">Instance of Contact to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Contact other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&
                (
                    GivenName == other.GivenName ||
                    GivenName != null &&
                    GivenName.Equals(other.GivenName)
                ) &&
                (
                    Surname == other.Surname ||
                    Surname != null &&
                    Surname.Equals(other.Surname)
                ) &&
                (
                    OrganizationName == other.OrganizationName ||
                    OrganizationName != null &&
                    OrganizationName.Equals(other.OrganizationName)
                ) &&
                (
                    Role == other.Role ||
                    Role != null &&
                    Role.Equals(other.Role)
                ) &&
                (
                    Notes == other.Notes ||
                    Notes != null &&
                    Notes.Equals(other.Notes)
                ) &&
                (
                    EmailAddress == other.EmailAddress ||
                    EmailAddress != null &&
                    EmailAddress.Equals(other.EmailAddress)
                ) &&
                (
                    WorkPhoneNumber == other.WorkPhoneNumber ||
                    WorkPhoneNumber != null &&
                    WorkPhoneNumber.Equals(other.WorkPhoneNumber)
                ) &&
                (
                    MobilePhoneNumber == other.MobilePhoneNumber ||
                    MobilePhoneNumber != null &&
                    MobilePhoneNumber.Equals(other.MobilePhoneNumber)
                ) &&
                (
                    FaxPhoneNumber == other.FaxPhoneNumber ||
                    FaxPhoneNumber != null &&
                    FaxPhoneNumber.Equals(other.FaxPhoneNumber)
                ) &&
                (
                    Address1 == other.Address1 ||
                    Address1 != null &&
                    Address1.Equals(other.Address1)
                ) &&
                (
                    Address2 == other.Address2 ||
                    Address2 != null &&
                    Address2.Equals(other.Address2)
                ) &&
                (
                    City == other.City ||
                    City != null &&
                    City.Equals(other.City)
                ) &&
                (
                    Province == other.Province ||
                    Province != null &&
                    Province.Equals(other.Province)
                ) &&
                (
                    PostalCode == other.PostalCode ||
                    PostalCode != null &&
                    PostalCode.Equals(other.PostalCode)
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

                if (GivenName != null)
                {
                    hash = hash * 59 + GivenName.GetHashCode();
                }

                if (Surname != null)
                {
                    hash = hash * 59 + Surname.GetHashCode();
                }

                if (OrganizationName != null)
                {
                    hash = hash * 59 + OrganizationName.GetHashCode();
                }

                if (Role != null)
                {
                    hash = hash * 59 + Role.GetHashCode();
                }

                if (Notes != null)
                {
                    hash = hash * 59 + Notes.GetHashCode();
                }

                if (EmailAddress != null)
                {
                    hash = hash * 59 + EmailAddress.GetHashCode();
                }

                if (WorkPhoneNumber != null)
                {
                    hash = hash * 59 + WorkPhoneNumber.GetHashCode();
                }

                if (MobilePhoneNumber != null)
                {
                    hash = hash * 59 + MobilePhoneNumber.GetHashCode();
                }

                if (FaxPhoneNumber != null)
                {
                    hash = hash * 59 + FaxPhoneNumber.GetHashCode();
                }

                if (Address1 != null)
                {
                    hash = hash * 59 + Address1.GetHashCode();
                }

                if (Address2 != null)
                {
                    hash = hash * 59 + Address2.GetHashCode();
                }

                if (City != null)
                {
                    hash = hash * 59 + City.GetHashCode();
                }

                if (Province != null)
                {
                    hash = hash * 59 + Province.GetHashCode();
                }

                if (PostalCode != null)
                {
                    hash = hash * 59 + PostalCode.GetHashCode();
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
        public static bool operator ==(Contact left, Contact right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Contact left, Contact right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
