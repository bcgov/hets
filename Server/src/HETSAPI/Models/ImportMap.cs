using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// Import Map Database Model
    /// </summary>
    public sealed class ImportMap : AuditableEntity, IEquatable<ImportMap>
    {
        /// <summary>
        /// IMport Map Database Model Constructor (required by entity framework)
        /// </summary>
        public ImportMap()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportMap" /> class.
        /// </summary>
        /// <param name="id">A system generated unique identifier for the ImportMap (required).</param>
        /// <param name="oldTable">Table name in old system (required).</param>
        /// <param name="newTable">Table name in new system. (required).</param>
        /// <param name="oldKey">Old primary key for record (required).</param>
        /// <param name="newKey">New primary key for record (required).</param>
        public ImportMap(int id, string oldTable, string newTable, string oldKey, int newKey)
        {
            Id = id;
            OldTable = oldTable;
            NewTable = newTable;
            OldKey = oldKey;
            NewKey = newKey;
        }

        /// <summary>
        /// A system generated unique identifier for the ImportMap
        /// </summary>
        /// <value>A system generated unique identifier for the ImportMap</value>
        [MetaData (Description = "A system generated unique identifier for the ImportMap")]
        public int Id { get; set; }

        /// <summary>
        /// Table name in old system
        /// </summary>
        /// <value>Table name in old system</value>
        [MetaData (Description = "Table name in old system")]
        public string OldTable { get; set; }

        /// <summary>
        /// Table name in new system.
        /// </summary>
        /// <value>Table name in new system.</value>
        [MetaData (Description = "Table name in new system.")]
        public string NewTable { get; set; }

        /// <summary>
        /// Old primary key for record
        /// </summary>
        /// <value>Old primary key for record</value>
        [MetaData (Description = "Old primary key for record")]
        [MaxLength(250)]
        public string OldKey { get; set; }

        /// <summary>
        /// New primary key for record
        /// </summary>
        /// <value>New primary key for record</value>
        [MetaData (Description = "New primary key for record")]
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
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            return obj.GetType() == GetType() && Equals((ImportMap)obj);
        }

        /// <summary>
        /// Returns true if ImportMap instances are equal
        /// </summary>
        /// <param name="other">Instance of ImportMap to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ImportMap other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&
                (
                    OldTable == other.OldTable ||
                    OldTable != null &&
                    OldTable.Equals(other.OldTable)
                ) &&
                (
                    NewTable == other.NewTable ||
                    NewTable != null &&
                    NewTable.Equals(other.NewTable)
                ) &&
                (
                    OldKey == other.OldKey ||
                    OldKey != null &&
                    OldKey.Equals(other.OldKey)
                ) &&
                (
                    NewKey == other.NewKey ||
                    NewKey.Equals(other.NewKey)
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

                if (OldTable != null)
                {
                    hash = hash * 59 + OldTable.GetHashCode();
                }

                if (NewTable != null)
                {
                    hash = hash * 59 + NewTable.GetHashCode();
                }

                if (OldKey != null)
                {
                    hash = hash * 59 + OldKey.GetHashCode();
                }

                hash = hash * 59 + NewKey.GetHashCode();

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
