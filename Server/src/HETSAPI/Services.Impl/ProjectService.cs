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

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectService : IProjectService
    {
        private readonly DbAppContext _context;
        

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public ProjectService(DbAppContext context)
        {
            _context = context;
        }

        private void AdjustRecord(Project item)
        {
            if (item != null)
            {
                if (item.District != null)
                {
                    // Adjust the record to allow it to be updated / inserted
                    // avoid inserting a District if possible.
                    int district_id = item.District.Id;
                    var exists = _context.Districts.Any(a => a.Id == district_id);
                    if (exists)
                    {
                        District district = _context.Districts.First(a => a.Id == district_id);
                        item.District = district;
                    }
                    else
                    {
                        item.District = null;
                    }
                }
                

                // Notes is a list     
                if (item.Notes != null)
                {
                    for (int i = 0; i < item.Notes.Count; i++)
                    {
                        Note note = item.Notes[i];
                        if (note != null)
                        {
                            int note_id = note.Id;
                            bool note_exists = _context.Notes.Any(a => a.Id == note_id);
                            if (note_exists)
                            {
                                note = _context.Notes.First(a => a.Id == note_id);
                                item.Notes[i] = note;
                            }
                            else
                            {
                                item.Notes[i] = null;
                            }
                        }
                    }
                }

                // History is a list     
                if (item.History != null)
                {
                    for (int i = 0; i < item.History.Count; i++)
                    {
                        History history = item.History[i];
                        if (history != null)
                        {
                            int history_id = history.Id;
                            bool history_exists = _context.Historys.Any(a => a.Id == history_id);
                            if (history_exists)
                            {
                                history = _context.Historys.First(a => a.Id == history_id);
                                item.History[i] = history;
                            }
                            else
                            {
                                item.History[i] = null;
                            }
                        }
                    }
                }

                // Adjust the record to allow it to be updated / inserted
                if (item.PrimaryContact != null)
                {
                    int primaryContact_id = item.PrimaryContact.Id;
                    bool primaryContact_exists = _context.Contacts.Any(a => a.Id == primaryContact_id);
                    if (primaryContact_exists)
                    {
                        Contact contact = _context.Contacts.First(a => a.Id == primaryContact_id);
                        item.PrimaryContact = contact;
                    }
                    else
                    {
                        item.PrimaryContact = null;
                    }
                }

                // Contacts is a list     
                if (item.Contacts != null)
                {
                    for (int i = 0; i < item.Contacts.Count; i++)
                    {
                        Contact contact = item.Contacts[i];
                        if (contact != null)
                        {
                            int contact_id = contact.Id;
                            bool history_exists = _context.Contacts.Any(a => a.Id == contact_id);
                            if (history_exists)
                            {
                                contact = _context.Contacts.First(a => a.Id == contact_id);
                                item.Contacts[i] = contact;
                            }
                            else
                            {
                                item.Contacts[i] = null;
                            }
                        }
                    }
                }

                // RentalRequests is a list     
                if (item.RentalRequests != null)
                {
                    for (int i = 0; i < item.RentalRequests.Count; i++)
                    {
                        RentalRequest rentalRequest = item.RentalRequests[i];
                        if (rentalRequest != null)
                        {
                            int contact_id = rentalRequest.Id;
                            bool history_exists = _context.RentalRequests.Any(a => a.Id == contact_id);
                            if (history_exists)
                            {
                                rentalRequest = _context.RentalRequests.First(a => a.Id == contact_id);
                                item.RentalRequests[i] = rentalRequest;
                            }
                            else
                            {
                                item.RentalRequests[i] = null;
                            }
                        }
                    }
                }

                // RentalAgreements is a list     
                if (item.RentalAgreements != null)
                {
                    for (int i = 0; i < item.RentalAgreements.Count; i++)
                    {
                        RentalAgreement rentalAgreement = item.RentalAgreements[i];
                        if (rentalAgreement != null)
                        {
                            int contact_id = rentalAgreement.Id;
                            bool history_exists = _context.RentalRequests.Any(a => a.Id == contact_id);
                            if (history_exists)
                            {
                                rentalAgreement = _context.RentalAgreements.First(a => a.Id == contact_id);
                                item.RentalAgreements[i] = rentalAgreement;
                            }
                            else
                            {
                                item.RentalAgreements[i] = null;
                            }
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
        /// <param name="localareas">Local areas (array of id numbers)</param>
        /// <param name="project">name or partial name for a Project</param>
        /// <param name="hasRequests">if true then only include Projects with active Requests</param>
        /// <param name="hasHires">if true then only include Projects with active Rental Agreements</param>
        /// <response code="200">OK</response>
        public virtual IActionResult ProjectsSearchGetAsync(int?[] districts, string project, bool? hasRequests, bool? hasHires)
        {
            var data = _context.Projects
                    .Include(x => x.District.Region)
                    .Include(x => x.PrimaryContact)
                    .Select(x => x);

            if (districts != null)
            {
                foreach (int? localarea in districts)
                {
                    if (localarea != null)
                    {
                        data = data.Where(x => x.District.Id == localarea);
                    }
                }
            }

            if (hasRequests != null)
            {
                
            }

            if (hasHires != null)
            {
                // hired is not currently implemented.                 
            }

            if (project != null)
            {
                data = data.Where(x => x.Name.Contains (project));
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
