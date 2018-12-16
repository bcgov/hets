using System;
using System.Collections.Generic;
using System.Linq;
using HetsData.Model;
using Microsoft.EntityFrameworkCore;

namespace HetsData.Helpers
{
    #region Project Models

    public class ProjectLite
    {
        public int Id { get; set; }
        public HetDistrict District { get; set; }
        public string Name { get; set; }
        public HetContact PrimaryContact { get; set; }        
        public int? Hires { get; set; }
        public int? Requests { get; set; }
        public string Status { get; set; }
        public string ProvincialProjectNumber { get; set; }
    }

    public class ProjectLiteList
    {
        public int Id { get; set; }
        public string Name { get; set; }        
    }

    public class ProjectRentalAgreementClone
    {
        public int ProjectId { get; set; }
        public int AgreementToCloneId { get; set; }
        public int RentalAgreementId { get; set; }
    }

    #endregion

    public static class ProjectHelper
    {
        #region Get a Project record (plus associated records)

        /// <summary>
        /// Get a Project record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <param name="districtId"></param>
        /// <returns></returns>
        public static HetProject GetRecord(int id, DbAppContext context, int? districtId = 0)
        {            
            HetProject project = context.HetProject.AsNoTracking() 
                .Include(x => x.ProjectStatusType)
                .Include(x => x.District)
                    .ThenInclude(x => x.Region)
                .Include(x => x.HetContact)
                .Include(x => x.PrimaryContact)
                .Include(x => x.HetRentalRequest)
                    .ThenInclude(y => y.DistrictEquipmentType)
                .Include(x => x.HetRentalRequest)
                    .ThenInclude(y => y.RentalRequestStatusType)
                .Include(x => x.HetRentalRequest)
                    .ThenInclude(y => y.HetRentalRequestRotationList)
                .Include(x => x.HetRentalAgreement)
                    .ThenInclude(y => y.Equipment)
                        .ThenInclude(z => z.DistrictEquipmentType)
                .Include(x => x.HetRentalAgreement)
                    .ThenInclude(y => y.RentalAgreementStatusType)
                .FirstOrDefault(a => a.ProjectId == id);

            if (project != null)
            {
                project.Status = project.ProjectStatusType.ProjectStatusTypeCode;

                // calculate the number of hired (yes or forced hire) equipment
                // count active requests (In Progress)
                int countActiveRequests = 0;

                foreach (HetRentalRequest rentalRequest in project.HetRentalRequest)
                {
                    rentalRequest.Status = rentalRequest.RentalRequestStatusType.RentalRequestStatusTypeCode;

                    int temp = 0;

                    foreach (HetRentalRequestRotationList equipment in rentalRequest.HetRentalRequestRotationList)
                    {
                        if (equipment.OfferResponse != null &&
                            equipment.OfferResponse.ToLower().Equals("yes"))
                        {
                            temp++;
                        }

                        if (equipment.IsForceHire != null &&
                            equipment.IsForceHire == true)
                        {
                            temp++;
                        }
                    }

                    rentalRequest.YesCount = temp;
                    rentalRequest.HetRentalRequestRotationList = null;

                    if (rentalRequest.RentalRequestStatusType.RentalRequestStatusTypeCode == null ||
                        rentalRequest.RentalRequestStatusType.RentalRequestStatusTypeCode
                            .Equals(HetRentalRequest.StatusInProgress))
                    {
                        countActiveRequests++;
                    }
                }

                // count active agreements (Active)
                int countActiveAgreements = 0;

                foreach (HetRentalAgreement rentalAgreement in project.HetRentalAgreement)
                {
                    rentalAgreement.Status = rentalAgreement.RentalAgreementStatusType.RentalAgreementStatusTypeCode;

                    if (rentalAgreement.RentalAgreementStatusType.RentalAgreementStatusTypeCode == null ||
                        rentalAgreement.RentalAgreementStatusType.RentalAgreementStatusTypeCode
                            .Equals(HetRentalAgreement.StatusActive))
                    {
                        countActiveAgreements++;
                    }

                    // workaround for converted records from Bc Bid
                    if (rentalAgreement.Number.StartsWith("BCBid"))
                    {
                        rentalAgreement.RentalRequestId = -1;
                        rentalAgreement.RentalRequestRotationListId = -1;
                    }
                }

                // Only allow editing the "Status" field under the following conditions:
                // * If Project.status is currently "Active" AND                
                //   (All child RentalRequests.Status != "In Progress" AND All child RentalAgreement.status != "Active"))
                // * If Project.status is currently != "Active"                               
                if (project.ProjectStatusType.ProjectStatusTypeCode.Equals(HetProject.StatusActive) &&
                    (countActiveRequests > 0 || countActiveAgreements > 0))
                {
                    project.CanEditStatus = false;
                }
                else
                {
                    project.CanEditStatus = true;
                }
            }

            // get fiscal year
            if (districtId > 0)
            {                
                HetDistrictStatus status = context.HetDistrictStatus.AsNoTracking()
                    .First(x => x.DistrictId == districtId);

                int? fiscalYear = status.CurrentFiscalYear;

                // fiscal year in the status table stores the "start" of the year
                if (fiscalYear != null && project != null)
                {
                    DateTime fiscalYearStart = new DateTime((int) fiscalYear, 4, 1);
                    project.FiscalYearStartDate = fiscalYearStart;
                }
            }

            return project;
        }

        #endregion

        #region Convert full project record to a "Lite" version

        /// <summary>
        /// Convert to Project Lite Model
        /// </summary>
        /// <param name="project"></param>
        public static ProjectLite ToLiteModel(HetProject project)
        {
            ProjectLite projectLite = new ProjectLite();

            if (project != null)
            {
                projectLite.Id = project.ProjectId;
                projectLite.Status = project.ProjectStatusType?.Description;
                projectLite.Name = project.Name;
                projectLite.PrimaryContact = project.PrimaryContact;
                projectLite.District = project.District;                
                projectLite.Requests = project.HetRentalRequest?.Count;
                projectLite.Hires = project.HetRentalAgreement?.Count;
                projectLite.ProvincialProjectNumber = project.ProvincialProjectNumber;
            }

            return projectLite;
        }

        #endregion

        #region Get Project History

        public static List<History> GetHistoryRecords(int id, int? offset, int? limit, DbAppContext context)
        {
            HetProject project = context.HetProject.AsNoTracking()
                .Include(x => x.HetHistory)
                .First(a => a.ProjectId == id);

            List<HetHistory> data = project.HetHistory
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
                    temp.AffectedEntityId = data[i].ProjectId;
                }

                result.Add(temp);
            }

            return result;
        }

        #endregion
    }
}
