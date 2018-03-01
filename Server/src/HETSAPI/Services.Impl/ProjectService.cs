using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Mappings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Project Rental Agreement Clone Class - required to clone a previous agreement
    /// </summary>
    public class ProjectRentalAgreementClone
    {
        public int ProjectId { get; set; }
        public int AgreementToCloneId { get; set; }
        public int RentalAgreementId { get; set; }
    }

    /// <summary>
    /// Project Service
    /// </summary>
    public class ProjectService : ServiceBase, IProjectService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Project Service Constructor
        /// </summary>
        public ProjectService(IHttpContextAccessor httpContextAccessor, DbAppContext context, IConfiguration configuration) : base(httpContextAccessor, context)
        {
            _context = context;
            _configuration = configuration;
        }

        private void AdjustRecord(Project item)
        {
            if (item == null) return;

            // adjust district
            if (item.District != null)
            {
                item.District = _context.Districts.FirstOrDefault(a => a.Id == item.District.Id);
            }

            // notes list
            if (item.Notes != null)
            {
                for (int i = 0; i < item.Notes.Count; i++)
                {
                    if (item.Notes[i] != null)
                    {
                        item.Notes[i] = _context.Notes.FirstOrDefault(a => a.Id == item.Notes[i].Id);
                    }
                }
            }

            // history list
            if (item.History != null)
            {
                for (int i = 0; i < item.History.Count; i++)
                {
                    if (item.History[i] != null)
                    {
                        item.History[i] = _context.Historys.FirstOrDefault(a => a.Id == item.History[i].Id);
                    }
                }
            }

            if (item.PrimaryContact != null)
            {
                item.PrimaryContact = _context.Contacts.FirstOrDefault(a => a.Id == item.PrimaryContact.Id);
            }

            if (item.Contacts != null)
            {
                for (int i = 0; i < item.Contacts.Count; i++)
                {
                    if (item.Contacts[i] != null)
                    {
                        item.Contacts[i] = _context.Contacts.FirstOrDefault(a => a.Id == item.Contacts[i].Id);
                    }
                }
            }

            if (item.RentalRequests != null)
            {
                for (int i = 0; i < item.RentalRequests.Count; i++)
                {
                    if (item.RentalRequests[i] != null)
                    {
                        item.RentalRequests[i] = _context.RentalRequests.FirstOrDefault(a => a.Id == item.RentalRequests[i].Id);
                    }
                }
            }

            // rental agreements list
            if (item.RentalAgreements != null)
            {
                for (int i = 0; i < item.RentalAgreements.Count; i++)
                {
                    if (item.RentalAgreements[i] != null)
                    {
                        item.RentalAgreements[i] = _context.RentalAgreements.FirstOrDefault(a => a.Id == item.RentalAgreements[i].Id);
                    }
                }
            }
        }

        /// <summary>
        /// Create bulk project records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Project created</response>
        public virtual IActionResult ProjectsBulkPostAsync(Project[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (Project item in items)
            {
                AdjustRecord(item);

                // determine if this is an insert or an update
                bool exists = _context.Projects.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.Projects.Update(item);
                }
                else
                {
                    _context.Projects.Add(item);
                }

                // save the changes
                _context.SaveChanges();
            }

            return new NoContentResult();
        }

        /// <summary>
        /// Get project by id
        /// </summary>
        /// <param name="id">id of Project to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult ProjectsIdGetAsync(int id)
        {
            bool exists = _context.Projects.Any(a => a.Id == id);

            if (exists)
            {
                Project result = GetProjectDetais(id);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        private Project GetProjectDetais(int id)
        {            
            Project result = _context.Projects.AsNoTracking()
                .Include(x => x.District.Region)
                .Include(x => x.Contacts)
                .Include(x => x.PrimaryContact)
                .Include(x => x.RentalRequests)
                    .ThenInclude(e => e.DistrictEquipmentType)
                        .ThenInclude(d => d.EquipmentType)
                .Include(x => x.RentalRequests)
                    .ThenInclude(y => y.RentalRequestRotationList)
                .Include(x => x.RentalAgreements)
                    .ThenInclude(e => e.Equipment)
                        .ThenInclude(d => d.DistrictEquipmentType)
                .First(a => a.Id == id);

            // calculate the number of hired (yes, forced hire) equipment
            // count active requests (In Progress)
            int countActiveRequests = 0;

            foreach (RentalRequest rentalRequest in result.RentalRequests)
            {
                int temp = 0;

                foreach (RentalRequestRotationList equipment in rentalRequest.RentalRequestRotationList)
                {
                    if (equipment.OfferResponse != null &&
                        equipment.OfferResponse.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
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
                rentalRequest.RentalRequestRotationList = null;

                if (rentalRequest.Status == null ||
                    rentalRequest.Status.Equals("In Progress", StringComparison.InvariantCultureIgnoreCase))
                {
                    countActiveRequests++;
                }
            }

            // count active agreements (Active)
            int countActiveAgreements = 0;

            foreach (RentalAgreement rentalAgreement in result.RentalAgreements)
            {
                if (rentalAgreement.Status == null ||
                    rentalAgreement.Status.Equals("Active", StringComparison.InvariantCultureIgnoreCase))
                {
                    countActiveAgreements++;
                }
            }

            // Only allow editing the "Status" field under the following conditions:
            // * If Project.status is currently "Active" AND                
            //   (All child RentalRequests.Status != "In Progress" AND All child RentalAgreement.status != "Active"))
            // * If Project.status is currently != "Active"                               
            if (result.Status.Equals("Active", StringComparison.InvariantCultureIgnoreCase) &&
                (countActiveRequests > 0 || countActiveAgreements > 0))
            {
                result.CanEditStatus = false;
            }
            else
            {
                result.CanEditStatus = true;
            }

            return result;
        }

        /// <summary>
        /// Update project
        /// </summary>
        /// <param name="id">id of Project to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult ProjectsIdPutAsync(int id, Project item)
        {
            AdjustRecord(item);

            bool exists = _context.Projects.Any(a => a.Id == id);

            if (exists && id == item.Id)
            {
                _context.Projects.Update(item);

                // save the changes
                _context.SaveChanges();

                Project result = GetProjectDetais(id);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create project
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Project created</response>
        public virtual IActionResult ProjectsPostAsync(Project item)
        {
            if (item != null)
            {
                AdjustRecord(item);

                bool exists = _context.Projects.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.Projects.Update(item);
                }
                else
                {
                    // record not found
                    _context.Projects.Add(item);
                }

                // save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // no record to insert
            return new ObjectResult(new HetsResponse("HETS-04", ErrorViewModel.GetDescription("HETS-04", _configuration)));
        }

        /// <summary>
        /// Search Projects
        /// </summary>
        /// <remarks>Used for the project search page.</remarks>
        /// <param name="districts">Districts (comma seperated list of id numbers)</param>
        /// <param name="project">name or partial name for a Project</param>
        /// <param name="hasRequests">if true then only include Projects with active Requests</param>
        /// <param name="hasHires">if true then only include Projects with active Rental Agreements</param>
        /// <param name="status">if included, filter the results to those with a status matching this string</param>
        /// <param name="projectNumber"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult ProjectsSearchGetAsync(string districts, string project, bool? hasRequests, bool? hasHires, string status, string projectNumber)
        {
            int?[] districtTokens = ParseIntArray(districts);

            // default search results must be limited to user
            int? districtId = _context.GetDistrictIdByUserId(GetCurrentUserId()).Single();

            IQueryable<Project> data = _context.Projects.AsNoTracking()                
                .Include(x => x.District.Region)
                .Include(x => x.PrimaryContact)
                .Include(x => x.RentalAgreements)
                .Include(x => x.RentalRequests)
                .Where(x => x.DistrictId.Equals(districtId));

            // **********************************************************************
            // filter results based on search critera
            // **********************************************************************
            if (districtTokens != null && districts.Length > 0)
            {
                data = data.Where(x => districtTokens.Contains(x.District.Id));
            }
            
            if (project != null)
            {
                // allow for case insensitive search of project name
                data = data.Where(x => x.Name.ToLowerInvariant().Contains(project.ToLowerInvariant()));
            }

            if (status != null)
            {
                data = data.Where(x => String.Equals(x.Status, status, StringComparison.CurrentCultureIgnoreCase));
            }

            // project number
            if (projectNumber != null)
            {
                // allow for case insensitive search of project name
                data = data.Where(x => String.Equals(x.ProvincialProjectNumber, projectNumber, StringComparison.CurrentCultureIgnoreCase));
            }

            // **********************************************************************
            // convert Project Model to View Model
            // **********************************************************************
            List<ProjectSearchResultViewModel> result = new List<ProjectSearchResultViewModel>();

            foreach (Project item in data)
            {
                result.Add(item.ToViewModel());
            }            

            return new ObjectResult(new HetsResponse(result));
        }

        # region Clone Project Agreements

        /// <summary>
        /// Get renatal agreements associated with a project by id
        /// </summary>
        /// <param name="id">id of Project to fetch agreements for</param>
        /// <response code="200">OK</response>
        public virtual IActionResult ProjectsIdGetAgreementsAsync(int id)
        {
            bool exists = _context.Projects.Any(a => a.Id == id);

            if (exists)
            {
                List<RentalAgreement> result = _context.Projects.AsNoTracking()
                    .Include(x => x.RentalAgreements)
                        .ThenInclude(e => e.Equipment)
                            .ThenInclude(d => d.DistrictEquipmentType)
                    .Include(x => x.RentalAgreements)
                        .ThenInclude(e => e.Equipment)
                            .ThenInclude(a => a.EquipmentAttachments)
                    .First(x => x.Id == id)
                    .RentalAgreements
                    .ToList();

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update a rental agreement by cloning a previous project rental agreement
        /// </summary>
        /// <param name="id">Project id</param>
        /// <param name="item"></param>
        /// <response code="200">Rental Agreement updated</response>
        public virtual IActionResult ProjectsRentalAgreementClonePostAsync(int id, ProjectRentalAgreementClone item)
        {
            if (item != null && id == item.ProjectId)
            {
                bool exists = _context.Projects.Any(a => a.Id == id);

                if (!exists)
                {
                   // record not found - project
                    return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                }
                
                Project project = _context.Projects
                    .Include(x => x.RentalAgreements)
                        .ThenInclude(y => y.RentalAgreementRates)
                    .Include(x => x.RentalAgreements)
                        .ThenInclude(y => y.RentalAgreementConditions)
                    .Include(x => x.RentalAgreements)
                        .ThenInclude(y => y.TimeRecords)
                    .First(a => a.Id == id);

                // check that the rental agreements exist
                exists = project.RentalAgreements.Any(a => a.Id == item.RentalAgreementId);

                if (!exists)
                {
                    // (RENTAL AGREEMENT) record not found
                    return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                }

                // check that the rental agreement to clone exist
                exists = project.RentalAgreements.Any(a => a.Id == item.AgreementToCloneId);

                if (!exists)
                {
                    // (RENTAL AGREEMENT) record not found
                    return new ObjectResult(new HetsResponse("HETS-11", ErrorViewModel.GetDescription("HETS-11", _configuration)));
                }

                int agreementToCloneIndex = project.RentalAgreements.FindIndex(a => a.Id == item.AgreementToCloneId);
                int newRentalagreementIndex = project.RentalAgreements.FindIndex(a => a.Id == item.RentalAgreementId);

                // ******************************************************************
                // Business Rules in the backend:
                // *Can't clone into an Agreement if it isn't Active
                // *Can't clone into an Agreement if it has existing time records
                // ******************************************************************
                if (!project.RentalAgreements[newRentalagreementIndex].Status
                    .Equals("Active", StringComparison.InvariantCultureIgnoreCase))
                {
                    // (RENTAL AGREEMENT) is not active
                    return new ObjectResult(new HetsResponse("HETS-12", ErrorViewModel.GetDescription("HETS-12", _configuration)));
                }

                if (project.RentalAgreements[newRentalagreementIndex].TimeRecords != null &&
                    project.RentalAgreements[newRentalagreementIndex].TimeRecords.Count > 0)
                {
                    // (RENTAL AGREEMENT) has tme records
                    return new ObjectResult(new HetsResponse("HETS-13", ErrorViewModel.GetDescription("HETS-13", _configuration)));
                }

                // ******************************************************************
                // clone agreement
                // ******************************************************************
                // update agreement attributes                
                project.RentalAgreements[newRentalagreementIndex].EquipmentRate =
                    project.RentalAgreements[agreementToCloneIndex].EquipmentRate;

                project.RentalAgreements[newRentalagreementIndex].Note =
                    project.RentalAgreements[agreementToCloneIndex].Note;                

                project.RentalAgreements[newRentalagreementIndex].RateComment =
                    project.RentalAgreements[agreementToCloneIndex].RateComment;

                project.RentalAgreements[newRentalagreementIndex].RatePeriod =
                    project.RentalAgreements[agreementToCloneIndex].RatePeriod;
                
                // update rates
                project.RentalAgreements[newRentalagreementIndex].RentalAgreementRates = null;

                foreach (RentalAgreementRate rate in project.RentalAgreements[agreementToCloneIndex].RentalAgreementRates)
                {
                    RentalAgreementRate temp = new RentalAgreementRate
                    {
                        Comment = rate.Comment,
                        ComponentName = rate.ComponentName,
                        Rate = rate.Rate,
                        RatePeriod = rate.RatePeriod,
                        IsIncludedInTotal = rate.IsIncludedInTotal,
                        IsAttachment = rate.IsAttachment,
                        PercentOfEquipmentRate = rate.PercentOfEquipmentRate
                    };

                    if (project.RentalAgreements[newRentalagreementIndex].RentalAgreementRates == null)
                    {
                        project.RentalAgreements[newRentalagreementIndex].RentalAgreementRates = 
                            new List<RentalAgreementRate>();
                    }

                    project.RentalAgreements[newRentalagreementIndex].RentalAgreementRates.Add(temp);                       
                }

                // update conditions
                project.RentalAgreements[newRentalagreementIndex].RentalAgreementConditions = null;

                foreach (RentalAgreementCondition condition in project.RentalAgreements[agreementToCloneIndex].RentalAgreementConditions)
                {
                    RentalAgreementCondition temp = new RentalAgreementCondition
                    {
                        Comment = condition.Comment,
                        ConditionName = condition.ConditionName                        
                    };

                    if (project.RentalAgreements[newRentalagreementIndex].RentalAgreementConditions == null)
                    {
                        project.RentalAgreements[newRentalagreementIndex].RentalAgreementConditions =
                            new List<RentalAgreementCondition>();
                    }

                    project.RentalAgreements[newRentalagreementIndex].RentalAgreementConditions.Add(temp);
                }

                // save the changes
                _context.SaveChanges();

                // ******************************************************************
                // return update rental agreement to update the screen
                // ******************************************************************
                RentalAgreement result = _context.RentalAgreements.AsNoTracking()
                    .Include(x => x.Equipment)
                        .ThenInclude(y => y.Owner)
                    .Include(x => x.Equipment)
                        .ThenInclude(y => y.DistrictEquipmentType)
                            .ThenInclude(d => d.EquipmentType)
                    .Include(x => x.Equipment)
                        .ThenInclude(y => y.EquipmentAttachments)
                    .Include(x => x.Equipment)
                        .ThenInclude(y => y.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.Project)
                        .ThenInclude(p => p.District.Region)
                    .Include(x => x.RentalAgreementConditions)
                    .Include(x => x.RentalAgreementRates)
                    .Include(x => x.TimeRecords)
                    .First(a => a.Id == item.RentalAgreementId);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        #endregion

        #region Project Time Records

        /// <summary>
        /// Get time records associated with project
        /// </summary>
        /// <param name="id">id of Owner to fetch Time Records for</param>
        /// <response code="200">OK</response>
        public virtual IActionResult ProjectsIdTimeRecordsGetAsync(int id)
        {
            bool exists = _context.Projects.Any(a => a.Id == id);

            if (exists)
            {
                Project project = _context.Projects.AsNoTracking()
                    .Include(x => x.RentalAgreements)
                        .ThenInclude(t => t.TimeRecords)
                    .First(x => x.Id == id);

                List<TimeRecord> timeRecords = new List<TimeRecord>();

                foreach (RentalAgreement rentalAgreement in project.RentalAgreements)
                {
                    timeRecords.AddRange(rentalAgreement.TimeRecords);
                }
                
                return new ObjectResult(new HetsResponse(timeRecords));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update or create a time record associated with a project
        /// </summary>
        /// <remarks>Update a Project&#39;s Time Record</remarks>
        /// <param name="id">id of Project to update Time Records for</param>
        /// <param name="item">Project Time Record</param>
        /// <response code="200">OK</response>
        public virtual IActionResult ProjectsIdTimeRecordsPostAsync(int id, TimeRecord item)
        {
            bool exists = _context.Projects.Any(a => a.Id == id);

            if (exists && item != null)
            {
                Project project = _context.Projects
                    .Include(x => x.RentalAgreements)
                        .ThenInclude(y => y.TimeRecords)
                    .First(x => x.Id == id);

                // ******************************************************************
                // must have the valid rental agreement id
                // ******************************************************************
                if (item.RentalAgreement.Id == 0)
                {
                    // (RENTAL AGREEMENT) record not found
                    return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                }

                exists = project.RentalAgreements.Any(a => a.Id == item.RentalAgreement.Id);

                if (!exists)
                {
                    // (RENTAL AGREEMENT) record not found
                    return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                }

                // ******************************************************************
                // add or update time record
                // ******************************************************************
                int rentalAgreementId = item.RentalAgreement.Id;
                int indexRental = project.RentalAgreements.FindIndex(a => a.Id == rentalAgreementId);

                if (item.Id > 0)
                {
                    _context.TimeRecords.Update(item);

                    int timeIndex = project.RentalAgreements[indexRental].TimeRecords.FindIndex(a => a.Id == item.Id);

                    if (timeIndex < 0)
                    {
                        // record not found
                        return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                    }

                    project.RentalAgreements[indexRental].TimeRecords[timeIndex].EnteredDate = DateTime.Now.ToUniversalTime();
                    project.RentalAgreements[indexRental].TimeRecords[timeIndex].Hours = item.Hours;
                    project.RentalAgreements[indexRental].TimeRecords[timeIndex].TimePeriod = item.TimePeriod;
                    project.RentalAgreements[indexRental].TimeRecords[timeIndex].WorkedDate = item.WorkedDate;
                }
                else // add time record
                {                    
                    item.EnteredDate = DateTime.Now.ToUniversalTime();

                    project.RentalAgreements[indexRental].TimeRecords.Add(item);
                }
                
                _context.SaveChanges();
                              
                // *************************************************************
                // return updated time records
                // *************************************************************
                List<TimeRecord> timeRecords = new List<TimeRecord>();

                foreach (RentalAgreement rentalAgreement in project.RentalAgreements)
                {
                    timeRecords.AddRange(rentalAgreement.TimeRecords);
                }

                return new ObjectResult(new HetsResponse(timeRecords));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update or create an array of time records associated with a project
        /// </summary>
        /// <remarks>Update a Project&#39;s Time Records</remarks>
        /// <param name="id">id of Project to update Time Records for</param>
        /// <param name="items">Array of Project Time Records</param>
        /// <response code="200">OK</response>
        public virtual IActionResult ProjectsIdTimeRecordsBulkPostAsync(int id, TimeRecord[] items)
        {
            bool exists = _context.Projects.Any(a => a.Id == id);

            if (exists && items != null)
            {
                Project project = _context.Projects
                    .Include(x => x.RentalAgreements)
                    .ThenInclude(t => t.TimeRecords)
                    .First(x => x.Id == id);

                // process each time record
                foreach (TimeRecord item in items)
                {
                    // ******************************************************************
                    // must have the valid rental agreement id
                    // ******************************************************************
                    if (item.RentalAgreement.Id == 0)
                    {
                        // (RENTAL AGREEMENT) record not found
                        return new ObjectResult(new HetsResponse("HETS-01",
                            ErrorViewModel.GetDescription("HETS-01", _configuration)));
                    }

                    exists = project.RentalAgreements.Any(a => a.Id == item.RentalAgreement.Id);

                    if (!exists)
                    {
                        // (RENTAL AGREEMENT) record not found
                        return new ObjectResult(new HetsResponse("HETS-01",
                            ErrorViewModel.GetDescription("HETS-01", _configuration)));
                    }

                    // ******************************************************************
                    // add or update time record
                    // ******************************************************************   
                    int rentalAgreementId = item.RentalAgreement.Id;
                    int indexRental = project.RentalAgreements.FindIndex(a => a.Id == rentalAgreementId);

                    if (item.Id > 0)
                    {
                        _context.TimeRecords.Update(item);

                        int timeIndex = project.RentalAgreements[indexRental].TimeRecords
                            .FindIndex(a => a.Id == item.Id);

                        if (timeIndex < 0)
                        {
                            // record not found
                            return new ObjectResult(new HetsResponse("HETS-01",
                                ErrorViewModel.GetDescription("HETS-01", _configuration)));
                        }

                        project.RentalAgreements[indexRental].TimeRecords[timeIndex].EnteredDate = DateTime.Now.ToUniversalTime();
                        project.RentalAgreements[indexRental].TimeRecords[timeIndex].Hours = item.Hours;
                        project.RentalAgreements[indexRental].TimeRecords[timeIndex].TimePeriod = item.TimePeriod;
                        project.RentalAgreements[indexRental].TimeRecords[timeIndex].WorkedDate = item.WorkedDate;
                    }
                    else // add time record
                    {
                        item.EnteredDate = DateTime.Now.ToUniversalTime();

                        project.RentalAgreements[indexRental].TimeRecords.Add(item);
                    }

                    _context.SaveChanges();
                }

                // *************************************************************
                // return updated time records
                // *************************************************************                
                List<TimeRecord> timeRecords = new List<TimeRecord>();

                foreach (RentalAgreement rentalAgreement in project.RentalAgreements)
                {
                    timeRecords.AddRange(rentalAgreement.TimeRecords);
                }

                return new ObjectResult(new HetsResponse(timeRecords));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        #endregion

        #region Project Equipment

        /// <summary>
        /// Get equipment associated with a project
        /// </summary>
        /// <remarks>Gets a Project&#39;s Equipment</remarks>
        /// <param name="id">id of Project to fetch Equipment for</param>
        /// <response code="200">OK</response>
        public virtual IActionResult ProjectsIdEquipmentGetAsync(int id)
        {
            bool exists = _context.Projects.Any(a => a.Id == id);

            if (exists)
            {
                Project project = _context.Projects.AsNoTracking()
                    .Include(x => x.RentalAgreements)
                        .ThenInclude(e => e.Equipment)
                        .ThenInclude(o => o.Owner)
                        .ThenInclude(c => c.PrimaryContact)
                    .First(x => x.Id == id);

                return new ObjectResult(new HetsResponse(project.RentalAgreements));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        #endregion

        #region Project Contacts

        /// <summary>
        /// Get contacts associated with project
        /// </summary>
        /// <param name="id">id of Owner to fetch Contacts for</param>
        /// <response code="200">OK</response>
        public virtual IActionResult ProjectsIdContactsGetAsync(int id)
        {
            bool exists = _context.Projects.Any(a => a.Id == id);

            if (exists)
            {
                Project project = _context.Projects.AsNoTracking()
                    .Include(x => x.District.Region)
                    .Include(x => x.Notes)
                    .Include(x => x.Attachments)
                    .Include(x => x.History)
                    .Include(x => x.Contacts)
                    .First(x => x.Id == id);

                return new ObjectResult(new HetsResponse(project.Contacts));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update a contact associated with a project
        /// </summary>
        /// <remarks>Updates a Contact associated with a Project</remarks>
        /// <param name="id">id of Project to add Contact to</param>
        /// <param name="item">Project contact</param>
        /// <param name="primary"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult ProjectsIdContactsPostAsync(int id, Contact item, bool primary)
        {
            bool exists = _context.Projects.Any(a => a.Id == id);

            if (exists && item != null)
            {
                Project project = _context.Projects
                    .Include(x => x.Contacts)
                    .First(x => x.Id == id);

                // ******************************************************************
                // add or update contact
                // ******************************************************************
                if (item.Id > 0)
                {
                    int contactIndex = project.Contacts.FindIndex(a => a.Id == item.Id);

                    if (contactIndex < 0)
                    {
                        // record not found
                        return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                    }

                    project.Contacts[contactIndex].Notes = item.Notes;
                    project.Contacts[contactIndex].Address1 = item.Address1;
                    project.Contacts[contactIndex].Address2 = item.Address2;
                    project.Contacts[contactIndex].City = item.City;
                    project.Contacts[contactIndex].EmailAddress = item.EmailAddress;
                    project.Contacts[contactIndex].FaxPhoneNumber = item.FaxPhoneNumber;
                    project.Contacts[contactIndex].GivenName = item.GivenName;
                    project.Contacts[contactIndex].MobilePhoneNumber = item.MobilePhoneNumber;
                    project.Contacts[contactIndex].PostalCode = item.PostalCode;
                    project.Contacts[contactIndex].Province = item.Province;
                    project.Contacts[contactIndex].Surname = item.Surname;
                    project.Contacts[contactIndex].Role = item.Role;

                    if (primary)
                    {
                        project.PrimaryContactId = item.Id;
                    }
                }
                else  // add note
                {                    
                    project.Contacts.Add(item);

                    _context.SaveChanges();

                    if (primary)
                    {
                        project.PrimaryContactId = item.Id;
                    }
                }

                _context.SaveChanges();
                
                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update contacts associated with a project
        /// </summary>
        /// <remarks>Replaces an Project&#39;s Contacts</remarks>
        /// <param name="id">id of Project to replace Contacts for</param>
        /// <param name="items">Replacement Project contacts.</param>
        /// <response code="200">OK</response>
        public virtual IActionResult ProjectsIdContactsPutAsync(int id, Contact[] items)
        {
            bool exists = _context.Projects.Any(a => a.Id == id);

            if (exists && items != null)
            {
                Project project = _context.Projects                    
                    .Include(x => x.Contacts)
                    .First(x => x.Id == id);

                // adjust the incoming list
                for (int i = 0; i < items.Length; i++)
                {
                    Contact item = items[i];

                    if (item != null)
                    {
                        bool contactExists = _context.Contacts.Any(x => x.Id == item.Id);

                        if (contactExists)
                        {
                            items[i] = _context.Contacts
                                .First(x => x.Id == item.Id);
                        }
                        else
                        {
                            _context.Add(item);
                            items[i] = item;
                        }
                    }
                }

                // remove contacts that are no longer attached
                foreach (Contact contact in project.Contacts)
                {
                    if (contact != null && items.All(x => x.Id != contact.Id))
                    {
                        _context.Remove(contact);
                    }
                }

                // replace contacts
                project.Contacts = items.ToList();
                _context.Update(project);
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(items));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        #endregion

        #region Project Attachments

        /// <summary>
        /// Get attachments associated with a project
        /// </summary>
        /// <remarks>Returns attachments for a particular Project</remarks>
        /// <param name="id">id of Project to fetch attachments for</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult ProjectsIdAttachmentsGetAsync(int id)
        {
            bool exists = _context.Projects.Any(a => a.Id == id);

            if (exists)
            {
                Project project = _context.Projects.AsNoTracking()
                    .Include(x => x.Attachments)
                    .First(a => a.Id == id);

                List<AttachmentViewModel> result = MappingExtensions.GetAttachmentListAsViewModel(project.Attachments);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        #endregion

        #region Project History

        /// <summary>
        /// Get project history
        /// </summary>
        /// <remarks>Returns History for a particular Project</remarks>
        /// <param name="id">id of Project to fetch History for</param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult ProjectsIdHistoryGetAsync(int id, int? offset, int? limit)
        {
            bool exists = _context.Projects.Any(a => a.Id == id);

            if (exists)
            {
                Project project = _context.Projects.AsNoTracking()
                    .Include(x => x.History)
                    .First(a => a.Id == id);

                List<History> data = project.History.OrderByDescending(y => y.AppLastUpdateTimestamp).ToList();

                if (offset == null)
                {
                    offset = 0;
                }

                if (limit == null)
                {
                    limit = data.Count - offset;
                }

                List<HistoryViewModel> result = new List<HistoryViewModel>();

                for (int i = (int)offset; i < data.Count && i < offset + limit; i++)
                {
                    result.Add(data[i].ToViewModel(id));
                }

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create a history record associated with a project
        /// </summary>
        /// <remarks>Add a History record to the Project</remarks>
        /// <param name="id">id of Project to fetch History for</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="201">History created</response>
        public virtual IActionResult ProjectsIdHistoryPostAsync(int id, History item)
        {
            HistoryViewModel result = new HistoryViewModel();

            bool exists = _context.Projects.Any(a => a.Id == id);

            if (exists)
            {
                Project project = _context.Projects
                    .Include(x => x.History)
                    .First(a => a.Id == id);

                if (project.History == null)
                {
                    project.History = new List<History>();
                }

                // force add
                item.Id = 0;
                project.History.Add(item);
                _context.Projects.Update(project);
                _context.SaveChanges();
            }

            result.HistoryText = item.HistoryText;
            result.Id = item.Id;
            result.LastUpdateTimestamp = item.AppLastUpdateTimestamp;
            result.LastUpdateUserid = item.AppLastUpdateUserid;
            result.AffectedEntityId = id;

            return new ObjectResult(new HetsResponse(result));
        }

        #endregion

        #region Project Note Records

        /// <summary>
        /// Get note records associated with project
        /// </summary>
        /// <param name="id">id of Project to fetch Notes for</param>
        /// <response code="200">OK</response>
        public virtual IActionResult ProjectsIdNotesGetAsync(int id)
        {
            bool exists = _context.Projects.Any(a => a.Id == id);

            if (exists)
            {
                Project project = _context.Projects.AsNoTracking()
                    .Include(x => x.Notes)
                    .First(x => x.Id == id);                

                List<Note> notes = new List<Note>();

                foreach (Note note in project.Notes)
                {
                    if (note.IsNoLongerRelevant == false)
                    {
                        notes.Add(note);
                    }
                }

                return new ObjectResult(new HetsResponse(notes));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update or create a note associated with a project
        /// </summary>
        /// <remarks>Update a Project&#39;s Notes</remarks>
        /// <param name="id">id of Project to update Notes for</param>
        /// <param name="item">Project Note</param>
        /// <response code="200">OK</response>
        public virtual IActionResult ProjectsIdNotesPostAsync(int id, Note item)
        {
            bool exists = _context.Projects.Any(a => a.Id == id);

            if (exists && item != null)
            {                
                Project project = _context.Projects
                    .Include(x => x.Notes)
                    .First(x => x.Id == id);
                
                // ******************************************************************
                // add or update note
                // ******************************************************************
                if (item.Id > 0)
                {
                    int noteIndex = project.Notes.FindIndex(a => a.Id == item.Id);

                    if (noteIndex < 0)
                    {
                        // record not found
                        return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                    }

                    project.Notes[noteIndex].Text = item.Text;
                    project.Notes[noteIndex].IsNoLongerRelevant = item.IsNoLongerRelevant;
                }
                else  // add note
                {
                    project.Notes.Add(item);
                }

                _context.SaveChanges();

                // *************************************************************
                // return updated time records
                // *************************************************************              
                List<Note> notes = new List<Note>();

                foreach (Note note in project.Notes)
                {
                    if (note.IsNoLongerRelevant == false)
                    {
                        notes.Add(note);
                    }
                }

                return new ObjectResult(new HetsResponse(notes));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update or create an array of notes associated with a project
        /// </summary>
        /// <remarks>Update a Project&#39;s Notes</remarks>
        /// <param name="id">id of Project to update Notes for</param>
        /// <param name="items">Array of Project Notes</param>
        /// <response code="200">OK</response>
        public virtual IActionResult ProjectsIdNotesBulkPostAsync(int id, Note[] items)
        {
            bool exists = _context.Projects.Any(a => a.Id == id);

            if (exists && items != null)
            {
                Project project = _context.Projects
                    .Include(x => x.Notes)
                    .First(x => x.Id == id);

                // process each note
                foreach (Note item in items)
                {                   
                    // ******************************************************************
                    // add or update note
                    // ******************************************************************
                    if (item.Id > 0)
                    {
                        int noteIndex = project.Notes.FindIndex(a => a.Id == item.Id);

                        if (noteIndex < 0)
                        {
                            // record not found
                            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                        }

                        project.Notes[noteIndex].Text = item.Text;
                        project.Notes[noteIndex].IsNoLongerRelevant = item.IsNoLongerRelevant;
                    }
                    else  // add note
                    {
                        project.Notes.Add(item);
                    }

                    _context.SaveChanges();
                }

                _context.SaveChanges();

                // *************************************************************
                // return updated notes                
                // *************************************************************
                List<Note> notes = new List<Note>();

                foreach (Note note in project.Notes)
                {
                    if (note.IsNoLongerRelevant == false)
                    {
                        notes.Add(note);
                    }
                }

                return new ObjectResult(new HetsResponse(notes));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        #endregion
    }
}
