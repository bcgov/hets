using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Mappings;
using Microsoft.Extensions.Logging;

namespace HETSAPI.Services.Impl
{   
    /// <summary>
    /// Rental Agreement Service
    /// </summary>
    public class RentalAgreementService : ServiceBase, IRentalAgreementService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public RentalAgreementService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, DbAppContext context, ILoggerFactory loggerFactory)
            : base(httpContextAccessor, context)
        {
            _context = context;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<RentalAgreementService>();
        }

        private void AdjustRecord(RentalAgreement item)
        {
            if (item != null)
            {
                if (item.Equipment != null)
                {
                    item.Equipment = _context.Equipments
                        .Include (x => x.LocalArea)
                        .FirstOrDefault(a => a.Id == item.Equipment.Id);
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
        /// Create bulk rental agreement records
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

            // save the changes
            _context.SaveChanges();

            return new NoContentResult();
        }

        /// <summary>
        /// Get all rental agreements
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalagreementsGetAsync()
        {
            List<RentalAgreement> result = _context.RentalAgreements.AsNoTracking()
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.Owner)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.EquipmentAttachments)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.LocalArea.ServiceArea.District.Region)
                .Include(x => x.Project)
                    .ThenInclude (p => p.District.Region)
                .Include(x => x.RentalAgreementConditions)
                .Include(x => x.RentalAgreementRates)
                .Include(x => x.TimeRecords)
                .ToList();

            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Delete rental agreement
        /// </summary>
        /// <param name="id">id of Project to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult RentalagreementsIdDeletePostAsync(int id)
        {
            bool exists = _context.RentalAgreements.Any(a => a.Id == id);

            if (exists)
            {
                RentalAgreement item = _context.RentalAgreements.First(a => a.Id == id);

                if (item != null)
                {
                    _context.RentalAgreements.Remove(item);

                    // save the changes
                    _context.SaveChanges();
                }

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get rental agreement by id
        /// </summary>
        /// <param name="id">id of Project to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult RentalagreementsIdGetAsync(int id)
        {
            bool exists = _context.RentalAgreements.Any(a => a.Id == id);

            if (exists)
            {
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
                    .First(a => a.Id == id);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get rental agreement pdf
        /// </summary>
        /// <remarks>Returns a PDF version of the specified rental agreement</remarks>
        /// <param name="id">id of RentalAgreement to obtain the PDF for</param>
        /// <exception cref="Exception"></exception>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalagreementsIdPdfGetAsync(int id)
        {
            _logger.LogInformation("Rental Agreement Pdf [Id: {0}]", id);

            RentalAgreement rentalAgreement = _context.RentalAgreements.AsNoTracking()
                .Include(x => x.Equipment).ThenInclude(y => y.Owner).ThenInclude(z => z.PrimaryContact)
                .Include(x => x.Equipment).ThenInclude(y => y.DistrictEquipmentType)
                .Include(x => x.Equipment).ThenInclude(y => y.EquipmentAttachments)
                .Include(x => x.Equipment).ThenInclude(y => y.LocalArea.ServiceArea.District.Region)
                .Include(x => x.Project).ThenInclude(p => p.District.Region)
                .Include(x => x.RentalAgreementConditions)
                .Include(x => x.RentalAgreementRates)
                .FirstOrDefault(a => a.Id == id);

            if (rentalAgreement != null)
            {
                // construct the view model
                RentalAgreementPdfViewModel rentalAgreementPdfViewModel = rentalAgreement.ToViewModel();                

                string payload = JsonConvert.SerializeObject(rentalAgreementPdfViewModel, new JsonSerializerSettings {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.Indented,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                });

                _logger.LogInformation("Rental Agreement Pdf [Id: {0}] - Payload Length: {1}", id, payload.Length);

                // pass the request on to the Pdf Micro Service
                string pdfHost = _configuration["PDF_SERVICE_NAME"];
                string pdfUrl = _configuration.GetSection("Constants:RentalAgreementPdfUrl").Value;
                string targetUrl = pdfHost + pdfUrl;                

                // generate pdf document name [unique portion only]
                string ownerName = rentalAgreement.Equipment.Owner.OrganizationName.Trim().ToLower();
                ownerName = CleanFileName(ownerName);
                ownerName = ownerName.Replace(" ", "");
                ownerName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(ownerName);
                string fileName = rentalAgreement.Number + "_" + ownerName;

                targetUrl = targetUrl + "/" + fileName;

                _logger.LogInformation("Rental Agreement Pdf [Id: {0}] - HETS Pdf Service Url: {1}", id, targetUrl);

                // call the microservice
                try
                {
                    HttpClient client = new HttpClient();
                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                    _logger.LogInformation("Rental Agreement Pdf [Id: {0}] - Calling HETS Pdf Service", id);
                    HttpResponseMessage response = client.PostAsync(targetUrl, stringContent).Result;
                    
                    // success
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        _logger.LogInformation("Rental Agreement Pdf [Id: {0}] - HETS Pdf Service Response: OK", id);

                        var pdfResponseBytes = GetPdf(response);

                        // convert to string and log
                        string pdfResponse = Encoding.Default.GetString(pdfResponseBytes);

                        _logger.LogInformation("Rental Agreement Pdf [Id: {0}] - HETS Pdf Filename: {1}", id, fileName);
                        _logger.LogInformation("Rental Agreement Pdf [Id: {0}] - HETS Pdf Size: {1}", id, pdfResponse.Length);

                        // return content
                        FileContentResult result = new FileContentResult(pdfResponseBytes, "application/pdf")
                        {
                            FileDownloadName = fileName
                        };

                        return result;
                    }

                    _logger.LogInformation("Rental Agreement Pdf [Id: {0}] - HETS Pdf Service Response: {1}", id, response.StatusCode);
                }
                catch (Exception ex)
                {
                    Debug.Write("Error generating pdf: " + ex.Message);
                    return new ObjectResult(new HetsResponse("HETS-05", ErrorViewModel.GetDescription("HETS-05", _configuration)));
                }                

                // problem occured
                return new ObjectResult(new HetsResponse("HETS-05", ErrorViewModel.GetDescription("HETS-05", _configuration)));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        private static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        private static byte[] GetPdf(HttpResponseMessage response)
        {
            try
            {
                var pdfResponseBytes = response.Content.ReadAsByteArrayAsync();
                pdfResponseBytes.Wait();

                return pdfResponseBytes.Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }            
        }

        /// <summary>
        /// Update rental agreement
        /// </summary>
        /// <param name="id">id of Rental Agreement to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult RentalagreementsIdPutAsync(int id, RentalAgreement item)
        {
            AdjustRecord(item);

            bool exists = _context.RentalAgreements.Any(a => a.Id == id);

            if (exists && id == item.Id)
            {
                _context.RentalAgreements.Update(item);

                // save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get rental agreement
        /// </summary>
        /// <param name="item"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetRentalAgreementNumber (RentalAgreement item, DbAppContext context)
        {
            string result = "";

            // validate item.
            if (item.Equipment != null && item.Equipment.LocalArea != null)
            {
                DateTime currentTime = DateTime.UtcNow;

                int fiscalYear = currentTime.Year;

                // fiscal year always ends in March.
                if (currentTime.Month > 3)
                {
                    fiscalYear++;
                }

                int localAreaNumber = item.Equipment.LocalArea.LocalAreaNumber;
                int localAreaId = item.Equipment.LocalArea.Id;

                DateTime fiscalYearStart = new DateTime(fiscalYear - 1, 1, 1);

                // count the number of rental agreements in the system.
                int currentCount = context.RentalAgreements
                    .Include(x => x.Equipment.LocalArea)
                    .Count(x => x.Equipment.LocalArea.Id == localAreaId && x.AppCreateTimestamp >= fiscalYearStart);

                currentCount++;

                // format of the Rental Agreement number is YYYY-#-####
                result = fiscalYear + "-" + localAreaNumber + "-" + currentCount.ToString ("D4");
            }

            return result;
        }

        /// <summary>
        /// Create rental agreement
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Project created</response>
        public virtual IActionResult RentalagreementsPostAsync(RentalAgreement item)
        {
            if (item != null)
            {
                AdjustRecord(item);

                bool exists = _context.RentalAgreements.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.RentalAgreements.Update(item);
                }
                else
                {
                    item.Number = GetRentalAgreementNumber(item, _context);

                    // record not found
                    _context.RentalAgreements.Add(item);
                }

                // save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // no record to insert
            return new ObjectResult(new HetsResponse("HETS-04", ErrorViewModel.GetDescription("HETS-04", _configuration)));
        }

        /// <summary>
        /// Release (terminate) a rental agreement
        /// </summary>
        /// /// <param name="id">Id of Rental Agreement to release</param>
        /// <response code="201">Rental Agreement released</response>
        public virtual IActionResult RentalagreementsIdReleasePostAsync(int id)
        {
            bool exists = _context.RentalAgreements.Any(a => a.Id == id);

            if (exists)
            {
                RentalAgreement rentalAgreement = _context.RentalAgreements.FirstOrDefault(a => a.Id == id);

                if (rentalAgreement == null)
                {
                    // record not found
                    return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                }

                // release (terminate) rental agreement
                rentalAgreement.Status = "Complete";

                _context.RentalAgreements.Update(rentalAgreement);

                // save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(rentalAgreement));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        #region Rental Agreement Time Records

        /// <summary>
        /// Get time records associated with rental agreement
        /// </summary>
        /// <param name="id">id of Rental Agreement to fetch Time Records for</param>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalAgreementsIdTimeRecordsGetAsync(int id)
        {
            bool exists = _context.RentalAgreements.Any(a => a.Id == id);

            if (exists)
            {
                RentalAgreement agreement = _context.RentalAgreements.AsNoTracking()
                    .Include(x => x.TimeRecords)
                    .First(x => x.Id == id);

                List<TimeRecord> timeRecords = new List<TimeRecord>();
                timeRecords.AddRange(agreement.TimeRecords);

                return new ObjectResult(new HetsResponse(timeRecords));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update or create a time record associated with a rental agreement
        /// </summary>
        /// <remarks>Update a Project&#39;s Time Record</remarks>
        /// <param name="id">id of Rental Agreement to update Time Records for</param>
        /// <param name="item">Rental Agreement Time Record</param>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalAgreementsIdTimeRecordsPostAsync(int id, TimeRecord item)
        {
            bool exists = _context.RentalAgreements.Any(a => a.Id == id);

            if (exists && item != null)
            {
                RentalAgreement agreement = _context.RentalAgreements
                    .Include(x => x.TimeRecords)
                    .First(x => x.Id == id);

                // ******************************************************************
                // add or update time record
                // ******************************************************************                
                if (item.Id > 0)
                {
                    int timeIndex = agreement.TimeRecords.FindIndex(a => a.Id == item.Id);

                    if (timeIndex < 0)
                    {
                        // record not found
                        return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                    }

                    agreement.TimeRecords[timeIndex].EnteredDate = item.EnteredDate;
                    agreement.TimeRecords[timeIndex].Hours = item.Hours;
                    agreement.TimeRecords[timeIndex].TimePeriod = item.TimePeriod;
                    agreement.TimeRecords[timeIndex].WorkedDate = item.WorkedDate;
                }
                else // add time record
                {
                    agreement.TimeRecords.Add(item);
                }

                _context.SaveChanges();

                // *************************************************************
                // return updated time records
                // *************************************************************
                List<TimeRecord> timeRecords = new List<TimeRecord>();

                timeRecords.AddRange(agreement.TimeRecords);

                return new ObjectResult(new HetsResponse(timeRecords));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update or create an array of time records associated with a rental agreement
        /// </summary>
        /// <remarks>Update a Renta Agreement&#39;s Time Records</remarks>
        /// <param name="id">id of Rental Agreement to update Time Records for</param>
        /// <param name="items">Array of Rental Agreement Time Records</param>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalAgreementsIdTimeRecordsBulkPostAsync(int id, TimeRecord[] items)
        {
            bool exists = _context.RentalAgreements.Any(a => a.Id == id);

            if (exists && items != null)
            {
                RentalAgreement agreement = _context.RentalAgreements
                    .Include(x => x.TimeRecords)
                    .First(x => x.Id == id);

                // process each time record
                foreach (TimeRecord item in items)
                {
                    // ******************************************************************
                    // add or update time record
                    // ******************************************************************                
                    if (item.Id > 0)
                    {
                        int timeIndex = agreement.TimeRecords.FindIndex(a => a.Id == item.Id);

                        if (timeIndex < 0)
                        {
                            // record not found
                            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                        }

                        agreement.TimeRecords[timeIndex].EnteredDate = item.EnteredDate;
                        agreement.TimeRecords[timeIndex].Hours = item.Hours;
                        agreement.TimeRecords[timeIndex].TimePeriod = item.TimePeriod;
                        agreement.TimeRecords[timeIndex].WorkedDate = item.WorkedDate;
                    }
                    else // add time record
                    {
                        agreement.TimeRecords.Add(item);
                    }

                    _context.SaveChanges();
                }

                // *************************************************************
                // return updated time records
                // *************************************************************
                agreement = _context.RentalAgreements
                    .Include(x => x.TimeRecords)
                    .First(x => x.Id == id);

                List<TimeRecord> timeRecords = new List<TimeRecord>();

                timeRecords.AddRange(agreement.TimeRecords);

                return new ObjectResult(new HetsResponse(timeRecords));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        #endregion

        #region Rental Agreement Rate Records

        /// <summary>
        /// Get rental agreement rate records associated with rental agreement
        /// </summary>
        /// <param name="id">id of Rental Agreement to fetch rate records for</param>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalAgreementsIdRentalAgreementRatesGetAsync(int id)
        {
            bool exists = _context.RentalAgreements.Any(a => a.Id == id);

            if (exists)
            {
                RentalAgreement agreement = _context.RentalAgreements.AsNoTracking()
                    .Include(x => x.RentalAgreementRates)
                    .First(x => x.Id == id);

                List<RentalAgreementRate> rates = new List<RentalAgreementRate>();
                rates.AddRange(agreement.RentalAgreementRates);

                return new ObjectResult(new HetsResponse(rates));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update or create a rate records associated with a rental agreement
        /// </summary>
        /// <remarks>Update a Rental Agreement&#39;s Rate Record</remarks>
        /// <param name="id">id of Rental Agreement to update Rate Records for</param>
        /// <param name="item">Rental Agreement Rate Record</param>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalAgreementsIdRentalAgreementRatesPostAsync(int id, RentalAgreementRate item)
        {
            bool exists = _context.RentalAgreements.Any(a => a.Id == id);

            if (exists && item != null)
            {
                RentalAgreement agreement = _context.RentalAgreements
                    .Include(x => x.RentalAgreementRates)
                    .First(x => x.Id == id);

                // ******************************************************************
                // add or update rate records
                // ******************************************************************                
                if (item.Id > 0)
                {
                    int rateIndex = agreement.RentalAgreementRates.FindIndex(a => a.Id == item.Id);

                    if (rateIndex < 0)
                    {
                        // record not found
                        return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                    }

                    agreement.RentalAgreementRates[rateIndex].Comment = item.Comment;
                    agreement.RentalAgreementRates[rateIndex].ComponentName = item.ComponentName;
                    agreement.RentalAgreementRates[rateIndex].IsAttachment = item.IsAttachment;
                    agreement.RentalAgreementRates[rateIndex].IsIncludedInTotal = item.IsIncludedInTotal;
                    agreement.RentalAgreementRates[rateIndex].PercentOfEquipmentRate = item.PercentOfEquipmentRate;
                    agreement.RentalAgreementRates[rateIndex].Rate = item.Rate;
                    agreement.RentalAgreementRates[rateIndex].RatePeriod = item.RatePeriod;
                }
                else // add rate records
                {
                    agreement.RentalAgreementRates.Add(item);
                }

                _context.SaveChanges();

                // *************************************************************
                // return updated rental agreement rate records
                // *************************************************************
                List<RentalAgreementRate> rateRecords = new List<RentalAgreementRate>();

                rateRecords.AddRange(agreement.RentalAgreementRates);

                return new ObjectResult(new HetsResponse(rateRecords));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update or create an array of rental agreement rate records associated with a rental agreement
        /// </summary>
        /// <remarks>Update a Rental Agreement&#39;s Rate Records</remarks>
        /// <param name="id">id of Rental Agreement to update Rate Records for</param>
        /// <param name="items">Array of Rental Agreement Rate Records</param>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalAgreementsIdRentalAgreementRatesBulkPostAsync(int id, RentalAgreementRate[] items)
        {
            bool exists = _context.RentalAgreements.Any(a => a.Id == id);

            if (exists && items != null)
            {
                RentalAgreement agreement = _context.RentalAgreements
                    .Include(x => x.RentalAgreementRates)
                    .First(x => x.Id == id);

                // process each rate records
                foreach (RentalAgreementRate item in items)
                {
                    // ******************************************************************
                    // add or update rate records
                    // ******************************************************************                
                    if (item.Id > 0)
                    {
                        int rateIndex = agreement.RentalAgreementRates.FindIndex(a => a.Id == item.Id);

                        if (rateIndex < 0)
                        {
                            // record not found
                            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                        }

                        agreement.RentalAgreementRates[rateIndex].Comment = item.Comment;
                        agreement.RentalAgreementRates[rateIndex].ComponentName = item.ComponentName;
                        agreement.RentalAgreementRates[rateIndex].IsAttachment = item.IsAttachment;
                        agreement.RentalAgreementRates[rateIndex].IsIncludedInTotal = item.IsIncludedInTotal;
                        agreement.RentalAgreementRates[rateIndex].PercentOfEquipmentRate = item.PercentOfEquipmentRate;
                        agreement.RentalAgreementRates[rateIndex].Rate = item.Rate;
                        agreement.RentalAgreementRates[rateIndex].RatePeriod = item.RatePeriod;
                    }
                    else // add rate records
                    {
                        agreement.RentalAgreementRates.Add(item);
                    }

                    _context.SaveChanges();
                }

                // *************************************************************
                // return updated rental agreement rate records
                // *************************************************************
                agreement = _context.RentalAgreements
                    .Include(x => x.RentalAgreementRates)
                    .First(x => x.Id == id);

                List<RentalAgreementRate> rateRecords = new List<RentalAgreementRate>();

                rateRecords.AddRange(agreement.RentalAgreementRates);

                return new ObjectResult(new HetsResponse(rateRecords));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        #endregion

        #region Rental Agreement Condition Records

        /// <summary>
        /// Get rental agreement condition records associated with rental agreement
        /// </summary>
        /// <param name="id">id of Rental Agreement to fetch condition records for</param>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalAgreementsIdRentalAgreementConditionsGetAsync(int id)
        {
            bool exists = _context.RentalAgreements.Any(a => a.Id == id);

            if (exists)
            {
                RentalAgreement agreement = _context.RentalAgreements.AsNoTracking()
                    .Include(x => x.RentalAgreementConditions)
                    .First(x => x.Id == id);

                List<RentalAgreementCondition> conditions = new List<RentalAgreementCondition>();
                conditions.AddRange(agreement.RentalAgreementConditions);

                return new ObjectResult(new HetsResponse(conditions));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update or create a condition records associated with a rental agreement
        /// </summary>
        /// <remarks>Update a Rental Agreement&#39;s Condition Record</remarks>
        /// <param name="id">id of Rental Agreement to update Condition Records for</param>
        /// <param name="item">Rental Agreement Condition Record</param>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalAgreementsIdRentalAgreementConditionsPostAsync(int id, RentalAgreementCondition item)
        {
            bool exists = _context.RentalAgreements.Any(a => a.Id == id);

            if (exists && item != null)
            {
                RentalAgreement agreement = _context.RentalAgreements
                    .Include(x => x.RentalAgreementConditions)
                    .First(x => x.Id == id);

                // ******************************************************************
                // add or update condition records
                // ******************************************************************                
                if (item.Id > 0)
                {
                    int condIndex = agreement.RentalAgreementConditions.FindIndex(a => a.Id == item.Id);

                    if (condIndex < 0)
                    {
                        // record not found
                        return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                    }

                    agreement.RentalAgreementConditions[condIndex].Comment = item.Comment;
                    agreement.RentalAgreementConditions[condIndex].ConditionName = item.ConditionName;
                }
                else // add condition records
                {
                    agreement.RentalAgreementConditions.Add(item);
                }

                _context.SaveChanges();

                // *************************************************************
                // return updated rental agreement condition records
                // *************************************************************
                List<RentalAgreementCondition> condRecords = new List<RentalAgreementCondition>();

                condRecords.AddRange(agreement.RentalAgreementConditions);

                return new ObjectResult(new HetsResponse(condRecords));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update or create an array of rental agreement condition records associated with a rental agreement
        /// </summary>
        /// <remarks>Update a Rental Agreement&#39;s Condition Records</remarks>
        /// <param name="id">id of Rental Agreement to update Condition Records for</param>
        /// <param name="items">Array of Rental Agreement Condition Records</param>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalAgreementsIdRentalAgreementConditionsBulkPostAsync(int id, RentalAgreementCondition[] items)
        {
            bool exists = _context.RentalAgreements.Any(a => a.Id == id);

            if (exists && items != null)
            {
                RentalAgreement agreement = _context.RentalAgreements
                    .Include(x => x.RentalAgreementConditions)
                    .First(x => x.Id == id);

                // process each condition records
                foreach (RentalAgreementCondition item in items)
                {
                    // ******************************************************************
                    // add or update rate records
                    // ******************************************************************                
                    if (item.Id > 0)
                    {
                        int condIndex = agreement.RentalAgreementConditions.FindIndex(a => a.Id == item.Id);

                        if (condIndex < 0)
                        {
                            // record not found
                            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                        }

                        agreement.RentalAgreementConditions[condIndex].Comment = item.Comment;
                        agreement.RentalAgreementConditions[condIndex].ConditionName = item.ConditionName;
                    }
                    else // add condition records
                    {
                        agreement.RentalAgreementConditions.Add(item);
                    }

                    _context.SaveChanges();
                }

                // *************************************************************
                // return updated rental agreement condition records
                // *************************************************************
                agreement = _context.RentalAgreements
                    .Include(x => x.RentalAgreementConditions)
                    .First(x => x.Id == id);

                List<RentalAgreementCondition> condRecords = new List<RentalAgreementCondition>();

                condRecords.AddRange(agreement.RentalAgreementConditions);

                return new ObjectResult(new HetsResponse(condRecords));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        #endregion


    }
}
