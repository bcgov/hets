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

namespace HETSAPI.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class ProjectSearchResultViewModel : IEquatable<ProjectSearchResultViewModel>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public ProjectSearchResultViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectSearchResultViewModel" /> class.
        /// </summary>
        /// <param name="Id">Id (required).</param>
        /// <param name="LocalArea">LocalArea.</param>
        /// <param name="Name">Name.</param>
        /// <param name="PrimaryContact">PrimaryContact.</param>
        /// <param name="Hires">count of RentalAgreement.status is Active for the project.</param>
        /// <param name="Requests">count of RentalRequest.status is Active for the project.</param>
        /// <param name="Status">Project status.</param>
        public ProjectSearchResultViewModel(int Id, LocalArea LocalArea = null, string Name = null, Contact PrimaryContact = null, int? Hires = null, int? Requests = null, string Status = null)
        {   
            this.Id = Id;
            this.LocalArea = LocalArea;
            this.Name = Name;
            this.PrimaryContact = PrimaryContact;
            this.Hires = Hires;
            this.Requests = Requests;
            this.Status = Status;
        }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or Sets LocalArea
        /// </summary>
        [DataMember(Name="localArea")]
        public LocalArea LocalArea { get; set; }

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
        [MetaDataExtension (Description = "count of RentalAgreement.status is Active for the project")]
        public int? Hires { get; set; }

        /// <summary>
        /// count of RentalRequest.status is Active for the project
        /// </summary>
        /// <value>count of RentalRequest.status is Active for the project</value>
        [DataMember(Name="requests")]
        [MetaDataExtension (Description = "count of RentalRequest.status is Active for the project")]
        public int? Requests { get; set; }

        /// <summary>
        /// Project status
        /// </summary>
        /// <value>Project status</value>
        [DataMember(Name="status")]
        [MetaDataExtension (Description = "Project status")]
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
            sb.Append("  LocalArea: ").Append(LocalArea).Append("\n");
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
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((ProjectSearchResultViewModel)obj);
        }

        /// <summary>
        /// Returns true if ProjectSearchResultViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of ProjectSearchResultViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ProjectSearchResultViewModel other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.LocalArea == other.LocalArea ||
                    this.LocalArea != null &&
                    this.LocalArea.Equals(other.LocalArea)
                ) &&                 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) &&                 
                (
                    this.PrimaryContact == other.PrimaryContact ||
                    this.PrimaryContact != null &&
                    this.PrimaryContact.Equals(other.PrimaryContact)
                ) &&                 
                (
                    this.Hires == other.Hires ||
                    this.Hires != null &&
                    this.Hires.Equals(other.Hires)
                ) &&                 
                (
                    this.Requests == other.Requests ||
                    this.Requests != null &&
                    this.Requests.Equals(other.Requests)
                ) &&                 
                (
                    this.Status == other.Status ||
                    this.Status != null &&
                    this.Status.Equals(other.Status)
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
                                   
                hash = hash * 59 + this.Id.GetHashCode();                   
                if (this.LocalArea != null)
                {
                    hash = hash * 59 + this.LocalArea.GetHashCode();
                }                if (this.Name != null)
                {
                    hash = hash * 59 + this.Name.GetHashCode();
                }                
                                   
                if (this.PrimaryContact != null)
                {
                    hash = hash * 59 + this.PrimaryContact.GetHashCode();
                }                if (this.Hires != null)
                {
                    hash = hash * 59 + this.Hires.GetHashCode();
                }                
                                if (this.Requests != null)
                {
                    hash = hash * 59 + this.Requests.GetHashCode();
                }                
                                if (this.Status != null)
                {
                    hash = hash * 59 + this.Status.GetHashCode();
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
