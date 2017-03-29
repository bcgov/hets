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

namespace HETSAPI.Models
{
    /// <summary>
    /// 
    /// </summary>

    public partial class ImportMap : AuditableEntity, IEquatable<ImportMap>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public ImportMap()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportMap" /> class.
        /// </summary>
        /// <param name="Id">A system generated unique identifier for the ImportMap (required).</param>
        /// <param name="OldTable">Table name in old system (required).</param>
        /// <param name="NewTable">Table name in new system. (required).</param>
        /// <param name="OldKey">Old primary key for record (required).</param>
        /// <param name="NewKey">New primary key for record (required).</param>
        public ImportMap(int Id, string OldTable, string NewTable, int OldKey, int NewKey)
        {   
            this.Id = Id;
            this.OldTable = OldTable;
            this.NewTable = NewTable;
            this.OldKey = OldKey;
            this.NewKey = NewKey;




        }

        /// <summary>
        /// A system generated unique identifier for the ImportMap
        /// </summary>
        /// <value>A system generated unique identifier for the ImportMap</value>
        [MetaDataExtension (Description = "A system generated unique identifier for the ImportMap")]
        public int Id { get; set; }
        
        /// <summary>
        /// Table name in old system
        /// </summary>
        /// <value>Table name in old system</value>
        [MetaDataExtension (Description = "Table name in old system")]
        public string OldTable { get; set; }
        
        /// <summary>
        /// Table name in new system.
        /// </summary>
        /// <value>Table name in new system.</value>
        [MetaDataExtension (Description = "Table name in new system.")]
        public string NewTable { get; set; }
        
        /// <summary>
        /// Old primary key for record
        /// </summary>
        /// <value>Old primary key for record</value>
        [MetaDataExtension (Description = "Old primary key for record")]
        public int OldKey { get; set; }
        
        /// <summary>
        /// New primary key for record
        /// </summary>
        /// <value>New primary key for record</value>
        [MetaDataExtension (Description = "New primary key for record")]
        public int NewKey { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ImportMap {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  OldTable: ").Append(OldTable).Append("\n");
            sb.Append("  NewTable: ").Append(NewTable).Append("\n");
            sb.Append("  OldKey: ").Append(OldKey).Append("\n");
            sb.Append("  NewKey: ").Append(NewKey).Append("\n");
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
            return Equals((ImportMap)obj);
        }

        /// <summary>
        /// Returns true if ImportMap instances are equal
        /// </summary>
        /// <param name="other">Instance of ImportMap to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ImportMap other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.OldTable == other.OldTable ||
                    this.OldTable != null &&
                    this.OldTable.Equals(other.OldTable)
                ) &&                 
                (
                    this.NewTable == other.NewTable ||
                    this.NewTable != null &&
                    this.NewTable.Equals(other.NewTable)
                ) &&                 
                (
                    this.OldKey == other.OldKey ||
                    this.OldKey.Equals(other.OldKey)
                ) &&                 
                (
                    this.NewKey == other.NewKey ||
                    this.NewKey.Equals(other.NewKey)
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
                                   
                hash = hash * 59 + this.Id.GetHashCode();                if (this.OldTable != null)
                {
                    hash = hash * 59 + this.OldTable.GetHashCode();
                }                
                                if (this.NewTable != null)
                {
                    hash = hash * 59 + this.NewTable.GetHashCode();
                }                
                                                   
                hash = hash * 59 + this.OldKey.GetHashCode();                                   
                hash = hash * 59 + this.NewKey.GetHashCode();
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
        public static bool operator ==(ImportMap left, ImportMap right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(ImportMap left, ImportMap right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
