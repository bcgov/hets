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

    public partial class Equipment : IEquatable<Equipment>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public Equipment()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Equipment" /> class.
        /// </summary>
        /// <param name="Id">Primary Key (required).</param>
        /// <param name="LocalArea">LocalArea.</param>
        /// <param name="EquipmentType">EquipmentType.</param>
        /// <param name="DumpTruckDetails">DumpTruckDetails.</param>
        /// <param name="Owner">Owner.</param>
        /// <param name="EquipCd">EquipCd.</param>
        /// <param name="Approval">Approval.</param>
        /// <param name="ApprovedDate">ApprovedDate.</param>
        /// <param name="ReceivedDate">ReceivedDate.</param>
        /// <param name="AddressLine1">Address 1 line of the address..</param>
        /// <param name="AddressLine2">Address 2 line of the address..</param>
        /// <param name="AddressLine3">Address 3 line of the address..</param>
        /// <param name="AddressLine4">Address 4 line of the address..</param>
        /// <param name="City">City.</param>
        /// <param name="Postal">Postal.</param>
        /// <param name="BlockNumber">BlockNumber.</param>
        /// <param name="Comment">Comment.</param>
        /// <param name="CycleHrsWrk">CycleHrsWrk.</param>
        /// <param name="FrozenOut">FrozenOut.</param>
        /// <param name="LastVerifiedDate">LastVerifiedDate.</param>
        /// <param name="Licence">Licence.</param>
        /// <param name="Make">Make.</param>
        /// <param name="Model">Model.</param>
        /// <param name="Year">Year.</param>
        /// <param name="Type">Type.</param>
        /// <param name="NumYears">NumYears.</param>
        /// <param name="Operator">Operator.</param>
        /// <param name="PayRate">PayRate.</param>
        /// <param name="RefuseRate">RefuseRate.</param>
        /// <param name="Seniority">Seniority.</param>
        /// <param name="SerialNum">SerialNum.</param>
        /// <param name="Size">Size.</param>
        /// <param name="ToDate">ToDate.</param>
        /// <param name="Working">Working.</param>
        /// <param name="YearEndReg">YearEndReg.</param>
        /// <param name="PrevRegArea">PrevRegArea.</param>
        /// <param name="YTD">YTD.</param>
        /// <param name="YTD1">YTD1.</param>
        /// <param name="YTD2">YTD2.</param>
        /// <param name="YTD3">YTD3.</param>
        /// <param name="StatusCd">StatusCd.</param>
        /// <param name="ArchiveCd">ArchiveCd.</param>
        /// <param name="ArchiveReason">ArchiveReason.</param>
        /// <param name="ArchiveDate">ArchiveDate.</param>
        /// <param name="DraftBlockNum">DraftBlockNum.</param>
        /// <param name="RegDumpTruck">RegDumpTruck.</param>
        /// <param name="EquipmentAttachments">EquipmentAttachments.</param>
        /// <param name="Notes">Notes.</param>
        /// <param name="Attachments">Attachments.</param>
        /// <param name="History">History.</param>
        /// <param name="SeniorityAudit">SeniorityAudit.</param>
        public Equipment(int Id, LocalArea LocalArea = null, EquipmentType EquipmentType = null, DumpTruck DumpTruckDetails = null, Owner Owner = null, string EquipCd = null, string Approval = null, DateTime? ApprovedDate = null, DateTime? ReceivedDate = null, string AddressLine1 = null, string AddressLine2 = null, string AddressLine3 = null, string AddressLine4 = null, string City = null, string Postal = null, float? BlockNumber = null, string Comment = null, float? CycleHrsWrk = null, string FrozenOut = null, DateTime? LastVerifiedDate = null, string Licence = null, string Make = null, string Model = null, string Year = null, string Type = null, float? NumYears = null, string Operator = null, float? PayRate = null, string RefuseRate = null, float? Seniority = null, string SerialNum = null, string Size = null, DateTime? ToDate = null, string Working = null, string YearEndReg = null, string PrevRegArea = null, float? YTD = null, float? YTD1 = null, float? YTD2 = null, float? YTD3 = null, string StatusCd = null, string ArchiveCd = null, string ArchiveReason = null, DateTime? ArchiveDate = null, float? DraftBlockNum = null, string RegDumpTruck = null, List<EquipmentAttachment> EquipmentAttachments = null, List<Note> Notes = null, List<Attachment> Attachments = null, List<History> History = null, List<SeniorityAudit> SeniorityAudit = null)
        {   
            this.Id = Id;
            this.LocalArea = LocalArea;
            this.EquipmentType = EquipmentType;
            this.DumpTruckDetails = DumpTruckDetails;
            this.Owner = Owner;
            this.EquipCd = EquipCd;
            this.Approval = Approval;
            this.ApprovedDate = ApprovedDate;
            this.ReceivedDate = ReceivedDate;
            this.AddressLine1 = AddressLine1;
            this.AddressLine2 = AddressLine2;
            this.AddressLine3 = AddressLine3;
            this.AddressLine4 = AddressLine4;
            this.City = City;
            this.Postal = Postal;
            this.BlockNumber = BlockNumber;
            this.Comment = Comment;
            this.CycleHrsWrk = CycleHrsWrk;
            this.FrozenOut = FrozenOut;
            this.LastVerifiedDate = LastVerifiedDate;
            this.Licence = Licence;
            this.Make = Make;
            this.Model = Model;
            this.Year = Year;
            this.Type = Type;
            this.NumYears = NumYears;
            this.Operator = Operator;
            this.PayRate = PayRate;
            this.RefuseRate = RefuseRate;
            this.Seniority = Seniority;
            this.SerialNum = SerialNum;
            this.Size = Size;
            this.ToDate = ToDate;
            this.Working = Working;
            this.YearEndReg = YearEndReg;
            this.PrevRegArea = PrevRegArea;
            this.YTD = YTD;
            this.YTD1 = YTD1;
            this.YTD2 = YTD2;
            this.YTD3 = YTD3;
            this.StatusCd = StatusCd;
            this.ArchiveCd = ArchiveCd;
            this.ArchiveReason = ArchiveReason;
            this.ArchiveDate = ArchiveDate;
            this.DraftBlockNum = DraftBlockNum;
            this.RegDumpTruck = RegDumpTruck;
            this.EquipmentAttachments = EquipmentAttachments;
            this.Notes = Notes;
            this.Attachments = Attachments;
            this.History = History;
            this.SeniorityAudit = SeniorityAudit;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        /// <value>Primary Key</value>
        [MetaDataExtension (Description = "Primary Key")]
        public int Id { get; set; }
        
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
        /// Gets or Sets EquipmentType
        /// </summary>
        public EquipmentType EquipmentType { get; set; }
        
        /// <summary>
        /// Foreign key for EquipmentType 
        /// </summary>       
        [ForeignKey("EquipmentType")]
        public int? EquipmentTypeRefId { get; set; }
        
        /// <summary>
        /// Gets or Sets DumpTruckDetails
        /// </summary>
        public DumpTruck DumpTruckDetails { get; set; }
        
        /// <summary>
        /// Foreign key for DumpTruckDetails 
        /// </summary>       
        [ForeignKey("DumpTruckDetails")]
        public int? DumpTruckDetailsRefId { get; set; }
        
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
        /// Gets or Sets EquipCd
        /// </summary>
        [MaxLength(255)]
        
        public string EquipCd { get; set; }
        
        /// <summary>
        /// Gets or Sets Approval
        /// </summary>
        [MaxLength(255)]
        
        public string Approval { get; set; }
        
        /// <summary>
        /// Gets or Sets ApprovedDate
        /// </summary>
        public DateTime? ApprovedDate { get; set; }
        
        /// <summary>
        /// Gets or Sets ReceivedDate
        /// </summary>
        public DateTime? ReceivedDate { get; set; }
        
        /// <summary>
        /// Address 1 line of the address.
        /// </summary>
        /// <value>Address 1 line of the address.</value>
        [MetaDataExtension (Description = "Address 1 line of the address.")]
        [MaxLength(255)]
        
        public string AddressLine1 { get; set; }
        
        /// <summary>
        /// Address 2 line of the address.
        /// </summary>
        /// <value>Address 2 line of the address.</value>
        [MetaDataExtension (Description = "Address 2 line of the address.")]
        [MaxLength(255)]
        
        public string AddressLine2 { get; set; }
        
        /// <summary>
        /// Address 3 line of the address.
        /// </summary>
        /// <value>Address 3 line of the address.</value>
        [MetaDataExtension (Description = "Address 3 line of the address.")]
        [MaxLength(255)]
        
        public string AddressLine3 { get; set; }
        
        /// <summary>
        /// Address 4 line of the address.
        /// </summary>
        /// <value>Address 4 line of the address.</value>
        [MetaDataExtension (Description = "Address 4 line of the address.")]
        [MaxLength(255)]
        
        public string AddressLine4 { get; set; }
        
        /// <summary>
        /// Gets or Sets City
        /// </summary>
        [MaxLength(255)]
        
        public string City { get; set; }
        
        /// <summary>
        /// Gets or Sets Postal
        /// </summary>
        [MaxLength(255)]
        
        public string Postal { get; set; }
        
        /// <summary>
        /// Gets or Sets BlockNumber
        /// </summary>
        public float? BlockNumber { get; set; }
        
        /// <summary>
        /// Gets or Sets Comment
        /// </summary>
        [MaxLength(255)]
        
        public string Comment { get; set; }
        
        /// <summary>
        /// Gets or Sets CycleHrsWrk
        /// </summary>
        public float? CycleHrsWrk { get; set; }
        
        /// <summary>
        /// Gets or Sets FrozenOut
        /// </summary>
        [MaxLength(255)]
        
        public string FrozenOut { get; set; }
        
        /// <summary>
        /// Gets or Sets LastVerifiedDate
        /// </summary>
        public DateTime? LastVerifiedDate { get; set; }
        
        /// <summary>
        /// Gets or Sets Licence
        /// </summary>
        [MaxLength(255)]
        
        public string Licence { get; set; }
        
        /// <summary>
        /// Gets or Sets Make
        /// </summary>
        [MaxLength(255)]
        
        public string Make { get; set; }
        
        /// <summary>
        /// Gets or Sets Model
        /// </summary>
        [MaxLength(255)]
        
        public string Model { get; set; }
        
        /// <summary>
        /// Gets or Sets Year
        /// </summary>
        [MaxLength(255)]
        
        public string Year { get; set; }
        
        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [MaxLength(255)]
        
        public string Type { get; set; }
        
        /// <summary>
        /// Gets or Sets NumYears
        /// </summary>
        public float? NumYears { get; set; }
        
        /// <summary>
        /// Gets or Sets Operator
        /// </summary>
        [MaxLength(255)]
        
        public string Operator { get; set; }
        
        /// <summary>
        /// Gets or Sets PayRate
        /// </summary>
        public float? PayRate { get; set; }
        
        /// <summary>
        /// Gets or Sets RefuseRate
        /// </summary>
        [MaxLength(255)]
        
        public string RefuseRate { get; set; }
        
        /// <summary>
        /// Gets or Sets Seniority
        /// </summary>
        public float? Seniority { get; set; }
        
        /// <summary>
        /// Gets or Sets SerialNum
        /// </summary>
        [MaxLength(255)]
        
        public string SerialNum { get; set; }
        
        /// <summary>
        /// Gets or Sets Size
        /// </summary>
        [MaxLength(255)]
        
        public string Size { get; set; }
        
        /// <summary>
        /// Gets or Sets ToDate
        /// </summary>
        public DateTime? ToDate { get; set; }
        
        /// <summary>
        /// Gets or Sets Working
        /// </summary>
        [MaxLength(255)]
        
        public string Working { get; set; }
        
        /// <summary>
        /// Gets or Sets YearEndReg
        /// </summary>
        [MaxLength(255)]
        
        public string YearEndReg { get; set; }
        
        /// <summary>
        /// Gets or Sets PrevRegArea
        /// </summary>
        [MaxLength(255)]
        
        public string PrevRegArea { get; set; }
        
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
        /// Gets or Sets StatusCd
        /// </summary>
        [MaxLength(255)]
        
        public string StatusCd { get; set; }
        
        /// <summary>
        /// Gets or Sets ArchiveCd
        /// </summary>
        [MaxLength(255)]
        
        public string ArchiveCd { get; set; }
        
        /// <summary>
        /// Gets or Sets ArchiveReason
        /// </summary>
        [MaxLength(255)]
        
        public string ArchiveReason { get; set; }
        
        /// <summary>
        /// Gets or Sets ArchiveDate
        /// </summary>
        public DateTime? ArchiveDate { get; set; }
        
        /// <summary>
        /// Gets or Sets DraftBlockNum
        /// </summary>
        public float? DraftBlockNum { get; set; }
        
        /// <summary>
        /// Gets or Sets RegDumpTruck
        /// </summary>
        [MaxLength(255)]
        
        public string RegDumpTruck { get; set; }
        
        /// <summary>
        /// Gets or Sets EquipmentAttachments
        /// </summary>
        public List<EquipmentAttachment> EquipmentAttachments { get; set; }
        
        /// <summary>
        /// Gets or Sets Notes
        /// </summary>
        public List<Note> Notes { get; set; }
        
        /// <summary>
        /// Gets or Sets Attachments
        /// </summary>
        public List<Attachment> Attachments { get; set; }
        
        /// <summary>
        /// Gets or Sets History
        /// </summary>
        public List<History> History { get; set; }
        
        /// <summary>
        /// Gets or Sets SeniorityAudit
        /// </summary>
        public List<SeniorityAudit> SeniorityAudit { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Equipment {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  LocalArea: ").Append(LocalArea).Append("\n");
            sb.Append("  EquipmentType: ").Append(EquipmentType).Append("\n");
            sb.Append("  DumpTruckDetails: ").Append(DumpTruckDetails).Append("\n");
            sb.Append("  Owner: ").Append(Owner).Append("\n");
            sb.Append("  EquipCd: ").Append(EquipCd).Append("\n");
            sb.Append("  Approval: ").Append(Approval).Append("\n");
            sb.Append("  ApprovedDate: ").Append(ApprovedDate).Append("\n");
            sb.Append("  ReceivedDate: ").Append(ReceivedDate).Append("\n");
            sb.Append("  AddressLine1: ").Append(AddressLine1).Append("\n");
            sb.Append("  AddressLine2: ").Append(AddressLine2).Append("\n");
            sb.Append("  AddressLine3: ").Append(AddressLine3).Append("\n");
            sb.Append("  AddressLine4: ").Append(AddressLine4).Append("\n");
            sb.Append("  City: ").Append(City).Append("\n");
            sb.Append("  Postal: ").Append(Postal).Append("\n");
            sb.Append("  BlockNumber: ").Append(BlockNumber).Append("\n");
            sb.Append("  Comment: ").Append(Comment).Append("\n");
            sb.Append("  CycleHrsWrk: ").Append(CycleHrsWrk).Append("\n");
            sb.Append("  FrozenOut: ").Append(FrozenOut).Append("\n");
            sb.Append("  LastVerifiedDate: ").Append(LastVerifiedDate).Append("\n");
            sb.Append("  Licence: ").Append(Licence).Append("\n");
            sb.Append("  Make: ").Append(Make).Append("\n");
            sb.Append("  Model: ").Append(Model).Append("\n");
            sb.Append("  Year: ").Append(Year).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  NumYears: ").Append(NumYears).Append("\n");
            sb.Append("  Operator: ").Append(Operator).Append("\n");
            sb.Append("  PayRate: ").Append(PayRate).Append("\n");
            sb.Append("  RefuseRate: ").Append(RefuseRate).Append("\n");
            sb.Append("  Seniority: ").Append(Seniority).Append("\n");
            sb.Append("  SerialNum: ").Append(SerialNum).Append("\n");
            sb.Append("  Size: ").Append(Size).Append("\n");
            sb.Append("  ToDate: ").Append(ToDate).Append("\n");
            sb.Append("  Working: ").Append(Working).Append("\n");
            sb.Append("  YearEndReg: ").Append(YearEndReg).Append("\n");
            sb.Append("  PrevRegArea: ").Append(PrevRegArea).Append("\n");
            sb.Append("  YTD: ").Append(YTD).Append("\n");
            sb.Append("  YTD1: ").Append(YTD1).Append("\n");
            sb.Append("  YTD2: ").Append(YTD2).Append("\n");
            sb.Append("  YTD3: ").Append(YTD3).Append("\n");
            sb.Append("  StatusCd: ").Append(StatusCd).Append("\n");
            sb.Append("  ArchiveCd: ").Append(ArchiveCd).Append("\n");
            sb.Append("  ArchiveReason: ").Append(ArchiveReason).Append("\n");
            sb.Append("  ArchiveDate: ").Append(ArchiveDate).Append("\n");
            sb.Append("  DraftBlockNum: ").Append(DraftBlockNum).Append("\n");
            sb.Append("  RegDumpTruck: ").Append(RegDumpTruck).Append("\n");
            sb.Append("  EquipmentAttachments: ").Append(EquipmentAttachments).Append("\n");
            sb.Append("  Notes: ").Append(Notes).Append("\n");
            sb.Append("  Attachments: ").Append(Attachments).Append("\n");
            sb.Append("  History: ").Append(History).Append("\n");
            sb.Append("  SeniorityAudit: ").Append(SeniorityAudit).Append("\n");
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
            return Equals((Equipment)obj);
        }

        /// <summary>
        /// Returns true if Equipment instances are equal
        /// </summary>
        /// <param name="other">Instance of Equipment to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Equipment other)
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
                    this.EquipmentType == other.EquipmentType ||
                    this.EquipmentType != null &&
                    this.EquipmentType.Equals(other.EquipmentType)
                ) &&                 
                (
                    this.DumpTruckDetails == other.DumpTruckDetails ||
                    this.DumpTruckDetails != null &&
                    this.DumpTruckDetails.Equals(other.DumpTruckDetails)
                ) &&                 
                (
                    this.Owner == other.Owner ||
                    this.Owner != null &&
                    this.Owner.Equals(other.Owner)
                ) &&                 
                (
                    this.EquipCd == other.EquipCd ||
                    this.EquipCd != null &&
                    this.EquipCd.Equals(other.EquipCd)
                ) &&                 
                (
                    this.Approval == other.Approval ||
                    this.Approval != null &&
                    this.Approval.Equals(other.Approval)
                ) &&                 
                (
                    this.ApprovedDate == other.ApprovedDate ||
                    this.ApprovedDate != null &&
                    this.ApprovedDate.Equals(other.ApprovedDate)
                ) &&                 
                (
                    this.ReceivedDate == other.ReceivedDate ||
                    this.ReceivedDate != null &&
                    this.ReceivedDate.Equals(other.ReceivedDate)
                ) &&                 
                (
                    this.AddressLine1 == other.AddressLine1 ||
                    this.AddressLine1 != null &&
                    this.AddressLine1.Equals(other.AddressLine1)
                ) &&                 
                (
                    this.AddressLine2 == other.AddressLine2 ||
                    this.AddressLine2 != null &&
                    this.AddressLine2.Equals(other.AddressLine2)
                ) &&                 
                (
                    this.AddressLine3 == other.AddressLine3 ||
                    this.AddressLine3 != null &&
                    this.AddressLine3.Equals(other.AddressLine3)
                ) &&                 
                (
                    this.AddressLine4 == other.AddressLine4 ||
                    this.AddressLine4 != null &&
                    this.AddressLine4.Equals(other.AddressLine4)
                ) &&                 
                (
                    this.City == other.City ||
                    this.City != null &&
                    this.City.Equals(other.City)
                ) &&                 
                (
                    this.Postal == other.Postal ||
                    this.Postal != null &&
                    this.Postal.Equals(other.Postal)
                ) &&                 
                (
                    this.BlockNumber == other.BlockNumber ||
                    this.BlockNumber != null &&
                    this.BlockNumber.Equals(other.BlockNumber)
                ) &&                 
                (
                    this.Comment == other.Comment ||
                    this.Comment != null &&
                    this.Comment.Equals(other.Comment)
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
                    this.LastVerifiedDate == other.LastVerifiedDate ||
                    this.LastVerifiedDate != null &&
                    this.LastVerifiedDate.Equals(other.LastVerifiedDate)
                ) &&                 
                (
                    this.Licence == other.Licence ||
                    this.Licence != null &&
                    this.Licence.Equals(other.Licence)
                ) &&                 
                (
                    this.Make == other.Make ||
                    this.Make != null &&
                    this.Make.Equals(other.Make)
                ) &&                 
                (
                    this.Model == other.Model ||
                    this.Model != null &&
                    this.Model.Equals(other.Model)
                ) &&                 
                (
                    this.Year == other.Year ||
                    this.Year != null &&
                    this.Year.Equals(other.Year)
                ) &&                 
                (
                    this.Type == other.Type ||
                    this.Type != null &&
                    this.Type.Equals(other.Type)
                ) &&                 
                (
                    this.NumYears == other.NumYears ||
                    this.NumYears != null &&
                    this.NumYears.Equals(other.NumYears)
                ) &&                 
                (
                    this.Operator == other.Operator ||
                    this.Operator != null &&
                    this.Operator.Equals(other.Operator)
                ) &&                 
                (
                    this.PayRate == other.PayRate ||
                    this.PayRate != null &&
                    this.PayRate.Equals(other.PayRate)
                ) &&                 
                (
                    this.RefuseRate == other.RefuseRate ||
                    this.RefuseRate != null &&
                    this.RefuseRate.Equals(other.RefuseRate)
                ) &&                 
                (
                    this.Seniority == other.Seniority ||
                    this.Seniority != null &&
                    this.Seniority.Equals(other.Seniority)
                ) &&                 
                (
                    this.SerialNum == other.SerialNum ||
                    this.SerialNum != null &&
                    this.SerialNum.Equals(other.SerialNum)
                ) &&                 
                (
                    this.Size == other.Size ||
                    this.Size != null &&
                    this.Size.Equals(other.Size)
                ) &&                 
                (
                    this.ToDate == other.ToDate ||
                    this.ToDate != null &&
                    this.ToDate.Equals(other.ToDate)
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
                ) &&                 
                (
                    this.PrevRegArea == other.PrevRegArea ||
                    this.PrevRegArea != null &&
                    this.PrevRegArea.Equals(other.PrevRegArea)
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
                    this.StatusCd == other.StatusCd ||
                    this.StatusCd != null &&
                    this.StatusCd.Equals(other.StatusCd)
                ) &&                 
                (
                    this.ArchiveCd == other.ArchiveCd ||
                    this.ArchiveCd != null &&
                    this.ArchiveCd.Equals(other.ArchiveCd)
                ) &&                 
                (
                    this.ArchiveReason == other.ArchiveReason ||
                    this.ArchiveReason != null &&
                    this.ArchiveReason.Equals(other.ArchiveReason)
                ) &&                 
                (
                    this.ArchiveDate == other.ArchiveDate ||
                    this.ArchiveDate != null &&
                    this.ArchiveDate.Equals(other.ArchiveDate)
                ) &&                 
                (
                    this.DraftBlockNum == other.DraftBlockNum ||
                    this.DraftBlockNum != null &&
                    this.DraftBlockNum.Equals(other.DraftBlockNum)
                ) &&                 
                (
                    this.RegDumpTruck == other.RegDumpTruck ||
                    this.RegDumpTruck != null &&
                    this.RegDumpTruck.Equals(other.RegDumpTruck)
                ) && 
                (
                    this.EquipmentAttachments == other.EquipmentAttachments ||
                    this.EquipmentAttachments != null &&
                    this.EquipmentAttachments.SequenceEqual(other.EquipmentAttachments)
                ) && 
                (
                    this.Notes == other.Notes ||
                    this.Notes != null &&
                    this.Notes.SequenceEqual(other.Notes)
                ) && 
                (
                    this.Attachments == other.Attachments ||
                    this.Attachments != null &&
                    this.Attachments.SequenceEqual(other.Attachments)
                ) && 
                (
                    this.History == other.History ||
                    this.History != null &&
                    this.History.SequenceEqual(other.History)
                ) && 
                (
                    this.SeniorityAudit == other.SeniorityAudit ||
                    this.SeniorityAudit != null &&
                    this.SeniorityAudit.SequenceEqual(other.SeniorityAudit)
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
                }                   
                if (this.EquipmentType != null)
                {
                    hash = hash * 59 + this.EquipmentType.GetHashCode();
                }                   
                if (this.DumpTruckDetails != null)
                {
                    hash = hash * 59 + this.DumpTruckDetails.GetHashCode();
                }                   
                if (this.Owner != null)
                {
                    hash = hash * 59 + this.Owner.GetHashCode();
                }                if (this.EquipCd != null)
                {
                    hash = hash * 59 + this.EquipCd.GetHashCode();
                }                
                                if (this.Approval != null)
                {
                    hash = hash * 59 + this.Approval.GetHashCode();
                }                
                                if (this.ApprovedDate != null)
                {
                    hash = hash * 59 + this.ApprovedDate.GetHashCode();
                }                
                                if (this.ReceivedDate != null)
                {
                    hash = hash * 59 + this.ReceivedDate.GetHashCode();
                }                
                                if (this.AddressLine1 != null)
                {
                    hash = hash * 59 + this.AddressLine1.GetHashCode();
                }                
                                if (this.AddressLine2 != null)
                {
                    hash = hash * 59 + this.AddressLine2.GetHashCode();
                }                
                                if (this.AddressLine3 != null)
                {
                    hash = hash * 59 + this.AddressLine3.GetHashCode();
                }                
                                if (this.AddressLine4 != null)
                {
                    hash = hash * 59 + this.AddressLine4.GetHashCode();
                }                
                                if (this.City != null)
                {
                    hash = hash * 59 + this.City.GetHashCode();
                }                
                                if (this.Postal != null)
                {
                    hash = hash * 59 + this.Postal.GetHashCode();
                }                
                                if (this.BlockNumber != null)
                {
                    hash = hash * 59 + this.BlockNumber.GetHashCode();
                }                
                                if (this.Comment != null)
                {
                    hash = hash * 59 + this.Comment.GetHashCode();
                }                
                                if (this.CycleHrsWrk != null)
                {
                    hash = hash * 59 + this.CycleHrsWrk.GetHashCode();
                }                
                                if (this.FrozenOut != null)
                {
                    hash = hash * 59 + this.FrozenOut.GetHashCode();
                }                
                                if (this.LastVerifiedDate != null)
                {
                    hash = hash * 59 + this.LastVerifiedDate.GetHashCode();
                }                
                                if (this.Licence != null)
                {
                    hash = hash * 59 + this.Licence.GetHashCode();
                }                
                                if (this.Make != null)
                {
                    hash = hash * 59 + this.Make.GetHashCode();
                }                
                                if (this.Model != null)
                {
                    hash = hash * 59 + this.Model.GetHashCode();
                }                
                                if (this.Year != null)
                {
                    hash = hash * 59 + this.Year.GetHashCode();
                }                
                                if (this.Type != null)
                {
                    hash = hash * 59 + this.Type.GetHashCode();
                }                
                                if (this.NumYears != null)
                {
                    hash = hash * 59 + this.NumYears.GetHashCode();
                }                
                                if (this.Operator != null)
                {
                    hash = hash * 59 + this.Operator.GetHashCode();
                }                
                                if (this.PayRate != null)
                {
                    hash = hash * 59 + this.PayRate.GetHashCode();
                }                
                                if (this.RefuseRate != null)
                {
                    hash = hash * 59 + this.RefuseRate.GetHashCode();
                }                
                                if (this.Seniority != null)
                {
                    hash = hash * 59 + this.Seniority.GetHashCode();
                }                
                                if (this.SerialNum != null)
                {
                    hash = hash * 59 + this.SerialNum.GetHashCode();
                }                
                                if (this.Size != null)
                {
                    hash = hash * 59 + this.Size.GetHashCode();
                }                
                                if (this.ToDate != null)
                {
                    hash = hash * 59 + this.ToDate.GetHashCode();
                }                
                                if (this.Working != null)
                {
                    hash = hash * 59 + this.Working.GetHashCode();
                }                
                                if (this.YearEndReg != null)
                {
                    hash = hash * 59 + this.YearEndReg.GetHashCode();
                }                
                                if (this.PrevRegArea != null)
                {
                    hash = hash * 59 + this.PrevRegArea.GetHashCode();
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
                                if (this.StatusCd != null)
                {
                    hash = hash * 59 + this.StatusCd.GetHashCode();
                }                
                                if (this.ArchiveCd != null)
                {
                    hash = hash * 59 + this.ArchiveCd.GetHashCode();
                }                
                                if (this.ArchiveReason != null)
                {
                    hash = hash * 59 + this.ArchiveReason.GetHashCode();
                }                
                                if (this.ArchiveDate != null)
                {
                    hash = hash * 59 + this.ArchiveDate.GetHashCode();
                }                
                                if (this.DraftBlockNum != null)
                {
                    hash = hash * 59 + this.DraftBlockNum.GetHashCode();
                }                
                                if (this.RegDumpTruck != null)
                {
                    hash = hash * 59 + this.RegDumpTruck.GetHashCode();
                }                
                                   
                if (this.EquipmentAttachments != null)
                {
                    hash = hash * 59 + this.EquipmentAttachments.GetHashCode();
                }                   
                if (this.Notes != null)
                {
                    hash = hash * 59 + this.Notes.GetHashCode();
                }                   
                if (this.Attachments != null)
                {
                    hash = hash * 59 + this.Attachments.GetHashCode();
                }                   
                if (this.History != null)
                {
                    hash = hash * 59 + this.History.GetHashCode();
                }                   
                if (this.SeniorityAudit != null)
                {
                    hash = hash * 59 + this.SeniorityAudit.GetHashCode();
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
        public static bool operator ==(Equipment left, Equipment right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Equipment left, Equipment right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
