using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HetsData.Model;

namespace HetsData.Helpers
{
    #region Owner Models

    public class OwnerLite
    {
        public int Id { get; set; }
        public string OwnerCode { get; set; }
        public string OrganizationName { get; set; }
        public string LocalAreaName { get; set; }
        public string PrimaryContactName { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
        public int EquipmentCount { get; set; }
        public string Status { get; set; }
    }

    public class OwnerVerificationReportModel
    {
        public string ReportDate { get; set; }
        public string Title { get; set; }
        public int DistrictId { get; set; }
        public int MinistryDistrictId { get; set; }
        public string DistrictName { get; set; }
        public string DistrictAddress { get; set; }
        public string DistrictContact { get; set; }
        public string LocalAreaName { get; set; }
        public List<HetOwner> Owners { get; set; }
    }

    public class OwnerWcbCgl
    {
        public int Id { get; set; }
        public int ServiceAreaId { get; set; }
        public string LocalAreaName { get; set; }
        public string OwnerCode { get; set; }
        public string OrganizationName { get; set; }
        public string PrimaryContactNumber { get; set; }
        public string PrimaryContactCell { get; set; }
        public string WcbNumber { get; set; }
        public DateTime? WcbExpiryDate { get; set; }
        public string CglNumber { get; set; }
        public DateTime? CglExpiryDate { get; set; }
    }

    public class OwnerLiteProjects
    {
        public int Id { get; set; }
        public string OwnerCode { get; set; }
        public string OrganizationName { get; set; }
        public int? LocalAreaId { get; set; }
        public List<int?> ProjectIds { get; set; }
    }

    public class MailingLabelPdfViewModel
    {
        public string ReportDate { get; set; }
        public string Title { get; set; }
        public int DistrictId { get; set; }
        public List<MailingLabelRowModel> LabelRow { get; set; }
    }

    public class MailingLabelRowModel
    {
        public HetOwner OwnerColumn1 { get; set; }
        public HetOwner OwnerColumn2 { get; set; }
    }

    #endregion

    public static class OwnerHelper
    {
        #region Get an Owner record (plus associated records)

        /// <summary>
        /// Get an Owner record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static HetOwner GetRecord(int id, DbAppContext context, IConfiguration configuration)
        {
            // get equipment status types
            int? statusIdArchived = StatusHelper.GetStatusId(HetEquipment.StatusArchived, "equipmentStatus", context);
            if (statusIdArchived == null)
            {
                throw new ArgumentException("Status Code not found");
            }

            // get owner record
            HetOwner owner = context.HetOwner.AsNoTracking()
                .Include(x => x.OwnerStatusType)
                .Include(x => x.LocalArea.ServiceArea.District.Region)
                .Include(x => x.HetEquipment)
                    .ThenInclude(y => y.LocalArea.ServiceArea.District.Region)
                .Include(x => x.HetEquipment)
                    .ThenInclude(y => y.DistrictEquipmentType)
                        .ThenInclude(z => z.EquipmentType)
                .Include(x => x.HetEquipment)
                    .ThenInclude(y => y.Owner)
                        .ThenInclude(c => c.PrimaryContact)
                .Include(x => x.HetEquipment)
                    .ThenInclude(y => y.EquipmentStatusType)
                .Include(x => x.HetEquipment)
                    .ThenInclude(y => y.HetEquipmentAttachment)
                .Include(x => x.HetContact)
                .Include(x => x.PrimaryContact)
                .FirstOrDefault(a => a.OwnerId == id);

            if (owner != null)
            {
                // remove any archived equipment
                owner.HetEquipment = owner.HetEquipment.Where(e => e.EquipmentStatusTypeId != statusIdArchived).ToList();

                // populate the "Status" description
                owner.Status = owner.OwnerStatusType.OwnerStatusTypeCode;

                foreach (HetEquipment equipment in owner.HetEquipment)
                {
                    equipment.IsHired = EquipmentHelper.IsHired(id, context);
                    equipment.NumberOfBlocks = EquipmentHelper.GetNumberOfBlocks(equipment, configuration);
                    equipment.HoursYtd = EquipmentHelper.GetYtdServiceHours(id, context);
                    equipment.Status = equipment.EquipmentStatusType.EquipmentStatusTypeCode;
                    equipment.EquipmentNumber = int.Parse(Regex.Match(equipment.EquipmentCode, @"\d+").Value);
                }

                // HETS-1115 - Do not allow changing seniority affecting entities if an active request exists
                owner.ActiveRentalRequest = RentalRequestStatus(id, context);
            }

            return owner;
        }

        #endregion

        #region Returns true if any equipment associated with this owner is on an active rotation list

