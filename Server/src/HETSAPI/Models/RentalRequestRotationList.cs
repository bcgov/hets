using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// REntal Request Rotation List Database Model
    /// </summary>
    [MetaDataExtension (Description = "An eligible piece of equipment for a request and a tracking of the hire offer and response process related to a request for that piece of equipment. Includes a link from the equipment to a Rental Agreement if the equipment was hired to satisfy a part of the request.")]
    public sealed class RentalRequestRotationList : AuditableEntity, IEquatable<RentalRequestRotationList>
    {
        /// <summary>
        /// Rental REquest Rotation List Database Model Constructor (required by entity framework)
        /// </summary>
        public RentalRequestRotationList()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalRequestRotationList" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a RequestRotationList (required).</param>
        /// <param name="rotationListSortOrder">The sort order of the piece of equipment on the rotation list at the time the request was created. This is the order the equipment will be offered the available work. (required).</param>
        /// <param name="equipment">Equipment (required).</param>
        /// <param name="rentalAgreement">The rental agreement (if any) created for an accepted hire offer..</param>
        /// <param name="isForceHire">True if the HETS Clerk designated the hire of this equipment as being a Forced Hire. A Force Hire implies that the usual seniority rules for hiring are bypassed because of special circumstances related to the hire - e.g. a the hire requires an attachment only one piece of equipment has..</param>
        /// <param name="wasAsked">True if the HETS Clerk contacted the equipment owner and asked to hire the piece of equipment..</param>
        /// <param name="askedDateTime">The Date-Time the HETS clerk contacted the equipment owner and asked to hire the piece of equipment..</param>
        /// <param name="offerResponse">The response to the offer to hire. Null prior to receiving a response; a string after with the response - likely just Yes or No.</param>
        /// <param name="offerRefusalReason">The reason why the user refused the offer based on a selection of values from the UI..</param>
        /// <param name="offerResponseDatetime">The date and time the final response to the offer was established..</param>
        /// <param name="offerResponseNote">A note entered about the response to the offer from the equipment owner about the offer. Usually used when the offer is a &amp;quot;No&amp;quot; or &amp;quot;Force Hire&amp;quot;..</param>
        /// <param name="note">An optional general note about the offer..</param>
        public RentalRequestRotationList(int id, int rotationListSortOrder, Equipment equipment, RentalAgreement rentalAgreement = null, 
            bool? isForceHire = null, bool? wasAsked = null, DateTime? askedDateTime = null, string offerResponse = null, 
            string offerRefusalReason = null, DateTime? offerResponseDatetime = null, string offerResponseNote = null, string note = null)
        {   
            Id = id;
            RotationListSortOrder = rotationListSortOrder;
            Equipment = equipment;
            RentalAgreement = rentalAgreement;
            IsForceHire = isForceHire;
            WasAsked = wasAsked;
            AskedDateTime = askedDateTime;
            OfferResponse = offerResponse;
            OfferRefusalReason = offerRefusalReason;
            OfferResponseDatetime = offerResponseDatetime;
            OfferResponseNote = offerResponseNote;
            Note = note;
        }

        /// <summary>
        /// A system-generated unique identifier for a RequestRotationList
        /// </summary>
        /// <value>A system-generated unique identifier for a RequestRotationList</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a RequestRotationList")]
        public int Id { get; set; }
        
        /// <summary>
        /// The sort order of the piece of equipment on the rotation list at the time the request was created. This is the order the equipment will be offered the available work.
        /// </summary>
        /// <value>The sort order of the piece of equipment on the rotation list at the time the request was created. This is the order the equipment will be offered the available work.</value>
        [MetaDataExtension (Description = "The sort order of the piece of equipment on the rotation list at the time the request was created. This is the order the equipment will be offered the available work.")]
        public int RotationListSortOrder { get; set; }
        
        /// <summary>
        /// Gets or Sets Equipment
        /// </summary>
        public Equipment Equipment { get; set; }
        
        /// <summary>
        /// Foreign key for Equipment 
        /// </summary>   
        [ForeignKey("Equipment")]
		[JsonIgnore]		
        public int? EquipmentId { get; set; }
        
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
		[JsonIgnore]
		[MetaDataExtension (Description = "The rental agreement (if any) created for an accepted hire offer.")]
        public int? RentalAgreementId { get; set; }
        
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
        /// The reason why the user refused the offer based on a selection of values from the UI.
        /// </summary>
        /// <value>The reason why the user refused the offer based on a selection of values from the UI.</value>
        [MetaDataExtension (Description = "The reason why the user refused the offer based on a selection of values from the UI.")]
        [MaxLength(50)]        
        public string OfferRefusalReason { get; set; }
        
        /// <summary>
        /// The date and time the final response to the offer was established.
        /// </summary>
        /// <value>The date and time the final response to the offer was established.</value>
        [MetaDataExtension (Description = "The date and time the final response to the offer was established.")]
        public DateTime? OfferResponseDatetime { get; set; }
        
        /// <summary>
        /// A note entered about the response to the offer from the equipment owner about the offer. Usually used when the offer is a &quot;No&quot; or &quot;Force Hire&quot;.
        /// </summary>
        /// <value>A note entered about the response to the offer from the equipment owner about the offer. Usually used when the offer is a &quot;No&quot; or &quot;Force Hire&quot;.</value>
        [MetaDataExtension (Description = "A note entered about the response to the offer from the equipment owner about the offer. Usually used when the offer is a &quot;No&quot; or &quot;Force Hire&quot;.")]
        [MaxLength(2048)]        
        public string OfferResponseNote { get; set; }
        
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

            sb.Append("class RentalRequestRotationList {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  RotationListSortOrder: ").Append(RotationListSortOrder).Append("\n");
            sb.Append("  Equipment: ").Append(Equipment).Append("\n");
            sb.Append("  RentalAgreement: ").Append(RentalAgreement).Append("\n");
            sb.Append("  IsForceHire: ").Append(IsForceHire).Append("\n");
            sb.Append("  WasAsked: ").Append(WasAsked).Append("\n");
            sb.Append("  AskedDateTime: ").Append(AskedDateTime).Append("\n");
            sb.Append("  OfferResponse: ").Append(OfferResponse).Append("\n");
            sb.Append("  OfferRefusalReason: ").Append(OfferRefusalReason).Append("\n");
            sb.Append("  OfferResponseDatetime: ").Append(OfferResponseDatetime).Append("\n");
            sb.Append("  OfferResponseNote: ").Append(OfferResponseNote).Append("\n");
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

            return Equals((RentalRequestRotationList)obj);
        }

        /// <summary>
        /// Returns true if RentalRequestRotationList instances are equal
        /// </summary>
        /// <param name="other">Instance of RentalRequestRotationList to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RentalRequestRotationList other)
        {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    RotationListSortOrder == other.RotationListSortOrder ||
                    RotationListSortOrder.Equals(other.RotationListSortOrder)
                ) &&                 
                (
                    Equipment == other.Equipment ||
                    Equipment != null &&
                    Equipment.Equals(other.Equipment)
                ) &&                 
                (
                    RentalAgreement == other.RentalAgreement ||
                    RentalAgreement != null &&
                    RentalAgreement.Equals(other.RentalAgreement)
                ) &&                 
                (
                    IsForceHire == other.IsForceHire ||
                    IsForceHire != null &&
                    IsForceHire.Equals(other.IsForceHire)
                ) &&                 
                (
                    WasAsked == other.WasAsked ||
                    WasAsked != null &&
                    WasAsked.Equals(other.WasAsked)
                ) &&                 
                (
                    AskedDateTime == other.AskedDateTime ||
                    AskedDateTime != null &&
                    AskedDateTime.Equals(other.AskedDateTime)
                ) &&                 
                (
                    OfferResponse == other.OfferResponse ||
                    OfferResponse != null &&
                    OfferResponse.Equals(other.OfferResponse)
                ) &&                 
                (
                    OfferRefusalReason == other.OfferRefusalReason ||
                    OfferRefusalReason != null &&
                    OfferRefusalReason.Equals(other.OfferRefusalReason)
                ) &&                 
                (
                    OfferResponseDatetime == other.OfferResponseDatetime ||
                    OfferResponseDatetime != null &&
                    OfferResponseDatetime.Equals(other.OfferResponseDatetime)
                ) &&                 
                (
                    OfferResponseNote == other.OfferResponseNote ||
                    OfferResponseNote != null &&
                    OfferResponseNote.Equals(other.OfferResponseNote)
                ) &&                 
                (
                    Note == other.Note ||
                    Note != null &&
                    Note.Equals(other.Note)
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
                hash = hash * 59 + RotationListSortOrder.GetHashCode();

                if (Equipment != null)
                {
                    hash = hash * 59 + Equipment.GetHashCode();
                }                   
                if (RentalAgreement != null)
                {
                    hash = hash * 59 + RentalAgreement.GetHashCode();
                }

                if (IsForceHire != null)
                {
                    hash = hash * 59 + IsForceHire.GetHashCode();
                }

                if (WasAsked != null)
                {
                    hash = hash * 59 + WasAsked.GetHashCode();
                }

                if (AskedDateTime != null)
                {
                    hash = hash * 59 + AskedDateTime.GetHashCode();
                }

                if (OfferResponse != null)
                {
                    hash = hash * 59 + OfferResponse.GetHashCode();
                }

                if (OfferRefusalReason != null)
                {
                    hash = hash * 59 + OfferRefusalReason.GetHashCode();
                }

                if (OfferResponseDatetime != null)
                {
                    hash = hash * 59 + OfferResponseDatetime.GetHashCode();
                }

                if (OfferResponseNote != null)
                {
                    hash = hash * 59 + OfferResponseNote.GetHashCode();
                }

                if (Note != null)
                {
                    hash = hash * 59 + Note.GetHashCode();
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
        public static bool operator ==(RentalRequestRotationList left, RentalRequestRotationList right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(RentalRequestRotationList left, RentalRequestRotationList right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
