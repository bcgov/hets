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
        /// Get all projects
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult ProjectsGetAsync()
        {
            List<Project> result = _context.Projects
                .Include(x => x.Attachments)
                .Include(x => x.Contacts)
                .Include(x => x.History)
                .Include(x => x.District.Region)
                .Include(x => x.Notes)
                .Include(x => x.PrimaryContact)
                .Include(x => x.RentalRequests)
                .Include(x => x.RentalAgreements)
                .ToList();

            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Delete project
        /// </summary>
        /// <param name="id">id of Project to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult ProjectsIdDeletePostAsync(int id)
        {
            bool exists = _context.Projects.Any(a => a.Id == id);

            if (exists)
            {
                Project item = _context.Projects.First(a => a.Id == id);

                if (item != null)
                {
                    _context.Projects.Remove(item);

                    // save the changes
                    _context.SaveChanges();
                }

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
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
                Project result = _context.Projects
                    .Include(x => x.Attachments)
                    .Include(x => x.Contacts)
                    .Include(x => x.History)
                    .Include(x => x.District.Region)
                    .Include(x => x.Notes)
                    .Include(x => x.PrimaryContact)
                    .Include(x => x.RentalRequests)
                        .ThenInclude(e => e.DistrictEquipmentType)
                        .ThenInclude(d => d.EquipmentType)
                    .Include(x => x.RentalAgreements)
                        .ThenInclude(e => e.Equipment)
                        .ThenInclude(d => d.DistrictEquipmentType)
                    .First(a => a.Id == id);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
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

                return new ObjectResult(new HetsResponse(item));
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
        /// <response code="200">OK</response>
        public virtual IActionResult ProjectsSearchGetAsync(string districts, string project, bool? hasRequests, bool? hasHires, string status)
        {
            int?[] districtTokens = ParseIntArray(districts);

            // default search results must be limited to user
            int? districtId = _context.GetDistrictIdByUserId(GetCurrentUserId()).Single();

            IQueryable<Project> data = _context.Projects
                .Where(x => x.DistrictId.Equals(districtId))
                .Include(x => x.District.Region)
                .Include(x => x.PrimaryContact)
                .Include(x => x.RentalAgreements)
                .Include(x => x.RentalRequests)
                .Select(x => x);

            if (districtTokens != null && districts.Length > 0)
            {
                data = data.Where(x => districtTokens.Contains(x.District.Id));
            }
            
            if (project != null)
            {
                // allow for case insensitive search of project name
                data = data.Where(x => x.Name.ToLowerInvariant().Contains(project.ToLowerInvariant()));
            }

            List<ProjectSearchResultViewModel> result = new List<ProjectSearchResultViewModel>();

            foreach (Project item in data)
            {
                item.ToViewModel();
                result.Add(item.ToViewModel());
            }

            // second pass to do calculated fields.
            foreach (ProjectSearchResultViewModel projectSearchResultViewModel in result)
            {
                // calculated fields.
                projectSearchResultViewModel.Requests = _context.RentalRequests
                    .Include(x => x.Project)
                    .Count(x => x.Project.Id == projectSearchResultViewModel.Id);

                projectSearchResultViewModel.Hires = _context.RentalAgreements
                    .Include(x => x.Project)
                    .Count(x => x.Project.Id == projectSearchResultViewModel.Id);
            }

            return new ObjectResult(new HetsResponse(result));
        }

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
                Project project = _context.Projects
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
                        .ThenInclude(t => t.TimeRecords)
                    .First(x => x.Id == id);

                // must have the rental agreement id
                if (item.RentalAgreement.Id == 0)
                {
                    // (RENTAL AGREEMENT) record not found
                    return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                }

                int rentalAgreementId = item.RentalAgreement.Id;                

                // add or update time record
                if (item.Id > 0)
                {
                    project.RentalAgreements[rentalAgreementId].TimeRecords.Add(item);
                }
                else  // update time record
                {
                    project.RentalAgreements[rentalAgreementId].TimeRecords[item.Id] = item;
                }
                
                _context.SaveChanges();

                // *************************************************************
                // return updated time records
                // *************************************************************
                project = _context.Projects
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
                Project project = _context.Projects
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
                Project project = _context.Projects
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
        /// <response code="200">OK</response>
        public virtual IActionResult ProjectsIdContactsPostAsync(int id, Contact item)
        {
            bool exists = _context.Projects.Any(a => a.Id == id);

            if (exists && item != null)
            {
                Project project = _context.Projects
                    .Include(x => x.District.Region)
                    .Include(x => x.Notes)
                    .Include(x => x.Attachments)
                    .Include(x => x.History)
                    .Include(x => x.Contacts)
                    .First(x => x.Id == id);

                // adjust the incoming list
                item.Id = 0;

                _context.Contacts.Add(item);
                project.Contacts.Add(item);

                _context.Projects.Update(project);
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
                    .Include(x => x.District.Region)
                    .Include(x => x.Notes)
                    .Include(x => x.Attachments)
                    .Include(x => x.History)
                    .Include(x => x.Contacts)
                    .First(x => x.Id == id);

                // adjust the incoming list
                for (int i = 0; i < items.Count(); i++)
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
                Project project = _context.Projects
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
                Project project = _context.Projects
                    .Include(x => x.History)
                    .First(a => a.Id == id);

                List<History> data = project.History.OrderByDescending(y => y.LastUpdateTimestamp).ToList();

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
            result.LastUpdateTimestamp = item.LastUpdateTimestamp;
            result.LastUpdateUserid = item.LastUpdateUserid;
            result.AffectedEntityId = id;

            return new ObjectResult(new HetsResponse(result));
        }

        #endregion
    }
}
