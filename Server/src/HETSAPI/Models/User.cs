using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// User Database Model
    /// </summary>
    [MetaData (Description = "An identified user in the HETS Application that has a defined authorization level.")]
	public partial class User : AuditableEntity, IEquatable<User>
	{
        /// <summary>
        /// User Database Model Constructor (required by entity framework)
        /// </summary>
        public User()
		{
			Id = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="User" /> class.
		/// </summary>
		/// <param name="id">A system-generated unique identifier for a User (required).</param>
		/// <param name="givenName">Given name of the user. (required).</param>
		/// <param name="surname">Surname of the user. (required).</param>
		/// <param name="active">A flag indicating the User is active in the system. Set false to remove access to the system for the user. (required).</param>
		/// <param name="initials">Initials of the user, to be presented where screen space is at a premium..</param>
		/// <param name="email">The email address of the user in the system..</param>
		/// <param name="smUserId">Security Manager User ID.</param>
		/// <param name="guid">The GUID unique to the user as provided by the authentication system. In this case, authentication is done by Siteminder and the GUID uniquely identifies the user within the user directories managed by Siteminder - e.g. IDIR and BCeID. The GUID is equivalent to the IDIR Id, but is guaranteed unique to a person, while the IDIR ID is not - IDIR IDs can be recycled..</param>
		/// <param name="smAuthorizationDirectory">The user directory service used by Siteminder to authenticate the user - usually IDIR or BCeID..</param>
		/// <param name="userRoles">UserRoles.</param>
		/// <param name="groupMemberships">GroupMemberships.</param>
		/// <param name="district">The District that the User belongs to.</param>
		public User(int id, string givenName, string surname, bool active, string initials = null, string email = null, 
		    string smUserId = null, string guid = null, string smAuthorizationDirectory = null, List<UserRole> userRoles = null, 
		    List<GroupMembership> groupMemberships = null, District district = null)
		{
			Id = id;
			GivenName = givenName;
			Surname = surname;
			Active = active;
			Initials = initials;
			Email = email;
			SmUserId = smUserId;
			Guid = guid;
			SmAuthorizationDirectory = smAuthorizationDirectory;
			UserRoles = userRoles;
			GroupMemberships = groupMemberships;
			District = district;
		}

		/// <summary>
		/// A system-generated unique identifier for a User
		/// </summary>
		/// <value>A system-generated unique identifier for a User</value>
		[MetaData (Description = "A system-generated unique identifier for a User")]
		public int Id { get; set; }

		/// <summary>
		/// Given name of the user.
		/// </summary>
		/// <value>Given name of the user.</value>
		[MetaData (Description = "Given name of the user.")]
		[MaxLength(50)]
		public string GivenName { get; set; }

		/// <summary>
		/// Surname of the user.
		/// </summary>
		/// <value>Surname of the user.</value>
		[MetaData (Description = "Surname of the user.")]
		[MaxLength(50)]
		public string Surname { get; set; }

		/// <summary>
		/// A flag indicating the User is active in the system. Set false to remove access to the system for the user.
		/// </summary>
		/// <value>A flag indicating the User is active in the system. Set false to remove access to the system for the user.</value>
		[MetaData (Description = "A flag indicating the User is active in the system. Set false to remove access to the system for the user.")]
		public bool Active { get; set; }

		/// <summary>
		/// Initials of the user, to be presented where screen space is at a premium.
		/// </summary>
		/// <value>Initials of the user, to be presented where screen space is at a premium.</value>
		[MetaData (Description = "Initials of the user, to be presented where screen space is at a premium.")]
		[MaxLength(10)]
		public string Initials { get; set; }

		/// <summary>
		/// The email address of the user in the system.
		/// </summary>
		/// <value>The email address of the user in the system.</value>
		[MetaData (Description = "The email address of the user in the system.")]
		[MaxLength(255)]
		public string Email { get; set; }

		/// <summary>
		/// Security Manager User ID
		/// </summary>
		/// <value>Security Manager User ID</value>
		[MetaData (Description = "Security Manager User ID")]
		[MaxLength(255)]
		public string SmUserId { get; set; }

		/// <summary>
		/// The GUID unique to the user as provided by the authentication system. In this case, authentication is done by Siteminder and the GUID uniquely identifies the user within the user directories managed by Siteminder - e.g. IDIR and BCeID. The GUID is equivalent to the IDIR Id, but is guaranteed unique to a person, while the IDIR ID is not - IDIR IDs can be recycled.
		/// </summary>
		/// <value>The GUID unique to the user as provided by the authentication system. In this case, authentication is done by Siteminder and the GUID uniquely identifies the user within the user directories managed by Siteminder - e.g. IDIR and BCeID. The GUID is equivalent to the IDIR Id, but is guaranteed unique to a person, while the IDIR ID is not - IDIR IDs can be recycled.</value>
		[MetaData (Description = "The GUID unique to the user as provided by the authentication system. In this case, authentication is done by Siteminder and the GUID uniquely identifies the user within the user directories managed by Siteminder - e.g. IDIR and BCeID. The GUID is equivalent to the IDIR Id, but is guaranteed unique to a person, while the IDIR ID is not - IDIR IDs can be recycled.")]
		[MaxLength(255)]
		public string Guid { get; set; }

		/// <summary>
		/// The user directory service used by Siteminder to authenticate the user - usually IDIR or BCeID.
		/// </summary>
		/// <value>The user directory service used by Siteminder to authenticate the user - usually IDIR or BCeID.</value>
		[MetaData (Description = "The user directory service used by Siteminder to authenticate the user - usually IDIR or BCeID.")]
		[MaxLength(255)]
		public string SmAuthorizationDirectory { get; set; }

		/// <summary>
		/// Gets or Sets UserRoles
		/// </summary>
		public List<UserRole> UserRoles { get; set; }

		/// <summary>
		/// Gets or Sets GroupMemberships
		/// </summary>
		public List<GroupMembership> GroupMemberships { get; set; }

		/// <summary>
		/// The District that the User belongs to
		/// </summary>
		/// <value>The District that the User belongs to</value>
		[MetaData (Description = "The District that the User belongs to")]
		public District District { get; set; }

		/// <summary>
		/// Foreign key for District
		/// </summary>
		[ForeignKey("District")]
		[JsonIgnore]
		[MetaData (Description = "The District that the User belongs to")]
		public int? DistrictId { get; set; }

		/// <summary>
		/// Returns the string presentation of the object
		/// </summary>
		/// <returns>String presentation of the object</returns>
		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.Append("class User {\n");
			sb.Append("  Id: ").Append(Id).Append("\n");
			sb.Append("  GivenName: ").Append(GivenName).Append("\n");
			sb.Append("  Surname: ").Append(Surname).Append("\n");
			sb.Append("  Active: ").Append(Active).Append("\n");
			sb.Append("  Initials: ").Append(Initials).Append("\n");
			sb.Append("  Email: ").Append(Email).Append("\n");
			sb.Append("  SmUserId: ").Append(SmUserId).Append("\n");
			sb.Append("  Guid: ").Append(Guid).Append("\n");
			sb.Append("  SmAuthorizationDirectory: ").Append(SmAuthorizationDirectory).Append("\n");
			sb.Append("  UserRoles: ").Append(UserRoles).Append("\n");
			sb.Append("  GroupMemberships: ").Append(GroupMemberships).Append("\n");
			sb.Append("  District: ").Append(District).Append("\n");
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
			return obj.GetType() == GetType() && Equals((User)obj);
		}

		/// <summary>
		/// Returns true if User instances are equal
		/// </summary>
		/// <param name="other">Instance of User to be compared</param>
		/// <returns>Boolean</returns>
		public bool Equals(User other)
		{
			if (other is null) { return false; }
			if (ReferenceEquals(this, other)) { return true; }

			return
				(
					Id == other.Id ||
					Id.Equals(other.Id)
				) &&
				(
					GivenName == other.GivenName ||
					GivenName != null &&
					GivenName.Equals(other.GivenName)
				) &&
				(
					Surname == other.Surname ||
					Surname != null &&
					Surname.Equals(other.Surname)
				) &&
				(
					Active == other.Active ||
					Active.Equals(other.Active)
				) &&
				(
					Initials == other.Initials ||
					Initials != null &&
					Initials.Equals(other.Initials)
				) &&
				(
					Email == other.Email ||
					Email != null &&
					Email.Equals(other.Email)
				) &&
				(
					SmUserId == other.SmUserId ||
					SmUserId != null &&
					SmUserId.Equals(other.SmUserId)
				) &&
				(
					Guid == other.Guid ||
					Guid != null &&
					Guid.Equals(other.Guid)
				) &&
				(
					SmAuthorizationDirectory == other.SmAuthorizationDirectory ||
					SmAuthorizationDirectory != null &&
					SmAuthorizationDirectory.Equals(other.SmAuthorizationDirectory)
				) &&
				(
					UserRoles == other.UserRoles ||
					UserRoles != null &&
					UserRoles.SequenceEqual(other.UserRoles)
				) &&
				(
					GroupMemberships == other.GroupMemberships ||
					GroupMemberships != null &&
					GroupMemberships.SequenceEqual(other.GroupMemberships)
				) &&
				(
					District == other.District ||
					District != null &&
					District.Equals(other.District)
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

			    if (GivenName != null)
				{
					hash = hash * 59 + GivenName.GetHashCode();
				}

			    if (Surname != null)
				{
					hash = hash * 59 + Surname.GetHashCode();
				}

				hash = hash * 59 + Active.GetHashCode();

			    if (Initials != null)
				{
					hash = hash * 59 + Initials.GetHashCode();
				}

			    if (Email != null)
				{
					hash = hash * 59 + Email.GetHashCode();
				}

			    if (SmUserId != null)
				{
					hash = hash * 59 + SmUserId.GetHashCode();
				}

			    if (Guid != null)
				{
					hash = hash * 59 + Guid.GetHashCode();
				}

			    if (SmAuthorizationDirectory != null)
				{
					hash = hash * 59 + SmAuthorizationDirectory.GetHashCode();
				}

				if (UserRoles != null)
				{
					hash = hash * 59 + UserRoles.GetHashCode();
				}

			    if (GroupMemberships != null)
				{
					hash = hash * 59 + GroupMemberships.GetHashCode();
				}

			    if (District != null)
				{
					hash = hash * 59 + District.GetHashCode();
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
		public static bool operator ==(User left, User right)
		{
			return Equals(left, right);
		}

		/// <summary>
		/// Not Equals
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator !=(User left, User right)
		{
			return !Equals(left, right);
		}

		#endregion Operators
	}
}
