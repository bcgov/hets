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
    public partial class RentalRequestSearchResultViewModel : IEquatable<RentalRequestSearchResultViewModel>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public RentalRequestSearchResultViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalRequestSearchResultViewModel" /> class.
        /// </summary>
        /// <param name="Id">Id (required).</param>
        /// <param name="LocalArea">LocalArea.</param>
        /// <param name="EquipmentCount">EquipmentCount.</param>
        /// <param name="EquipmentTypeName">EquipmentTypeName.</param>
        /// <param name="ProjectName">ProjectName.</param>
        /// <param name="PrimaryContact">PrimaryContact.</param>
        /// <param name="Status">Project status.</param>
        public RentalRequestSearchResultViewModel(int Id, LocalArea LocalArea = null, int? EquipmentCount = null, string EquipmentTypeName = null, string ProjectName = null, Contact PrimaryContact = null, string Status = null)
        {   
            this.Id = Id;
            this.LocalArea = LocalArea;
            this.EquipmentCount = EquipmentCount;
            this.EquipmentTypeName = EquipmentTypeName;
            this.ProjectName = ProjectName;
            this.PrimaryContact = PrimaryContact;
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
        /// Gets or Sets EquipmentCount
        /// </summary>
        [DataMember(Name="equipmentCount")]
        public int? EquipmentCount { get; set; }

        /// <summary>
        /// Gets or Sets EquipmentTypeName
        /// </summary>
        [DataMember(Name="equipmentTypeName")]
        public string EquipmentTypeName { get; set; }

        /// <summary>
        /// Gets or Sets ProjectName
        /// </summary>
        [DataMember(Name="projectName")]
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or Sets PrimaryContact
        /// </summary>
        [DataMember(Name="primaryContact")]
        public Contact PrimaryContact { get; set; }

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
            sb.Append("class RentalRequestSearchResultViewModel {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  LocalArea: ").Append(LocalArea).Append("\n");
            sb.Append("  EquipmentCount: ").Append(EquipmentCount).Append("\n");
            sb.Append("  EquipmentTypeName: ").Append(EquipmentTypeName).Append("\n");
            sb.Append("  ProjectName: ").Append(ProjectName).Append("\n");
            sb.Append("  PrimaryContact: ").Append(PrimaryContact).Append("\n");
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
            return Equals((RentalRequestSearchResultViewModel)obj);
        }

        /// <summary>
        /// Returns true if RentalRequestSearchResultViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of RentalRequestSearchResultViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RentalRequestSearchResultViewModel other)
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
                    this.EquipmentCount == other.EquipmentCount ||
                    this.EquipmentCount != null &&
                    this.EquipmentCount.Equals(other.EquipmentCount)
                ) &&                 
                (
                    this.EquipmentTypeName == other.EquipmentTypeName ||
                    this.EquipmentTypeName != null &&
                    this.EquipmentTypeName.Equals(other.EquipmentTypeName)
                ) &&                 
                (
                    this.ProjectName == other.ProjectName ||
                    this.ProjectName != null &&
                    this.ProjectName.Equals(other.ProjectName)
                ) &&                 
                (
                    this.PrimaryContact == other.PrimaryContact ||
                    this.PrimaryContact != null &&
                    this.PrimaryContact.Equals(other.PrimaryContact)
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
                }                if (this.EquipmentCount != null)
                {
                    hash = hash * 59 + this.EquipmentCount.GetHashCode();
                }                
                                if (this.EquipmentTypeName != null)
                {
                    hash = hash * 59 + this.EquipmentTypeName.GetHashCode();
                }                
                                if (this.ProjectName != null)
                {
                    hash = hash * 59 + this.ProjectName.GetHashCode();
                }                
                                   
                if (this.PrimaryContact != null)
                {
                    hash = hash * 59 + this.PrimaryContact.GetHashCode();
                }                if (this.Status != null)
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
        public static bool operator ==(RentalRequestSearchResultViewModel left, RentalRequestSearchResultViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(RentalRequestSearchResultViewModel left, RentalRequestSearchResultViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
