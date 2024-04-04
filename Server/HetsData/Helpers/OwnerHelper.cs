using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HetsData.Entities;
using HetsData.Dtos;
using HetsCommon;

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
        public List<OwnerDto> Owners { get; set; }
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

        private DateTime? _wcbExpiryDate;
        public DateTime? WcbExpiryDate {
            get => _wcbExpiryDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _wcbExpiryDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        public string CglNumber { get; set; }

        private DateTime? _cglExpiryDate;
        public DateTime? CglExpiryDate {
            get => _cglExpiryDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _cglExpiryDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
    }

    public class OwnerLiteProjects
    {
        public int Id { get; set; }
        public string OwnerCode { get; set; }
        public string OrganizationName { get; set; }
        public int? LocalAreaId { get; set; }
        public IEnumerable<int?> ProjectIds { get; set; }
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
        public OwnerDto OwnerColumn1 { get; set; }
        public OwnerDto OwnerColumn2 { get; set; }
    }

    #endregion

    public static class OwnerHelper
    {
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

                if (owner.HetEquipments != null)
                {
                    ownerLite.EquipmentCount = CalculateEquipmentCount(owner.HetEquipments.ToList());
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
                ownerLite.WcbExpiryDate = 
                    owner.WorkSafeBcexpiryDate is DateTime wcbExpiryDateUtc ? 
                        DateUtils.AsUTC(wcbExpiryDateUtc) : null;
                ownerLite.CglNumber = owner.CglPolicyNumber;
                ownerLite.CglExpiryDate = 
                    owner.CglendDate is DateTime cglEndDateUtc ? 
                        DateUtils.AsUTC(cglEndDateUtc) : null;
            }

            return ownerLite;
        }

        #endregion
    }
}
