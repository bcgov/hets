/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists.
 *
 * OpenAPI spec version: v1
 *
 *
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Mappings;
using Microsoft.AspNetCore.Http;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    ///
    /// </summary>
    public class ProjectService : ServiceBase, IProjectService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public ProjectService(IHttpContextAccessor httpContextAccessor, DbAppContext context) : base(httpContextAccessor, context)
        {
            _context = context;
        }

        private void AdjustRecord(Project item)
        {
            if (item != null)
            {
                // Adjust district
                if (item.District != null)
                {
                    item.District = _context.Districts.FirstOrDefault(a => a.Id == item.District.Id);
                }

                // Notes is a list
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

                // History is a list
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

                // RentalAgreements is a list
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
        }




        /// <summary>
        ///
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
                // Save the changes
                _context.SaveChanges();
            }

            return new NoContentResult();
        }

        /// <summary>
        ///
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult ProjectsGetAsync()
        {
            var result = _context.Projects
                .Include(x => x.Attachments)
                .Include(x => x.Contacts)
                .Include(x => x.History)
                .Include(x => x.District.Region)
                .Include(x => x.Notes)
                .Include(x => x.PrimaryContact)
                .Include(x => x.RentalRequests)
                .Include(x => x.RentalAgreements)
                .ToList();
            return new ObjectResult(result);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="id">id of Owner to fetch Contacts for</param>
        /// <response code="200">OK</response>
        public virtual IActionResult ProjectsIdContactsGetAsync(int id)
        {
            var exists = _context.Projects.Any(a => a.Id == id);
            if (exists)
            {
                Project project = _context.Projects
                    .Include(x => x.District.Region)
                    .Include(x => x.Notes)
                    .Include(x => x.Attachments)
                    .Include(x => x.History)
                    .Include(x => x.Contacts)
                    .First(x => x.Id == id);

                return new ObjectResult(project.Contacts);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }


        /// <summary>
        ///
        /// </summary>
        /// <remarks>Replaces an Owner&#39;s Contacts</remarks>
        /// <param name="id">id of Owner to replace Contacts for</param>
        /// <param name="item">Replacement Owner contacts.</param>
        /// <response code="200">OK</response>
        public virtual IActionResult ProjectsIdContactsPostAsync(int id, Contact item)
        {
            var exists = _context.Projects.Any(a => a.Id == id);
            if (exists && item != null)
            {
                Project project = _context.Projects
                    .Include(x => x.District.Region)
                    .Include(x => x.Notes)
                    .Include(x => x.Attachments)
                    .Include(x => x.History)
                    .Include(x => x.Contacts)
                    .First(x => x.Id == id);

                // adjust the incoming list.
                item.Id = 0;

                _context.Contacts.Add(item);
                project.Contacts.Add(item);
                _context.Projects.Update(project);
                _context.SaveChanges();

                return new ObjectResult(item);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }


        /// <summary>
        ///
        /// </summary>
        /// <remarks>Replaces an Project&#39;s Contacts</remarks>
        /// <param name="id">id of Project to replace Contacts for</param>
        /// <param name="items">Replacement Project contacts.</param>
        /// <response code="200">OK</response>
        public virtual IActionResult ProjectsIdContactsPutAsync(int id, Contact[] items)
        {
            var exists = _context.Projects.Any(a => a.Id == id);
            if (exists && items != null)
            {
                Project project = _context.Projects
                    .Include(x => x.District.Region)
                    .Include(x => x.Notes)
                    .Include(x => x.Attachments)
                    .Include(x => x.History)
                    .Include(x => x.Contacts)
                    .First(x => x.Id == id);

                // adjust the incoming list.

                for (int i = 0; i < items.Count(); i++)
                {
                    Contact item = items[i];
                    if (item != null)
                    {
                        bool contact_exists = _context.Contacts.Any(x => x.Id == item.Id);
                        if (contact_exists)
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

                // remove contacts that are no longer attached.

                foreach (Contact contact in project.Contacts)
                {
                    if (contact != null && !items.Any(x => x.Id == contact.Id))
                    {
                        _context.Remove(contact);
                    }
                }

                // replace Contacts.
                project.Contacts = items.ToList();
                _context.Update(project);
                _context.SaveChanges();

                return new ObjectResult(items);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        ///
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
                var result = MappingExtensions.GetAttachmentListAsViewModel(project.Attachments);
                return new ObjectResult(result);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id">id of Project to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult ProjectsIdDeletePostAsync(int id)
        {
            var exists = _context.Projects.Any(a => a.Id == id);
            if (exists)
            {
                var item = _context.Projects.First(a => a.Id == id);
                if (item != null)
                {
                    _context.Projects.Remove(item);
                    // Save the changes
                    _context.SaveChanges();
                }
                return new ObjectResult(item);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }


        /// <summary>
        ///
        /// </summary>
        /// <remarks>Returns History for a particular Project</remarks>
        /// <param name="id">id of Project to fetch History for</param>
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
                    limit = data.Count() - offset;
                }
                List<HistoryViewModel> result = new List<HistoryViewModel>();

                for (int i = (int)offset; i < data.Count() && i < offset + limit; i++)
                {
                    result.Add(data[i].ToViewModel(id));
                }

                return new ObjectResult(result);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>Add a History record to the Project</remarks>
        /// <param name="id">id of Project to fetch History for</param>
        /// <param name="item"></param>
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

            return new ObjectResult(result);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="id">id of Project to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult ProjectsIdGetAsync(int id)
        {
            var exists = _context.Projects.Any(a => a.Id == id);
            if (exists)
            {
                var result = _context.Projects
                    .Include(x => x.Attachments)
                    .Include(x => x.Contacts)
                    .Include(x => x.History)
                    .Include(x => x.District.Region)
                    .Include(x => x.Notes)
                    .Include(x => x.PrimaryContact)
                    .Include(x => x.RentalRequests)
                    .Include(x => x.RentalAgreements)
                    .First(a => a.Id == id);
                return new ObjectResult(result);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id">id of Project to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult ProjectsIdPutAsync(int id, Project item)
        {
            AdjustRecord(item);
            var exists = _context.Projects.Any(a => a.Id == id);
            if (exists && id == item.Id)
            {
                _context.Projects.Update(item);
                // Save the changes
                _context.SaveChanges();
                return new ObjectResult(item);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Project created</response>
        public virtual IActionResult ProjectsPostAsync(Project item)
        {
            if (item != null)
            {
                AdjustRecord(item);

                var exists = _context.Projects.Any(a => a.Id == item.Id);
                if (exists)
                {
                    _context.Projects.Update(item);
                }
                else
                {
                    // record not found
                    _context.Projects.Add(item);
                }
                // Save the changes
                _context.SaveChanges();
                return new ObjectResult(item);
            }
            else
            {
                return new StatusCodeResult(400);
            }
        }

        /// <summary>
        /// Searches Projects
        /// </summary>
        /// <remarks>Used for the project search page.</remarks>
        /// <param name="districtsCSV">Local areas (comma seperated list of id numbers)</param>
        /// <param name="project">name or partial name for a Project</param>
        /// <param name="hasRequests">if true then only include Projects with active Requests</param>
        /// <param name="hasHires">if true then only include Projects with active Rental Agreements</param>
        /// <response code="200">OK</response>
        public virtual IActionResult ProjectsSearchGetAsync(string districtsCSV, string project, bool? hasRequests, bool? hasHires)
        {
            int?[] districts = ParseIntArray(districtsCSV);

            var data = _context.Projects
                    .Include(x => x.District.Region)
                    .Include(x => x.PrimaryContact)
                    .Select(x => x);

            if (districts != null && districts.Length > 0)
            {
                data = data.Where(x => districts.Contains(x.District.Id));
            }

            if (hasRequests != null)
            {
                // throw new NotImplementedException();
            }

            if (hasHires != null)
            {
                // throw new NotImplementedException();
            }

            if (project != null)
            {
                // Allow for case insensitive search of project name
                data = data.Where(x => x.Name.ToLowerInvariant().Contains(project.ToLowerInvariant()));
            }

            var result = new List<ProjectSearchResultViewModel>();
            foreach (var item in data)
            {
                ProjectSearchResultViewModel newItem = item.ToViewModel();
                result.Add(item.ToViewModel());
            }

            // second pass to do calculated fields.
            foreach (ProjectSearchResultViewModel projectSearchResultViewModel in result)
            {
                // calculated fields.
                projectSearchResultViewModel.Requests = _context.RentalRequests
                    .Include(x => x.Project)
                    .Where(x => x.Project.Id == projectSearchResultViewModel.Id)
                    .Count();

                // TODO filter on status once RentalAgreements has a status field.
                projectSearchResultViewModel.Hires = _context.RentalAgreements
                    .Include(x => x.Project)
                    .Where(x => x.Project.Id == projectSearchResultViewModel.Id)
                    .Count();
            }

            return new ObjectResult(result);
        }
    }
}
