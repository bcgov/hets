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
using HETSAPI.Services;
using HETSAPI.Services.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;

namespace HETSAPI.Services.Impl
{
    public class RentalAgreementService : ServiceBase, IRentalAgreementService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration Configuration;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public RentalAgreementService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, DbAppContext context) : base(httpContextAccessor, context)
        {
            _context = context;
            Configuration = configuration;
        }

        private void AdjustRecord(RentalAgreement item)
        {
            if (item != null)
            {
                if (item.Equipment != null)
                {
                    item.Equipment = _context.Equipments.FirstOrDefault(a => a.Id == item.Equipment.Id);
                }

                if (item.Project != null)
                {
                    item.Project = _context.Projects.FirstOrDefault(a => a.Id == item.Project.Id);
                }
                

                if (item.RentalAgreementConditions != null)
                {
                    for (int i = 0; i < item.RentalAgreementConditions.Count; i++)
                    {                        
                        if (item.RentalAgreementConditions[i] != null)
                        {
                            item.RentalAgreementConditions[i] = _context.RentalAgreementConditions.FirstOrDefault(a => a.Id == item.RentalAgreementConditions[i].Id);
                        }
                    }
                }

                if (item.RentalAgreementRates != null)
                {
                    for (int i = 0; i < item.RentalAgreementRates.Count; i++)
                    {
                        if (item.RentalAgreementRates[i] != null)
                        {
                            item.RentalAgreementRates[i] = _context.RentalAgreementRates.FirstOrDefault(a => a.Id == item.RentalAgreementRates[i].Id);
                        }
                    }
                }

                if (item.TimeRecords != null)
                {
                    for (int i = 0; i < item.TimeRecords.Count; i++)
                    {
                        if (item.TimeRecords[i] != null)
                        {
                            item.TimeRecords[i] = _context.TimeRecords.First(a => a.Id == item.TimeRecords[i].Id);
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
        public virtual IActionResult RentalagreementsBulkPostAsync(RentalAgreement[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }
            foreach (RentalAgreement item in items)
            {
                AdjustRecord(item);
                bool exists = _context.RentalAgreements.Any(a => a.Id == item.Id);
                if (exists)
                {
                    _context.RentalAgreements.Update(item);
                }
                else
                {
                    _context.RentalAgreements.Add(item);
                }
            }
            // Save the changes
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalagreementsGetAsync()
        {
            var result = _context.RentalAgreements                                
                .Include(x => x.Equipment).ThenInclude(y => y.Owner)
                .Include(x => x.Equipment).ThenInclude(y => y.EquipmentAttachments)
                .Include(x => x.Equipment).ThenInclude(y => y.LocalArea.ServiceArea.District.Region)
                .Include(x => x.Project).ThenInclude (p => p.District.Region)
                .Include(x => x.RentalAgreementConditions)
                .Include(x => x.RentalAgreementRates)
                .Include(x => x.TimeRecords)
                .ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Project to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult RentalagreementsIdDeletePostAsync(int id)
        {
            var exists = _context.RentalAgreements.Any(a => a.Id == id);
            if (exists)
            {
                var item = _context.RentalAgreements.First(a => a.Id == id);
                if (item != null)
                {
                    _context.RentalAgreements.Remove(item);
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
        public virtual IActionResult RentalagreementsIdGetAsync(int id)
        {
            var exists = _context.RentalAgreements.Any(a => a.Id == id);
            if (exists)
            {
                var result = _context.RentalAgreements
                    .Include(x => x.Equipment).ThenInclude(y => y.Owner)
                    .Include(x => x.Equipment).ThenInclude(y => y.EquipmentAttachments)
                    .Include(x => x.Equipment).ThenInclude(y => y.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.Project).ThenInclude(p => p.District.Region)
                    .Include(x => x.RentalAgreementConditions)
                    .Include(x => x.RentalAgreementRates)
                    .Include(x => x.TimeRecords)
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
        /// <remarks>Returns a PDF version of the specified rental agreement</remarks>
        /// <param name="id">id of RentalAgreement to obtain the PDF for</param>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalagreementsIdPdfGetAsync(int id)
        {
            FileContentResult result = null;
            RentalAgreement rentalAgreement = _context.RentalAgreements.FirstOrDefault(a => a.Id == id);
            if (rentalAgreement != null)
            {

                // construct the view model.

                RentalAgreementPdfViewModel rentalAgreementPdfViewModel = new RentalAgreementPdfViewModel();

                rentalAgreementPdfViewModel.Id = rentalAgreement.Id;
                rentalAgreementPdfViewModel.Number = rentalAgreement.Number;
                

                string payload = JsonConvert.SerializeObject(rentalAgreementPdfViewModel);

                // pass the request on to the PDF Micro Service
                string pdfHost = Configuration["PDF_SERVICE_NAME"];

                string targetUrl = pdfHost + "/api/PDF/GetPDF";

                // call the microservice
                try
                {
                    HttpClient client = new HttpClient();

                    var request = new HttpRequestMessage(HttpMethod.Post, targetUrl);
                    request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

                    request.Headers.Clear();
                    // transfer over the request headers.
                    foreach (var item in Request.Headers)
                    {
                        string key = item.Key;
                        string value = item.Value;
                        request.Headers.Add(key, value);
                    }

                    Task<HttpResponseMessage> responseTask = client.SendAsync(request);
                    responseTask.Wait();

                    HttpResponseMessage response = responseTask.Result;
                    if (response.StatusCode == HttpStatusCode.OK) // success
                    {
                        var bytetask = response.Content.ReadAsByteArrayAsync();
                        bytetask.Wait();

                        result = new FileContentResult(bytetask.Result, "application/pdf");
                        result.FileDownloadName = "RentalAgreement-" + rentalAgreement.Number + ".pdf";
                    }
                }
                catch (Exception e)
                {
                    result = null;
                }

                // check that the result has a value
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new StatusCodeResult(400); // problem occured
                }

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
        public virtual IActionResult RentalagreementsIdPutAsync(int id, RentalAgreement item)
        {
            AdjustRecord(item);
            var exists = _context.RentalAgreements.Any(a => a.Id == id);
            if (exists && id == item.Id)
            {
                _context.RentalAgreements.Update(item);
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
        public virtual IActionResult RentalagreementsPostAsync(RentalAgreement item)
        {
            if (item != null)
            {
                AdjustRecord(item);

                var exists = _context.RentalAgreements.Any(a => a.Id == item.Id);
                if (exists)
                {
                    _context.RentalAgreements.Update(item);
                }
                else
                {
                    // record not found
                    _context.RentalAgreements.Add(item);
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

    }
}
