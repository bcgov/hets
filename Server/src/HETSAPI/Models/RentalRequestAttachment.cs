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
    /// Attachments that are required as part of the Rental Requests
    /// </summary>
        [MetaDataExtension (Description = "Attachments that are required as part of the Rental Requests")]

    public partial class RentalRequestAttachment : AuditableEntity, IEquatable<RentalRequestAttachment>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public RentalRequestAttachment()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalRequestAttachment" /> class.
        /// </summary>
        /// <param name="Id">A system-generated unique identifier for a RentalRequestAttachment (required).</param>
        /// <param name="RentalRequest">A foreign key reference to the system-generated unique identifier for a Rental Request (required).</param>
        /// <param name="Attachment">The name&amp;#x2F;type attachment needed as part of the fulfillment of the request (required).</param>
        public RentalRequestAttachment(int Id, RentalRequest RentalRequest, string Attachment)
        {   
            this.Id = Id;
            this.RentalRequest = RentalRequest;
            this.Attachment = Attachment;


        }

        /// <summary>
        /// A system-generated unique identifier for a RentalRequestAttachment
        /// </summary>
        /// <value>A system-generated unique identifier for a RentalRequestAttachment</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a RentalRequestAttachment")]
        public int Id { get; set; }
        
        /// <summary>
        /// A foreign key reference to the system-generated unique identifier for a Rental Request
        /// </summary>
        /// <value>A foreign key reference to the system-generated unique identifier for a Rental Request</value>
        [MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for a Rental Request")]
        public RentalRequest RentalRequest { get; set; }
        
        /// <summary>
        /// Foreign key for RentalRequest 
        /// </summary>   
        [ForeignKey("RentalRequest")]
		[JsonIgnore]
		[MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for a Rental Request")]
        public int? RentalRequestId { get; set; }
        
        /// <summary>
        /// The name&#x2F;type attachment needed as part of the fulfillment of the request
        /// </summary>
        /// <value>The name&#x2F;type attachment needed as part of the fulfillment of the request</value>
        [MetaDataExtension (Description = "The name&#x2F;type attachment needed as part of the fulfillment of the request")]
        [MaxLength(150)]
        
        public string Attachment { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class RentalRequestAttachment {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  RentalRequest: ").Append(RentalRequest).Append("\n");
            sb.Append("  Attachment: ").Append(Attachment).Append("\n");
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
            return Equals((RentalRequestAttachment)obj);
        }

        /// <summary>
        /// Returns true if RentalRequestAttachment instances are equal
        /// </summary>
        /// <param name="other">Instance of RentalRequestAttachment to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RentalRequestAttachment other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.RentalRequest == other.RentalRequest ||
                    this.RentalRequest != null &&
                    this.RentalRequest.Equals(other.RentalRequest)
                ) &&                 
                (
                    this.Attachment == other.Attachment ||
                    this.Attachment != null &&
                    this.Attachment.Equals(other.Attachment)
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
                if (this.RentalRequest != null)
                {
                    hash = hash * 59 + this.RentalRequest.GetHashCode();
                }                if (this.Attachment != null)
                {
                    hash = hash * 59 + this.Attachment.GetHashCode();
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
        public static bool operator ==(RentalRequestAttachment left, RentalRequestAttachment right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(RentalRequestAttachment left, RentalRequestAttachment right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