        /// <summary>
        /// Returns true if any equipment associated with this owner is on an active rotation list
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool RentalRequestStatus(int id, DbAppContext context)
        {
            // get equipment status types
            int? statusIdActive = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", context);
            if (statusIdActive == null)
            {
                throw new ArgumentException("Status Code not found");
            }

            // get rental request status type
            int? statusIdInProgress = StatusHelper.GetStatusId(HetRentalRequest.StatusInProgress, "rentalRequestStatus", context);
            if (statusIdInProgress == null)
            {
                throw new ArgumentException("Status Code not found");
            }

            return context.HetRentalRequestRotationList.AsNoTracking()
                .Include(x => x.RentalRequest)
                .Include(x => x.Equipment)
                .Any(x => x.Equipment.OwnerId == id &&
                          x.Equipment.EquipmentStatusTypeId == statusIdActive &&
                          x.RentalRequest.RentalRequestStatusTypeId == statusIdInProgress);

        }

        #endregion

        #region Convert full owner record to a "Lite" version

        /// <summary>
        /// Convert to Owner Lite Model
        /// </summary>
        /// <param name="owner"></param>
        public static OwnerLite ToLiteModel(HetOwner owner)
        {
            OwnerLite ownerLite = new OwnerLite();

            if (owner != null)
            {
                ownerLite.Id = owner.OwnerId;
                ownerLite.OwnerCode = owner.OwnerCode;
                ownerLite.OrganizationName = owner.OrganizationName;

                if (owner.LocalArea != null)
                {
                    ownerLite.LocalAreaName = owner.LocalArea.Name;
                }

                if (owner.PrimaryContact != null)
                {
                    string tempName = "";

                    if (!string.IsNullOrEmpty(owner.PrimaryContact.GivenName))
                    {
                        tempName = owner.PrimaryContact.GivenName.Trim();

                        if (!string.IsNullOrEmpty(tempName))
                        {
                            tempName = tempName + " ";
                        }
                    }

                    if (!string.IsNullOrEmpty(owner.PrimaryContact.Surname))
                    {
                        tempName = tempName + owner.PrimaryContact.Surname.Trim();
                    }

                    ownerLite.PrimaryContactName = tempName;
                    ownerLite.WorkPhoneNumber = owner.PrimaryContact.WorkPhoneNumber;
                    ownerLite.MobilePhoneNumber = owner.PrimaryContact.MobilePhoneNumber;
                }

                if (owner.HetEquipment != null)
                {
                    ownerLite.EquipmentCount = CalculateEquipmentCount(owner.HetEquipment.ToList());
                }

                ownerLite.Status = owner.OwnerStatusType.Description;
            }

            return ownerLite;
        }

        /// <summary>
        /// Function to populate equipment count for this owner
        /// </summary>
        public static int CalculateEquipmentCount(List<HetEquipment> equipmentList)
        {
            int equipmentCount = 0;

            foreach (HetEquipment equipment in equipmentList)
            {
                if (equipment.EquipmentStatusType.EquipmentStatusTypeCode != HetEquipment.StatusArchived)
                {
                    ++equipmentCount;
                }
            }

            return equipmentCount;
        }

        #endregion

        #region Convert full owner record to a "Wcb/Cgl" report version

        /// <summary>
        /// Convert to Owner Wb Cgl Model
        /// </summary>
        /// <param name="owner"></param>
        public static OwnerWcbCgl ToWcbCglModel(HetOwner owner)
        {
            OwnerWcbCgl ownerLite = new OwnerWcbCgl();

            if (owner != null)
            {
                ownerLite.Id = owner.OwnerId;
                ownerLite.OwnerCode = owner.OwnerCode;
                ownerLite.OrganizationName = owner.OrganizationName;

                if (owner.LocalArea != null)
                {
                    ownerLite.ServiceAreaId = owner.LocalArea.ServiceArea.ServiceAreaId;
                    ownerLite.LocalAreaName = owner.LocalArea.Name;
                }

                if (owner.PrimaryContact != null)
                {
                    // set phone number
                    ownerLite.PrimaryContactNumber = owner.PrimaryContact.WorkPhoneNumber;

                    if (string.IsNullOrEmpty(ownerLite.PrimaryContactNumber))
                    {
                        ownerLite.PrimaryContactNumber = owner.PrimaryContact.MobilePhoneNumber;
                    }

                    // set mobile number
                    ownerLite.PrimaryContactCell = owner.PrimaryContact.MobilePhoneNumber;
                }

                ownerLite.WcbNumber = owner.WorkSafeBcpolicyNumber;
                ownerLite.WcbExpiryDate = owner.WorkSafeBcexpiryDate;
                ownerLite.CglNumber = owner.CglPolicyNumber;
                ownerLite.CglExpiryDate = owner.CglendDate;
            }

            return ownerLite;
        }

        #endregion

        #region Get Owner History

