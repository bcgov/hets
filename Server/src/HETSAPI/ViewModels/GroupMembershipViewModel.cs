using System;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace HETSAPI.ViewModels
{
    /// <summary>
    /// Group Membership View Model
    /// </summary>
    [DataContract]
    public sealed class GroupMembershipViewModel : IEquatable<GroupMembershipViewModel>
    {
        /// <summary>
        /// Group Membership View Model Constructor
        /// </summary>
        public GroupMembershipViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupMembershipViewModel" /> class.
        /// </summary>
        /// <param name="active">Active (required).</param>
        /// <param name="groupId">GroupId (required).</param>
        /// <param name="userId">UserId (required).</param>
        /// <param name="id">Id.</param>
        public GroupMembershipViewModel(bool active, int groupId, int userId, int? id = null)
        {   
            Active = active;
            GroupId = groupId;
            UserId = userId;
            Id = id;
        }

        /// <summary>
        /// Gets or Sets Active
        /// </summary>
        [DataMember(Name="active")]
        public bool Active { get; set; }

        /// <summary>
        /// Gets or Sets GroupId
        /// </summary>
        [DataMember(Name="groupId")]
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or Sets UserId
        /// </summary>
        [DataMember(Name="userId")]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id")]
        public int? Id { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class GroupMembershipViewModel {\n");
            sb.Append("  Active: ").Append(Active).Append("\n");
            sb.Append("  GroupId: ").Append(GroupId).Append("\n");
            sb.Append("  UserId: ").Append(UserId).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
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

            return Equals((GroupMembershipViewModel)obj);
        }

        /// <summary>
        /// Returns true if GroupMembershipViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of GroupMembershipViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(GroupMembershipViewModel other)
        {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Active == other.Active ||
                    Active.Equals(other.Active)
                ) &&                 
                (
                    GroupId == other.GroupId ||
                    GroupId.Equals(other.GroupId)
                ) &&                 
                (
                    UserId == other.UserId ||
                    UserId.Equals(other.UserId)
                ) &&                 
                (
                    Id == other.Id ||
                    Id != null &&
                    Id.Equals(other.Id)
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
                hash = hash * 59 + Active.GetHashCode();                                                   
                hash = hash * 59 + GroupId.GetHashCode();                                   
                hash = hash * 59 + UserId.GetHashCode();

                if (Id != null)
                {
                    hash = hash * 59 + Id.GetHashCode();
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
        public static bool operator ==(GroupMembershipViewModel left, GroupMembershipViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(GroupMembershipViewModel left, GroupMembershipViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
