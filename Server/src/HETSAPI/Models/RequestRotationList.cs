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
    /// An eligible piece of equipment for a request and a tracking of the hire offer and response process related to a request for that piece of equipment. Includes a link from the equipment to a Rental Agreement if the equipment was hired to satisfy a part of the request.
    /// </summary>
        [MetaDataExtension (Description = "An eligible piece of equipment for a request and a tracking of the hire offer and response process related to a request for that piece of equipment. Includes a link from the equipment to a Rental Agreement if the equipment was hired to satisfy a part of the request.")]

    public partial class RequestRotationList : IEquatable<RequestRotationList>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public RequestRotationList()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestRotationList" /> class.
        /// </summary>
        /// <param name="Id">A system-generated unique identifier for a RequestRotationList (required).</param>
        /// <param name="Request">Request.</param>
        /// <param name="RotationListSortOrder">The sort order of the piece of equipment on the rotaton list at the time the request was created. This is the order the equipment will be offered the available work..</param>
        /// <param name="Equipment">Equipment.</param>
        /// <param name="RentalAgreement">The rental agreement (if any) created for an accepted hire offer..</param>
        /// <param name="IsForceHire">True if the HETS Clerk designated the hire of this equipment as being a Forced Hire. A Force Hire implies that the usual seniority rules for hiring are bypassed because of special circumstances related to the hire - e.g. a the hire requires an attachment only one piece of equipment has..</param>
        /// <param name="WasAsked">True if the HETS Clerk contacted the equipment owner and asked to hire the piece of equipment..</param>
        /// <param name="AskedDateTime">The Date-Time the HETS clerk contacted the equipment owner and asked to hire the piece of equipment..</param>
        /// <param name="OfferResponse">The response to the offer to hire. Null prior to receiving a response; a string after with the response - likely just Yes or No.</param>
        /// <param name="RefuseReason">An optional reason given by the equipment owner for refusing the offer..</param>
        /// <param name="Note">An optional general note about the offer..</param>
        public RequestRotationList(int Id, Request Request = null, int? RotationListSortOrder = null, Equipment Equipment = null, RentalAgreement RentalAgreement = null, bool? IsForceHire = null, bool? WasAsked = null, DateTime? AskedDateTime = null, string OfferResponse = null, string RefuseReason = null, string Note = null)
        {   
            this.Id = Id;
            this.Request = Request;
            this.RotationListSortOrder = RotationListSortOrder;
            this.Equipment = Equipment;
            this.RentalAgreement = RentalAgreement;
            this.IsForceHire = IsForceHire;
            this.WasAsked = WasAsked;
            this.AskedDateTime = AskedDateTime;
            this.OfferResponse = OfferResponse;
            this.RefuseReason = RefuseReason;
            this.Note = Note;
        }

        /// <summary>
        /// A system-generated unique identifier for a RequestRotationList
        /// </summary>
        /// <value>A system-generated unique identifier for a RequestRotationList</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a RequestRotationList")]
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or Sets Request
        /// </summary>
        public Request Request { get; set; }
        
        /// <summary>
        /// Foreign key for Request 
        /// </summary>       
        [ForeignKey("Request")]
        public int? RequestRefId { get; set; }
        
        /// <summary>
        /// The sort order of the piece of equipment on the rotaton list at the time the request was created. This is the order the equipment will be offered the available work.
        /// </summary>
        /// <value>The sort order of the piece of equipment on the rotaton list at the time the request was created. This is the order the equipment will be offered the available work.</value>
        [MetaDataExtension (Description = "The sort order of the piece of equipment on the rotaton list at the time the request was created. This is the order the equipment will be offered the available work.")]
        public int? RotationListSortOrder { get; set; }
        
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
        /// The rental agreement (if any) created for an accepted hire offer.
        /// </summary>
        /// <value>The rental agreement (if any) created for an accepted hire offer.</value>
        [MetaDataExtension (Description = "The rental agreement (if any) created for an accepted hire offer.")]
        public RentalAgreement RentalAgreement { get; set; }
        
        /// <summary>
        /// Foreign key for RentalAgreement 
        /// </summary>       
        [ForeignKey("RentalAgreement")]
        public int? RentalAgreementRefId { get; set; }
        
        /// <summary>
        /// True if the HETS Clerk designated the hire of this equipment as being a Forced Hire. A Force Hire implies that the usual seniority rules for hiring are bypassed because of special circumstances related to the hire - e.g. a the hire requires an attachment only one piece of equipment has.
        /// </summary>
        /// <value>True if the HETS Clerk designated the hire of this equipment as being a Forced Hire. A Force Hire implies that the usual seniority rules for hiring are bypassed because of special circumstances related to the hire - e.g. a the hire requires an attachment only one piece of equipment has.</value>
        [MetaDataExtension (Description = "True if the HETS Clerk designated the hire of this equipment as being a Forced Hire. A Force Hire implies that the usual seniority rules for hiring are bypassed because of special circumstances related to the hire - e.g. a the hire requires an attachment only one piece of equipment has.")]
        public bool? IsForceHire { get; set; }
        
        /// <summary>
        /// True if the HETS Clerk contacted the equipment owner and asked to hire the piece of equipment.
        /// </summary>
        /// <value>True if the HETS Clerk contacted the equipment owner and asked to hire the piece of equipment.</value>
        [MetaDataExtension (Description = "True if the HETS Clerk contacted the equipment owner and asked to hire the piece of equipment.")]
        public bool? WasAsked { get; set; }
        
        /// <summary>
        /// The Date-Time the HETS clerk contacted the equipment owner and asked to hire the piece of equipment.
        /// </summary>
        /// <value>The Date-Time the HETS clerk contacted the equipment owner and asked to hire the piece of equipment.</value>
        [MetaDataExtension (Description = "The Date-Time the HETS clerk contacted the equipment owner and asked to hire the piece of equipment.")]
        public DateTime? AskedDateTime { get; set; }
        
        /// <summary>
        /// The response to the offer to hire. Null prior to receiving a response; a string after with the response - likely just Yes or No
        /// </summary>
        /// <value>The response to the offer to hire. Null prior to receiving a response; a string after with the response - likely just Yes or No</value>
        [MetaDataExtension (Description = "The response to the offer to hire. Null prior to receiving a response; a string after with the response - likely just Yes or No")]
        public string OfferResponse { get; set; }
        
        /// <summary>
        /// An optional reason given by the equipment owner for refusing the offer.
        /// </summary>
        /// <value>An optional reason given by the equipment owner for refusing the offer.</value>
        [MetaDataExtension (Description = "An optional reason given by the equipment owner for refusing the offer.")]
        [MaxLength(2048)]
        
        public string RefuseReason { get; set; }
        
        /// <summary>
        /// An optional general note about the offer.
        /// </summary>
        /// <value>An optional general note about the offer.</value>
        [MetaDataExtension (Description = "An optional general note about the offer.")]
        [MaxLength(2048)]
        
        public string Note { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class RequestRotationList {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Request: ").Append(Request).Append("\n");
            sb.Append("  RotationListSortOrder: ").Append(RotationListSortOrder).Append("\n");
            sb.Append("  Equipment: ").Append(Equipment).Append("\n");
            sb.Append("  RentalAgreement: ").Append(RentalAgreement).Append("\n");
            sb.Append("  IsForceHire: ").Append(IsForceHire).Append("\n");
            sb.Append("  WasAsked: ").Append(WasAsked).Append("\n");
            sb.Append("  AskedDateTime: ").Append(AskedDateTime).Append("\n");
            sb.Append("  OfferResponse: ").Append(OfferResponse).Append("\n");
            sb.Append("  RefuseReason: ").Append(RefuseReason).Append("\n");
            sb.Append("  Note: ").Append(Note).Append("\n");
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
            return Equals((RequestRotationList)obj);
        }

        /// <summary>
        /// Returns true if RequestRotationList instances are equal
        /// </summary>
        /// <param name="other">Instance of RequestRotationList to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RequestRotationList other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.Request == other.Request ||
                    this.Request != null &&
                    this.Request.Equals(other.Request)
                ) &&                 
                (
                    this.RotationListSortOrder == other.RotationListSortOrder ||
                    this.RotationListSortOrder != null &&
                    this.RotationListSortOrder.Equals(other.RotationListSortOrder)
                ) &&                 
                (
                    this.Equipment == other.Equipment ||
                    this.Equipment != null &&
                    this.Equipment.Equals(other.Equipment)
                ) &&                 
                (
                    this.RentalAgreement == other.RentalAgreement ||
                    this.RentalAgreement != null &&
                    this.RentalAgreement.Equals(other.RentalAgreement)
                ) &&                 
                (
                    this.IsForceHire == other.IsForceHire ||
                    this.IsForceHire != null &&
                    this.IsForceHire.Equals(other.IsForceHire)
                ) &&                 
                (
                    this.WasAsked == other.WasAsked ||
                    this.WasAsked != null &&
                    this.WasAsked.Equals(other.WasAsked)
                ) &&                 
                (
                    this.AskedDateTime == other.AskedDateTime ||
                    this.AskedDateTime != null &&
                    this.AskedDateTime.Equals(other.AskedDateTime)
                ) &&                 
                (
                    this.OfferResponse == other.OfferResponse ||
                    this.OfferResponse != null &&
                    this.OfferResponse.Equals(other.OfferResponse)
                ) &&                 
                (
                    this.RefuseReason == other.RefuseReason ||
                    this.RefuseReason != null &&
                    this.RefuseReason.Equals(other.RefuseReason)
                ) &&                 
                (
                    this.Note == other.Note ||
                    this.Note != null &&
                    this.Note.Equals(other.Note)
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
                if (this.Request != null)
                {
                    hash = hash * 59 + this.Request.GetHashCode();
                }                if (this.RotationListSortOrder != null)
                {
                    hash = hash * 59 + this.RotationListSortOrder.GetHashCode();
                }                
                                   
                if (this.Equipment != null)
                {
                    hash = hash * 59 + this.Equipment.GetHashCode();
                }                   
                if (this.RentalAgreement != null)
                {
                    hash = hash * 59 + this.RentalAgreement.GetHashCode();
                }                if (this.IsForceHire != null)
                {
                    hash = hash * 59 + this.IsForceHire.GetHashCode();
                }                
                                if (this.WasAsked != null)
                {
                    hash = hash * 59 + this.WasAsked.GetHashCode();
                }                
                                if (this.AskedDateTime != null)
                {
                    hash = hash * 59 + this.AskedDateTime.GetHashCode();
                }                
                                if (this.OfferResponse != null)
                {
                    hash = hash * 59 + this.OfferResponse.GetHashCode();
                }                
                                if (this.RefuseReason != null)
                {
                    hash = hash * 59 + this.RefuseReason.GetHashCode();
                }                
                                if (this.Note != null)
                {
                    hash = hash * 59 + this.Note.GetHashCode();
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
        public static bool operator ==(RequestRotationList left, RequestRotationList right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(RequestRotationList left, RequestRotationList right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
