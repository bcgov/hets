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
    /// Information about the hiring of a specific piece of equipment to satisfy part or all of a request from a project. TABLE DEFINITION IN PROGRESS - MORE COLUMNS TO BE ADDED
    /// </summary>
        [MetaDataExtension (Description = "Information about the hiring of a specific piece of equipment to satisfy part or all of a request from a project. TABLE DEFINITION IN PROGRESS - MORE COLUMNS TO BE ADDED")]

    public partial class RentalAgreement : IEquatable<RentalAgreement>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public RentalAgreement()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalAgreement" /> class.
        /// </summary>
        /// <param name="Id">A system-generated unique identifier for a RentalAgreement (required).</param>
        /// <param name="Equipment">Equipment.</param>
        /// <param name="Project">Project.</param>
        /// <param name="TimeRecords">TimeRecords.</param>
        public RentalAgreement(int Id, Equipment Equipment = null, Project Project = null, List<TimeRecord> TimeRecords = null)
        {   
            this.Id = Id;
            this.Equipment = Equipment;
            this.Project = Project;
            this.TimeRecords = TimeRecords;
        }

        /// <summary>
        /// A system-generated unique identifier for a RentalAgreement
        /// </summary>
        /// <value>A system-generated unique identifier for a RentalAgreement</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a RentalAgreement")]
        public int Id { get; set; }
        
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
        /// Gets or Sets Project
        /// </summary>
        public Project Project { get; set; }
        
        /// <summary>
        /// Foreign key for Project 
        /// </summary>       
        [ForeignKey("Project")]
        public int? ProjectRefId { get; set; }
        
        /// <summary>
        /// Gets or Sets TimeRecords
        /// </summary>
        public List<TimeRecord> TimeRecords { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class RentalAgreement {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Equipment: ").Append(Equipment).Append("\n");
            sb.Append("  Project: ").Append(Project).Append("\n");
            sb.Append("  TimeRecords: ").Append(TimeRecords).Append("\n");
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
            return Equals((RentalAgreement)obj);
        }

        /// <summary>
        /// Returns true if RentalAgreement instances are equal
        /// </summary>
        /// <param name="other">Instance of RentalAgreement to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RentalAgreement other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.Equipment == other.Equipment ||
                    this.Equipment != null &&
                    this.Equipment.Equals(other.Equipment)
                ) &&                 
                (
                    this.Project == other.Project ||
                    this.Project != null &&
                    this.Project.Equals(other.Project)
                ) && 
                (
                    this.TimeRecords == other.TimeRecords ||
                    this.TimeRecords != null &&
                    this.TimeRecords.SequenceEqual(other.TimeRecords)
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
                if (this.Equipment != null)
                {
                    hash = hash * 59 + this.Equipment.GetHashCode();
                }                   
                if (this.Project != null)
                {
                    hash = hash * 59 + this.Project.GetHashCode();
                }                   
                if (this.TimeRecords != null)
                {
                    hash = hash * 59 + this.TimeRecords.GetHashCode();
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
        public static bool operator ==(RentalAgreement left, RentalAgreement right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(RentalAgreement left, RentalAgreement right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
