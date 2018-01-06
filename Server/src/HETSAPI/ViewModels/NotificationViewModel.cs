using System;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace HETSAPI.ViewModels
{
    /// <summary>
    /// Notification View Model
    /// </summary>
    [DataContract]
    public sealed class NotificationViewModel : IEquatable<NotificationViewModel>
    {
        /// <summary>
        /// Notification View Model Constructor
        /// </summary>
        public NotificationViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationViewModel" /> class.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="eventId">EventId.</param>
        /// <param name="event2Id">Event2Id.</param>
        /// <param name="hasBeenViewed">HasBeenViewed.</param>
        /// <param name="isWatchNotification">IsWatchNotification.</param>
        /// <param name="isExpired">IsExpired.</param>
        /// <param name="isAllDay">IsAllDay.</param>
        /// <param name="priorityCode">PriorityCode.</param>
        /// <param name="userId">UserId.</param>
        public NotificationViewModel(int id, int? eventId = null, int? event2Id = null, bool? hasBeenViewed = null, 
            bool? isWatchNotification = null, bool? isExpired = null, bool? isAllDay = null, string priorityCode = null, int? userId = null)
        {
            Id = id;
            EventId = eventId;
            Event2Id = event2Id;
            HasBeenViewed = hasBeenViewed;
            IsWatchNotification = isWatchNotification;
            IsExpired = isExpired;
            IsAllDay = isAllDay;
            PriorityCode = priorityCode;
            UserId = userId;
        }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id")]
        public int? Id { get; set; }

        /// <summary>
        /// Gets or Sets EventId
        /// </summary>
        [DataMember(Name="eventId")]
        public int? EventId { get; set; }

        /// <summary>
        /// Gets or Sets Event2Id
        /// </summary>
        [DataMember(Name="event2Id")]
        public int? Event2Id { get; set; }

        /// <summary>
        /// Gets or Sets HasBeenViewed
        /// </summary>
        [DataMember(Name="hasBeenViewed")]
        public bool? HasBeenViewed { get; set; }

        /// <summary>
        /// Gets or Sets IsWatchNotification
        /// </summary>
        [DataMember(Name="isWatchNotification")]
        public bool? IsWatchNotification { get; set; }

        /// <summary>
        /// Gets or Sets IsExpired
        /// </summary>
        [DataMember(Name="isExpired")]
        public bool? IsExpired { get; set; }

        /// <summary>
        /// Gets or Sets IsAllDay
        /// </summary>
        [DataMember(Name="isAllDay")]
        public bool? IsAllDay { get; set; }

        /// <summary>
        /// Gets or Sets PriorityCode
        /// </summary>
        [DataMember(Name="priorityCode")]
        public string PriorityCode { get; set; }

        /// <summary>
        /// Gets or Sets UserId
        /// </summary>
        [DataMember(Name="userId")]
        public int? UserId { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class NotificationViewModel {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  EventId: ").Append(EventId).Append("\n");
            sb.Append("  Event2Id: ").Append(Event2Id).Append("\n");
            sb.Append("  HasBeenViewed: ").Append(HasBeenViewed).Append("\n");
            sb.Append("  IsWatchNotification: ").Append(IsWatchNotification).Append("\n");
            sb.Append("  IsExpired: ").Append(IsExpired).Append("\n");
            sb.Append("  IsAllDay: ").Append(IsAllDay).Append("\n");
            sb.Append("  PriorityCode: ").Append(PriorityCode).Append("\n");
            sb.Append("  UserId: ").Append(UserId).Append("\n");
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

            return Equals((NotificationViewModel)obj);
        }

        /// <summary>
        /// Returns true if NotificationViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of NotificationViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(NotificationViewModel other)
        {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id != null &&
                    Id.Equals(other.Id)
                ) &&                 
                (
                    EventId == other.EventId ||
                    EventId != null &&
                    EventId.Equals(other.EventId)
                ) &&                 
                (
                    Event2Id == other.Event2Id ||
                    Event2Id != null &&
                    Event2Id.Equals(other.Event2Id)
                ) &&                 
                (
                    HasBeenViewed == other.HasBeenViewed ||
                    HasBeenViewed != null &&
                    HasBeenViewed.Equals(other.HasBeenViewed)
                ) &&                 
                (
                    IsWatchNotification == other.IsWatchNotification ||
                    IsWatchNotification != null &&
                    IsWatchNotification.Equals(other.IsWatchNotification)
                ) &&                 
                (
                    IsExpired == other.IsExpired ||
                    IsExpired != null &&
                    IsExpired.Equals(other.IsExpired)
                ) &&                 
                (
                    IsAllDay == other.IsAllDay ||
                    IsAllDay != null &&
                    IsAllDay.Equals(other.IsAllDay)
                ) &&                 
                (
                    PriorityCode == other.PriorityCode ||
                    PriorityCode != null &&
                    PriorityCode.Equals(other.PriorityCode)
                ) &&                 
                (
                    UserId == other.UserId ||
                    UserId != null &&
                    UserId.Equals(other.UserId)
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
                if (Id != null)
                {
                    hash = hash * 59 + Id.GetHashCode();
                }

                if (EventId != null)
                {
                    hash = hash * 59 + EventId.GetHashCode();
                }

                if (Event2Id != null)
                {
                    hash = hash * 59 + Event2Id.GetHashCode();
                }

                if (HasBeenViewed != null)
                {
                    hash = hash * 59 + HasBeenViewed.GetHashCode();
                }

                if (IsWatchNotification != null)
                {
                    hash = hash * 59 + IsWatchNotification.GetHashCode();
                }

                if (IsExpired != null)
                {
                    hash = hash * 59 + IsExpired.GetHashCode();
                }

                if (IsAllDay != null)
                {
                    hash = hash * 59 + IsAllDay.GetHashCode();
                }

                if (PriorityCode != null)
                {
                    hash = hash * 59 + PriorityCode.GetHashCode();
                }

                if (UserId != null)
                {
                    hash = hash * 59 + UserId.GetHashCode();
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
        public static bool operator ==(NotificationViewModel left, NotificationViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(NotificationViewModel left, NotificationViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
