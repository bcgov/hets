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
    /// 
    /// </summary>

    public partial class RotationListBlock : IEquatable<RotationListBlock>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public RotationListBlock()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RotationListBlock" /> class.
        /// </summary>
        /// <param name="Id">Primary Key (required).</param>
        /// <param name="RotationList">RotationList.</param>
        /// <param name="BlockNum">The block number. 1 for Primary, 2 for Secondary, 3 for Open.</param>
        /// <param name="CycleNum">CycleNum.</param>
        /// <param name="MaxCycle">MaxCycle.</param>
        /// <param name="LastHiredEquipment">LastHiredEquipment.</param>
        /// <param name="StartCycleEquipment">StartCycleEquipment.</param>
        /// <param name="Moved">Moved.</param>
        /// <param name="StartWasZero">StartWasZero.</param>
        /// <param name="RotatedBlock">RotatedBlock.</param>
        /// <param name="BlockName">BlockName.</param>
        /// <param name="Closed">Closed.</param>
        /// <param name="ClosedComments">ClosedComments.</param>
        /// <param name="ClosedDate">ClosedDate.</param>
        /// <param name="ClosedBy">ClosedBy.</param>
        /// <param name="ReservedDate">ReservedDate.</param>
        /// <param name="ReservedBy">ReservedBy.</param>
        public RotationListBlock(int Id, RotationList RotationList = null, int? BlockNum = null, float? CycleNum = null, float? MaxCycle = null, Equipment LastHiredEquipment = null, Equipment StartCycleEquipment = null, string Moved = null, string StartWasZero = null, int? RotatedBlock = null, string BlockName = null, string Closed = null, string ClosedComments = null, DateTime? ClosedDate = null, string ClosedBy = null, DateTime? ReservedDate = null, string ReservedBy = null)
        {   
            this.Id = Id;
            this.RotationList = RotationList;
            this.BlockNum = BlockNum;
            this.CycleNum = CycleNum;
            this.MaxCycle = MaxCycle;
            this.LastHiredEquipment = LastHiredEquipment;
            this.StartCycleEquipment = StartCycleEquipment;
            this.Moved = Moved;
            this.StartWasZero = StartWasZero;
            this.RotatedBlock = RotatedBlock;
            this.BlockName = BlockName;
            this.Closed = Closed;
            this.ClosedComments = ClosedComments;
            this.ClosedDate = ClosedDate;
            this.ClosedBy = ClosedBy;
            this.ReservedDate = ReservedDate;
            this.ReservedBy = ReservedBy;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        /// <value>Primary Key</value>
        [MetaDataExtension (Description = "Primary Key")]
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or Sets RotationList
        /// </summary>
        public RotationList RotationList { get; set; }
        
        /// <summary>
        /// Foreign key for RotationList 
        /// </summary>       
        [ForeignKey("RotationList")]
        public int? RotationListRefId { get; set; }
        
        /// <summary>
        /// The block number. 1 for Primary, 2 for Secondary, 3 for Open
        /// </summary>
        /// <value>The block number. 1 for Primary, 2 for Secondary, 3 for Open</value>
        [MetaDataExtension (Description = "The block number. 1 for Primary, 2 for Secondary, 3 for Open")]
        public int? BlockNum { get; set; }
        
        /// <summary>
        /// Gets or Sets CycleNum
        /// </summary>
        public float? CycleNum { get; set; }
        
        /// <summary>
        /// Gets or Sets MaxCycle
        /// </summary>
        public float? MaxCycle { get; set; }
        
        /// <summary>
        /// Gets or Sets LastHiredEquipment
        /// </summary>
        public Equipment LastHiredEquipment { get; set; }
        
        /// <summary>
        /// Foreign key for LastHiredEquipment 
        /// </summary>       
        [ForeignKey("LastHiredEquipment")]
        public int? LastHiredEquipmentRefId { get; set; }
        
        /// <summary>
        /// Gets or Sets StartCycleEquipment
        /// </summary>
        public Equipment StartCycleEquipment { get; set; }
        
        /// <summary>
        /// Foreign key for StartCycleEquipment 
        /// </summary>       
        [ForeignKey("StartCycleEquipment")]
        public int? StartCycleEquipmentRefId { get; set; }
        
        /// <summary>
        /// Gets or Sets Moved
        /// </summary>
        [MaxLength(255)]
        
        public string Moved { get; set; }
        
        /// <summary>
        /// Gets or Sets StartWasZero
        /// </summary>
        [MaxLength(255)]
        
        public string StartWasZero { get; set; }
        
        /// <summary>
        /// Gets or Sets RotatedBlock
        /// </summary>
        public int? RotatedBlock { get; set; }
        
        /// <summary>
        /// Gets or Sets BlockName
        /// </summary>
        public string BlockName { get; set; }
        
        /// <summary>
        /// Gets or Sets Closed
        /// </summary>
        [MaxLength(255)]
        
        public string Closed { get; set; }
        
        /// <summary>
        /// Gets or Sets ClosedComments
        /// </summary>
        [MaxLength(2048)]
        
        public string ClosedComments { get; set; }
        
        /// <summary>
        /// Gets or Sets ClosedDate
        /// </summary>
        public DateTime? ClosedDate { get; set; }
        
        /// <summary>
        /// Gets or Sets ClosedBy
        /// </summary>
        public string ClosedBy { get; set; }
        
        /// <summary>
        /// Gets or Sets ReservedDate
        /// </summary>
        public DateTime? ReservedDate { get; set; }
        
        /// <summary>
        /// Gets or Sets ReservedBy
        /// </summary>
        [MaxLength(255)]
        
        public string ReservedBy { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class RotationListBlock {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  RotationList: ").Append(RotationList).Append("\n");
            sb.Append("  BlockNum: ").Append(BlockNum).Append("\n");
            sb.Append("  CycleNum: ").Append(CycleNum).Append("\n");
            sb.Append("  MaxCycle: ").Append(MaxCycle).Append("\n");
            sb.Append("  LastHiredEquipment: ").Append(LastHiredEquipment).Append("\n");
            sb.Append("  StartCycleEquipment: ").Append(StartCycleEquipment).Append("\n");
            sb.Append("  Moved: ").Append(Moved).Append("\n");
            sb.Append("  StartWasZero: ").Append(StartWasZero).Append("\n");
            sb.Append("  RotatedBlock: ").Append(RotatedBlock).Append("\n");
            sb.Append("  BlockName: ").Append(BlockName).Append("\n");
            sb.Append("  Closed: ").Append(Closed).Append("\n");
            sb.Append("  ClosedComments: ").Append(ClosedComments).Append("\n");
            sb.Append("  ClosedDate: ").Append(ClosedDate).Append("\n");
            sb.Append("  ClosedBy: ").Append(ClosedBy).Append("\n");
            sb.Append("  ReservedDate: ").Append(ReservedDate).Append("\n");
            sb.Append("  ReservedBy: ").Append(ReservedBy).Append("\n");
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
            return Equals((RotationListBlock)obj);
        }

        /// <summary>
        /// Returns true if RotationListBlock instances are equal
        /// </summary>
        /// <param name="other">Instance of RotationListBlock to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RotationListBlock other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.RotationList == other.RotationList ||
                    this.RotationList != null &&
                    this.RotationList.Equals(other.RotationList)
                ) &&                 
                (
                    this.BlockNum == other.BlockNum ||
                    this.BlockNum != null &&
                    this.BlockNum.Equals(other.BlockNum)
                ) &&                 
                (
                    this.CycleNum == other.CycleNum ||
                    this.CycleNum != null &&
                    this.CycleNum.Equals(other.CycleNum)
                ) &&                 
                (
                    this.MaxCycle == other.MaxCycle ||
                    this.MaxCycle != null &&
                    this.MaxCycle.Equals(other.MaxCycle)
                ) &&                 
                (
                    this.LastHiredEquipment == other.LastHiredEquipment ||
                    this.LastHiredEquipment != null &&
                    this.LastHiredEquipment.Equals(other.LastHiredEquipment)
                ) &&                 
                (
                    this.StartCycleEquipment == other.StartCycleEquipment ||
                    this.StartCycleEquipment != null &&
                    this.StartCycleEquipment.Equals(other.StartCycleEquipment)
                ) &&                 
                (
                    this.Moved == other.Moved ||
                    this.Moved != null &&
                    this.Moved.Equals(other.Moved)
                ) &&                 
                (
                    this.StartWasZero == other.StartWasZero ||
                    this.StartWasZero != null &&
                    this.StartWasZero.Equals(other.StartWasZero)
                ) &&                 
                (
                    this.RotatedBlock == other.RotatedBlock ||
                    this.RotatedBlock != null &&
                    this.RotatedBlock.Equals(other.RotatedBlock)
                ) &&                 
                (
                    this.BlockName == other.BlockName ||
                    this.BlockName != null &&
                    this.BlockName.Equals(other.BlockName)
                ) &&                 
                (
                    this.Closed == other.Closed ||
                    this.Closed != null &&
                    this.Closed.Equals(other.Closed)
                ) &&                 
                (
                    this.ClosedComments == other.ClosedComments ||
                    this.ClosedComments != null &&
                    this.ClosedComments.Equals(other.ClosedComments)
                ) &&                 
                (
                    this.ClosedDate == other.ClosedDate ||
                    this.ClosedDate != null &&
                    this.ClosedDate.Equals(other.ClosedDate)
                ) &&                 
                (
                    this.ClosedBy == other.ClosedBy ||
                    this.ClosedBy != null &&
                    this.ClosedBy.Equals(other.ClosedBy)
                ) &&                 
                (
                    this.ReservedDate == other.ReservedDate ||
                    this.ReservedDate != null &&
                    this.ReservedDate.Equals(other.ReservedDate)
                ) &&                 
                (
                    this.ReservedBy == other.ReservedBy ||
                    this.ReservedBy != null &&
                    this.ReservedBy.Equals(other.ReservedBy)
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
                if (this.RotationList != null)
                {
                    hash = hash * 59 + this.RotationList.GetHashCode();
                }                if (this.BlockNum != null)
                {
                    hash = hash * 59 + this.BlockNum.GetHashCode();
                }                
                                if (this.CycleNum != null)
                {
                    hash = hash * 59 + this.CycleNum.GetHashCode();
                }                
                                if (this.MaxCycle != null)
                {
                    hash = hash * 59 + this.MaxCycle.GetHashCode();
                }                
                                   
                if (this.LastHiredEquipment != null)
                {
                    hash = hash * 59 + this.LastHiredEquipment.GetHashCode();
                }                   
                if (this.StartCycleEquipment != null)
                {
                    hash = hash * 59 + this.StartCycleEquipment.GetHashCode();
                }                if (this.Moved != null)
                {
                    hash = hash * 59 + this.Moved.GetHashCode();
                }                
                                if (this.StartWasZero != null)
                {
                    hash = hash * 59 + this.StartWasZero.GetHashCode();
                }                
                                if (this.RotatedBlock != null)
                {
                    hash = hash * 59 + this.RotatedBlock.GetHashCode();
                }                
                                if (this.BlockName != null)
                {
                    hash = hash * 59 + this.BlockName.GetHashCode();
                }                
                                if (this.Closed != null)
                {
                    hash = hash * 59 + this.Closed.GetHashCode();
                }                
                                if (this.ClosedComments != null)
                {
                    hash = hash * 59 + this.ClosedComments.GetHashCode();
                }                
                                if (this.ClosedDate != null)
                {
                    hash = hash * 59 + this.ClosedDate.GetHashCode();
                }                
                                if (this.ClosedBy != null)
                {
                    hash = hash * 59 + this.ClosedBy.GetHashCode();
                }                
                                if (this.ReservedDate != null)
                {
                    hash = hash * 59 + this.ReservedDate.GetHashCode();
                }                
                                if (this.ReservedBy != null)
                {
                    hash = hash * 59 + this.ReservedBy.GetHashCode();
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
        public static bool operator ==(RotationListBlock left, RotationListBlock right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(RotationListBlock left, RotationListBlock right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
