using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Entities
{
    public partial class HetOwner
    {
        /// <summary>
        /// Status Codes
        /// </summary>
        public const string StatusApproved = "Approved";
        public const string StatusArchived = "Archived";
        public const string StatusPending = "Unapproved";

        // HETS-1115 - Do not allow changing seniority affecting entities if an active request exists
        [NotMapped]
        public bool ActiveRentalRequest { get; set; }

        [NotMapped]
        public string Status { get; set; }

        [NotMapped]
        public string ReportDate { get; set; }

        [NotMapped]
        public string Classification { get; set; }

        [NotMapped]
        public string Title { get; set; }

        [NotMapped]
        public int DistrictId { get; set; }

        [NotMapped]
        public int MinistryDistrictId { get; set; }

        [NotMapped]
        public string DistrictName { get; set; }

        [NotMapped]
        public string DistrictAddress { get; set; }

        [NotMapped]
        public string DistrictContact { get; set; }

        [NotMapped]
        public string LocalAreaName { get; set; }

        [NotMapped]
        public string PrimaryContactRole { get; set; }

        [NotMapped]
        public string PrimaryContactGivenName { get; set; }

        [NotMapped]
        public string PrimaryContactSurname { get; set; }

        [NotMapped]
        public string PrimaryContactPhone { get; set; }

        [NotMapped]
        public string PrimaryContactNameBusiness
        {
            get
            {
                if (PrimaryContact == null)
                {
                    return null;
                }

                string temp = PrimaryContact.GivenName;

                if (!string.IsNullOrEmpty(temp))
                {
                    temp = temp + " ";
                }

                return (temp + PrimaryContact.Surname);
            }
        }

        [NotMapped]
        public string LocalAreaNameBusiness => LocalArea?.Name;

        [NotMapped]
        public string DistrictNameBusiness => LocalArea?.ServiceArea?.District?.Name;

        [NotMapped]
        public string SharedKeyHeader { get; set; }
    }
}