        public static List<History> GetHistoryRecords(int id, int? offset, int? limit, DbAppContext context)
        {
            HetOwner owner = context.HetOwner.AsNoTracking()
                .Include(x => x.HetHistory)
                .First(a => a.OwnerId == id);

            List<HetHistory> data = owner.HetHistory
                .OrderByDescending(y => y.AppLastUpdateTimestamp)
                .ToList();

            if (offset == null)
            {
                offset = 0;
            }

            if (limit == null)
            {
                limit = data.Count - offset;
            }

            List<History> result = new List<History>();

            for (int i = (int)offset; i < data.Count && i < offset + limit; i++)
            {
                History temp = new History();

                if (data[i] != null)
                {
                    temp.HistoryText = data[i].HistoryText;
                    temp.Id = data[i].HistoryId;
                    temp.LastUpdateTimestamp = data[i].AppLastUpdateTimestamp;
                    temp.LastUpdateUserid = data[i].AppLastUpdateUserid;
                    temp.AffectedEntityId = data[i].OwnerId;
                }

                result.Add(temp);
            }

            return result;
        }

        #endregion

        #region Owner Verification Data

        public static OwnerVerificationReportModel GetOwnerVerificationLetterData(
            DbAppContext context, int?[] localAreas, int?[] owners,
            int? equipmentStatusId, int? ownerStatusId)
        {
            try
            {
                OwnerVerificationReportModel model = new OwnerVerificationReportModel();

                // get owner records
                IQueryable<HetOwner> ownerRecords = context.HetOwner.AsNoTracking()
                    .Include(x => x.PrimaryContact)
                    .Include(x => x.Business)
                    .Include(x => x.HetEquipment)
                        .ThenInclude(a => a.HetEquipmentAttachment)
                    .Include(x => x.HetEquipment)
                        .ThenInclude(l => l.LocalArea)
                    .Include(x => x.HetEquipment)
                        .ThenInclude(y => y.DistrictEquipmentType)
                    .Include(x => x.LocalArea)
                        .ThenInclude(s => s.ServiceArea)
                            .ThenInclude(d => d.District)
                    .Where(x => x.OwnerStatusTypeId == ownerStatusId)
                    .OrderBy(x => x.LocalArea.Name).ThenBy(x => x.OrganizationName);

                if (owners?.Length > 0)
                {
                    ownerRecords = ownerRecords.Where(x => owners.Contains(x.OwnerId));
                }

                if (localAreas?.Length > 0)
                {
                    ownerRecords = ownerRecords.Where(x => localAreas.Contains(x.LocalAreaId));
                }

                // convert to list
                List<HetOwner> ownerList = ownerRecords.ToList();

                if (ownerList.Any())
                {
                    // get address and contact info
                    string address = ownerList[0].LocalArea.ServiceArea.Address;

                    if (!string.IsNullOrEmpty(address))
                    {
                        address = address.Replace("  ", " ");
                        address = address.Replace("amp;", "and");
                        address = address.Replace("|", " | ");
                    }
                    else
                    {
                        address = "";
                    }

                    string contact = $"Phone: {ownerList[0].LocalArea.ServiceArea.Phone} | Fax: {ownerList[0].LocalArea.ServiceArea.Fax}";

                    // generate pdf document name [unique portion only]
                    string fileName = "OwnerVerification";

                    // setup model document generation
                    model = new OwnerVerificationReportModel
                    {
                        ReportDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        Title = fileName,
                        DistrictId = ownerList[0].LocalArea.ServiceArea.District.DistrictId,
                        MinistryDistrictId = ownerList[0].LocalArea.ServiceArea.District.MinistryDistrictId,
                        DistrictName = ownerList[0].LocalArea.ServiceArea.District.Name,
                        DistrictAddress = address,
                        DistrictContact = contact,
                        LocalAreaName = ownerList[0].LocalArea.Name,
                        Owners = new List<HetOwner>()
                    };

                    // add owner records - must verify district ids too
                    foreach (HetOwner owner in ownerRecords)
                    {
                        if (owner.LocalArea.ServiceArea.District == null ||
                            owner.LocalArea.ServiceArea.District.DistrictId != model.DistrictId)
                        {
                            // missing district - data error [HETS-16]
                            throw new ArgumentException("Missing district id");
                        }

                        owner.ReportDate = model.ReportDate;
                        owner.Title = model.Title;
                        owner.DistrictId = model.DistrictId;
                        owner.MinistryDistrictId = model.MinistryDistrictId;
                        owner.DistrictName = model.DistrictName;
                        owner.DistrictAddress = model.DistrictAddress;
                        owner.DistrictContact = model.DistrictContact;
                        owner.LocalAreaName = model.LocalAreaName;

                        if (!string.IsNullOrEmpty(owner.SharedKey))
                        {
                            owner.SharedKeyHeader = "Secret Key: ";
                        }
                        else
                        {
                            //"Business Name: label and value instead"
                            owner.SharedKeyHeader = "Business Name: ";

                            if (owner.Business != null && !string.IsNullOrEmpty(owner.Business.BceidLegalName))
                            {
                                owner.SharedKeyHeader = "Business Name: " + owner.Business.BceidLegalName;
                            }
                        }

                        // strip out inactive and archived equipment
                        owner.HetEquipment = owner.HetEquipment.Where(x => x.EquipmentStatusTypeId == equipmentStatusId).ToList();

                        model.Owners.Add(owner);
                    }
                }

                return model;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion
    }
}
