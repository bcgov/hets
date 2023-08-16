using AutoMapper;
using HetsData.Dtos;
using HetsData.Helpers;
using HetsData.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using HetsCommon;

namespace HetsData.Repositories
{
    public interface IProjectRepository
    {
        ProjectDto GetRecord(int projectId, int? districtId = 0);
        ProjectLite ToLiteModel(HetProject project);
    }
    public class ProjectRepository : IProjectRepository
    {
        private DbAppContext _dbContext;
        private IMapper _mapper;

        public ProjectRepository(DbAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a Project record
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="districtId"></param>
        /// <returns></returns>
        public ProjectDto GetRecord(int projectId, int? districtId = 0)
        {
            HetProject project = _dbContext.HetProjects.AsNoTracking()
                .Include(x => x.ProjectStatusType)
                .Include(x => x.District)
                    .ThenInclude(x => x.Region)
                .Include(x => x.HetContacts)
                .Include(x => x.PrimaryContact)
                .Include(x => x.HetRentalRequests)
                    .ThenInclude(y => y.DistrictEquipmentType)
                .Include(x => x.HetRentalRequests)
                    .ThenInclude(y => y.RentalRequestStatusType)
                .Include(x => x.HetRentalRequests)
                    .ThenInclude(y => y.HetRentalRequestRotationLists)
                .Include(x => x.HetRentalRequests)
                    .ThenInclude(y => y.LocalArea)
                .Include(x => x.HetRentalAgreements)
                    .ThenInclude(y => y.Equipment)
                        .ThenInclude(z => z.DistrictEquipmentType)
                .Include(x => x.HetRentalAgreements)
                    .ThenInclude(y => y.RentalAgreementStatusType)
                .Include(x => x.HetRentalAgreements)
                    .ThenInclude(y => y.Equipment)
                        .ThenInclude(z => z.LocalArea)
                .FirstOrDefault(a => a.ProjectId == projectId);

            if (project != null)
            {
                project.Status = project.ProjectStatusType.ProjectStatusTypeCode;

                // calculate the number of hired (yes or forced hire) equipment
                // count active requests (In Progress)
                int countActiveRequests = 0;

                foreach (HetRentalRequest rentalRequest in project.HetRentalRequests)
                {
                    rentalRequest.Status = rentalRequest.RentalRequestStatusType.RentalRequestStatusTypeCode;

                    int temp = 0;

                    foreach (HetRentalRequestRotationList equipment in rentalRequest.HetRentalRequestRotationLists)
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
                    rentalRequest.HetRentalRequestRotationLists = null;

                    if (rentalRequest.RentalRequestStatusType.RentalRequestStatusTypeCode == null ||
                        rentalRequest.RentalRequestStatusType.RentalRequestStatusTypeCode
                            .Equals(HetRentalRequest.StatusInProgress))
                    {
                        countActiveRequests++;
                    }
                }

                // count active agreements (Active)
                int countActiveAgreements = 0;

                foreach (HetRentalAgreement rentalAgreement in project.HetRentalAgreements)
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

                    if (rentalAgreement.Equipment.LocalArea != null)
                    {
                        rentalAgreement.LocalAreaName = rentalAgreement.Equipment.LocalArea.Name;
                    }
                }

                foreach (HetRentalRequest rentalRequest in project.HetRentalRequests)
                {
                    if (rentalRequest.LocalArea != null)
                    {
                        rentalRequest.LocalAreaName = rentalRequest.LocalArea.Name;
                    }
                }

                //To make rental agreement lightweight
                foreach (HetRentalAgreement rentalAgreement in project.HetRentalAgreements)
                {
                    rentalAgreement.Equipment.LocalArea = null;
                }

                //To make rental request lightweight
                foreach (HetRentalRequest rentalRequest in project.HetRentalRequests)
                {
                    rentalRequest.LocalArea = null;
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
                HetDistrictStatus status = _dbContext.HetDistrictStatuses.AsNoTracking()
                    .First(x => x.DistrictId == districtId);

                int? fiscalYear = status.CurrentFiscalYear;

                // fiscal year in the status table stores the "start" of the year
                if (fiscalYear != null && project != null)
                {
                    DateTime fiscalYearStart = DateUtils.ConvertPacificToUtcTime(
                        new DateTime((int)fiscalYear, 4, 1, 0, 0, 0));

                    project.FiscalYearStartDate = fiscalYearStart;
                }
            }

            return _mapper.Map<ProjectDto>(project);
        }

        /// <summary>
        /// Convert to Project Lite Model
        /// </summary>
        /// <param name="project"></param>
        public ProjectLite ToLiteModel(HetProject project)
        {
            ProjectLite projectLite = new ProjectLite();

            if (project != null)
            {
                projectLite.Id = project.ProjectId;
                projectLite.Status = project.ProjectStatusType?.Description;
                projectLite.Name = project.Name;
                projectLite.PrimaryContact = _mapper.Map<ContactDto>(project.PrimaryContact);
                projectLite.District = _mapper.Map<DistrictDto>(project.District);
                projectLite.Requests = project.HetRentalRequests?.Count;
                projectLite.Hires = project.HetRentalAgreements?.Count;
                projectLite.ProvincialProjectNumber = project.ProvincialProjectNumber;
                projectLite.FiscalYear = project.FiscalYear;
            }

            return projectLite;
        }
    }
}
