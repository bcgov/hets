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
    /// A permission that is part of a Role - a component of the authorization provided by the Role to the user to which the Role is assigned.
    /// </summary>
        [MetaDataExtension (Description = "A permission that is part of a Role - a component of the authorization provided by the Role to the user to which the Role is assigned.")]

    public partial class RolePermission : IEquatable<RolePermission>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public RolePermission()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RolePermission" /> class.
        /// </summary>
        /// <param name="Id">A system-generated unique identifier for a RolePermission (required).</param>
        /// <param name="Role">Role.</param>
        /// <param name="Permission">Permission.</param>
        public RolePermission(int Id, Role Role = null, Permission Permission = null)
        {   
            this.Id = Id;
            this.Role = Role;
            this.Permission = Permission;
        }

        /// <summary>
        /// A system-generated unique identifier for a RolePermission
        /// </summary>
        /// <value>A system-generated unique identifier for a RolePermission</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a RolePermission")]
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or Sets Role
        /// </summary>
        public Role Role { get; set; }
        
        /// <summary>
        /// Foreign key for Role 
        /// </summary>       
        [ForeignKey("Role")]
        public int? RoleRefId { get; set; }
        
        /// <summary>
        /// Gets or Sets Permission
        /// </summary>
        public Permission Permission { get; set; }
        
        /// <summary>
        /// Foreign key for Permission 
        /// </summary>       
        [ForeignKey("Permission")]
        public int? PermissionRefId { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class RolePermission {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Role: ").Append(Role).Append("\n");
            sb.Append("  Permission: ").Append(Permission).Append("\n");
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
            return Equals((RolePermission)obj);
        }

        /// <summary>
        /// Returns true if RolePermission instances are equal
        /// </summary>
        /// <param name="other">Instance of RolePermission to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RolePermission other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.Role == other.Role ||
                    this.Role != null &&
                    this.Role.Equals(other.Role)
                ) &&                 
                (
                    this.Permission == other.Permission ||
                    this.Permission != null &&
                    this.Permission.Equals(other.Permission)
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
                if (this.Role != null)
                {
                    hash = hash * 59 + this.Role.GetHashCode();
                }                   
                if (this.Permission != null)
                {
                    hash = hash * 59 + this.Permission.GetHashCode();
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
        public static bool operator ==(RolePermission left, RolePermission right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(RolePermission left, RolePermission right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
