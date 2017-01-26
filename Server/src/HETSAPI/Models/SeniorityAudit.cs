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

namespace HETSAPI.Models
{
    /// <summary>
    /// 
    /// </summary>

    public partial class SeniorityAudit : IEquatable<SeniorityAudit>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public SeniorityAudit()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeniorityAudit" /> class.
        /// </summary>
        /// <param name="Id">Primary Key (required).</param>
        /// <param name="GeneratedTime">GeneratedTime.</param>
        /// <param name="LocalArea">LocalArea.</param>
        /// <param name="Equipment">Equipment.</param>
        /// <param name="BlockNum">BlockNum.</param>
        /// <param name="EquipCd">EquipCd.</param>
        /// <param name="Owner">Owner.</param>
        /// <param name="OwnerName">OwnerName.</param>
        /// <param name="Seniority">Seniority.</param>
        /// <param name="YTD">YTD.</param>
        /// <param name="YTD1">YTD1.</param>
        /// <param name="YTD2">YTD2.</param>
        /// <param name="YTD3">YTD3.</param>
        /// <param name="CycleHrsWrk">CycleHrsWrk.</param>
        /// <param name="FrozenOut">FrozenOut.</param>
        /// <param name="Project">Project.</param>
        /// <param name="Working">Working.</param>
        /// <param name="YearEndReg">YearEndReg.</param>
        public SeniorityAudit(int Id, DateTime? GeneratedTime = null, LocalArea LocalArea = null, Equipment Equipment = null, float? BlockNum = null, string EquipCd = null, Owner Owner = null, string OwnerName = null, float? Seniority = null, float? YTD = null, float? YTD1 = null, float? YTD2 = null, float? YTD3 = null, float? CycleHrsWrk = null, string FrozenOut = null, Project Project = null, string Working = null, string YearEndReg = null)
        {   
            this.Id = Id;
            this.GeneratedTime = GeneratedTime;
            this.LocalArea = LocalArea;
            this.Equipment = Equipment;
            this.BlockNum = BlockNum;
            this.EquipCd = EquipCd;
            this.Owner = Owner;
            this.OwnerName = OwnerName;
            this.Seniority = Seniority;
            this.YTD = YTD;
            this.YTD1 = YTD1;
            this.YTD2 = YTD2;
            this.YTD3 = YTD3;
            this.CycleHrsWrk = CycleHrsWrk;
            this.FrozenOut = FrozenOut;
            this.Project = Project;
            this.Working = Working;
            this.YearEndReg = YearEndReg;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        /// <value>Primary Key</value>
        [MetaDataExtension (Description = "Primary Key")]
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or Sets GeneratedTime
        /// </summary>
        public DateTime? GeneratedTime { get; set; }
        
        /// <summary>
        /// Gets or Sets LocalArea
        /// </summary>
        public LocalArea LocalArea { get; set; }
        
        /// <summary>
        /// Foreign key for LocalArea 
        /// </summary>       
        [ForeignKey("LocalArea")]
        public int? LocalAreaRefId { get; set; }
        
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
        /// Gets or Sets BlockNum
        /// </summary>
        public float? BlockNum { get; set; }
        
        /// <summary>
        /// Gets or Sets EquipCd
        /// </summary>
        public string EquipCd { get; set; }
        
        /// <summary>
        /// Gets or Sets Owner
        /// </summary>
        public Owner Owner { get; set; }
        
        /// <summary>
        /// Foreign key for Owner 
        /// </summary>       
        [ForeignKey("Owner")]
        public int? OwnerRefId { get; set; }
        
        /// <summary>
        /// Gets or Sets OwnerName
        /// </summary>
        public string OwnerName { get; set; }
        
        /// <summary>
        /// Gets or Sets Seniority
        /// </summary>
        public float? Seniority { get; set; }
        
        /// <summary>
        /// Gets or Sets YTD
        /// </summary>
        public float? YTD { get; set; }
        
        /// <summary>
        /// Gets or Sets YTD1
        /// </summary>
        public float? YTD1 { get; set; }
        
        /// <summary>
        /// Gets or Sets YTD2
        /// </summary>
        public float? YTD2 { get; set; }
        
        /// <summary>
        /// Gets or Sets YTD3
        /// </summary>
        public float? YTD3 { get; set; }
        
        /// <summary>
        /// Gets or Sets CycleHrsWrk
        /// </summary>
        public float? CycleHrsWrk { get; set; }
        
        /// <summary>
        /// Gets or Sets FrozenOut
        /// </summary>
        public string FrozenOut { get; set; }
        
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
        /// Gets or Sets Working
        /// </summary>
        public string Working { get; set; }
        
        /// <summary>
        /// Gets or Sets YearEndReg
        /// </summary>
        public string YearEndReg { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class SeniorityAudit {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  GeneratedTime: ").Append(GeneratedTime).Append("\n");
            sb.Append("  LocalArea: ").Append(LocalArea).Append("\n");
            sb.Append("  Equipment: ").Append(Equipment).Append("\n");
            sb.Append("  BlockNum: ").Append(BlockNum).Append("\n");
            sb.Append("  EquipCd: ").Append(EquipCd).Append("\n");
            sb.Append("  Owner: ").Append(Owner).Append("\n");
            sb.Append("  OwnerName: ").Append(OwnerName).Append("\n");
            sb.Append("  Seniority: ").Append(Seniority).Append("\n");
            sb.Append("  YTD: ").Append(YTD).Append("\n");
            sb.Append("  YTD1: ").Append(YTD1).Append("\n");
            sb.Append("  YTD2: ").Append(YTD2).Append("\n");
            sb.Append("  YTD3: ").Append(YTD3).Append("\n");
            sb.Append("  CycleHrsWrk: ").Append(CycleHrsWrk).Append("\n");
            sb.Append("  FrozenOut: ").Append(FrozenOut).Append("\n");
            sb.Append("  Project: ").Append(Project).Append("\n");
            sb.Append("  Working: ").Append(Working).Append("\n");
            sb.Append("  YearEndReg: ").Append(YearEndReg).Append("\n");
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
            return Equals((SeniorityAudit)obj);
        }

        /// <summary>
        /// Returns true if SeniorityAudit instances are equal
        /// </summary>
        /// <param name="other">Instance of SeniorityAudit to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(SeniorityAudit other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.GeneratedTime == other.GeneratedTime ||
                    this.GeneratedTime != null &&
                    this.GeneratedTime.Equals(other.GeneratedTime)
                ) &&                 
                (
                    this.LocalArea == other.LocalArea ||
                    this.LocalArea != null &&
                    this.LocalArea.Equals(other.LocalArea)
                ) &&                 
                (
                    this.Equipment == other.Equipment ||
                    this.Equipment != null &&
                    this.Equipment.Equals(other.Equipment)
                ) &&                 
                (
                    this.BlockNum == other.BlockNum ||
                    this.BlockNum != null &&
                    this.BlockNum.Equals(other.BlockNum)
                ) &&                 
                (
                    this.EquipCd == other.EquipCd ||
                    this.EquipCd != null &&
                    this.EquipCd.Equals(other.EquipCd)
                ) &&                 
                (
                    this.Owner == other.Owner ||
                    this.Owner != null &&
                    this.Owner.Equals(other.Owner)
                ) &&                 
                (
                    this.OwnerName == other.OwnerName ||
                    this.OwnerName != null &&
                    this.OwnerName.Equals(other.OwnerName)
                ) &&                 
                (
                    this.Seniority == other.Seniority ||
                    this.Seniority != null &&
                    this.Seniority.Equals(other.Seniority)
                ) &&                 
                (
                    this.YTD == other.YTD ||
                    this.YTD != null &&
                    this.YTD.Equals(other.YTD)
                ) &&                 
                (
                    this.YTD1 == other.YTD1 ||
                    this.YTD1 != null &&
                    this.YTD1.Equals(other.YTD1)
                ) &&                 
                (
                    this.YTD2 == other.YTD2 ||
                    this.YTD2 != null &&
                    this.YTD2.Equals(other.YTD2)
                ) &&                 
                (
                    this.YTD3 == other.YTD3 ||
                    this.YTD3 != null &&
                    this.YTD3.Equals(other.YTD3)
                ) &&                 
                (
                    this.CycleHrsWrk == other.CycleHrsWrk ||
                    this.CycleHrsWrk != null &&
                    this.CycleHrsWrk.Equals(other.CycleHrsWrk)
                ) &&                 
                (
                    this.FrozenOut == other.FrozenOut ||
                    this.FrozenOut != null &&
                    this.FrozenOut.Equals(other.FrozenOut)
                ) &&                 
                (
                    this.Project == other.Project ||
                    this.Project != null &&
                    this.Project.Equals(other.Project)
                ) &&                 
                (
                    this.Working == other.Working ||
                    this.Working != null &&
                    this.Working.Equals(other.Working)
                ) &&                 
                (
                    this.YearEndReg == other.YearEndReg ||
                    this.YearEndReg != null &&
                    this.YearEndReg.Equals(other.YearEndReg)
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
                                   
                hash = hash * 59 + this.Id.GetHashCode();                if (this.GeneratedTime != null)
                {
                    hash = hash * 59 + this.GeneratedTime.GetHashCode();
                }                
                                   
                if (this.LocalArea != null)
                {
                    hash = hash * 59 + this.LocalArea.GetHashCode();
                }                   
                if (this.Equipment != null)
                {
                    hash = hash * 59 + this.Equipment.GetHashCode();
                }                if (this.BlockNum != null)
                {
                    hash = hash * 59 + this.BlockNum.GetHashCode();
                }                
                                if (this.EquipCd != null)
                {
                    hash = hash * 59 + this.EquipCd.GetHashCode();
                }                
                                   
                if (this.Owner != null)
                {
                    hash = hash * 59 + this.Owner.GetHashCode();
                }                if (this.OwnerName != null)
                {
                    hash = hash * 59 + this.OwnerName.GetHashCode();
                }                
                                if (this.Seniority != null)
                {
                    hash = hash * 59 + this.Seniority.GetHashCode();
                }                
                                if (this.YTD != null)
                {
                    hash = hash * 59 + this.YTD.GetHashCode();
                }                
                                if (this.YTD1 != null)
                {
                    hash = hash * 59 + this.YTD1.GetHashCode();
                }                
                                if (this.YTD2 != null)
                {
                    hash = hash * 59 + this.YTD2.GetHashCode();
                }                
                                if (this.YTD3 != null)
                {
                    hash = hash * 59 + this.YTD3.GetHashCode();
                }                
                                if (this.CycleHrsWrk != null)
                {
                    hash = hash * 59 + this.CycleHrsWrk.GetHashCode();
                }                
                                if (this.FrozenOut != null)
                {
                    hash = hash * 59 + this.FrozenOut.GetHashCode();
                }                
                                   
                if (this.Project != null)
                {
                    hash = hash * 59 + this.Project.GetHashCode();
                }                if (this.Working != null)
                {
                    hash = hash * 59 + this.Working.GetHashCode();
                }                
                                if (this.YearEndReg != null)
                {
                    hash = hash * 59 + this.YearEndReg.GetHashCode();
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
        public static bool operator ==(SeniorityAudit left, SeniorityAudit right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(SeniorityAudit left, SeniorityAudit right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
