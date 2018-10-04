using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Helpers;
using HetsData.Model;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Rental Agreement Controller
    /// </summary>
    [Route("/api/rentalAgreements")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class RentalAgreementController : Controller
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpContext _httpContext;
        private readonly ILogger _logger;

        public RentalAgreementController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory)
        {
            _context = context;
            _configuration = configuration;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = loggerFactory.CreateLogger<RentalAgreementController>();

            // set context data
            User user = UserAccountHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.UserGuid;
            _context.SmBusinessGuid = user.BusinessGuid;
        }
        
        /// <summary>
        /// Get rental agreement by id
        /// </summary>
        /// <param name="id">id of RentalAgreement to fetch</param>
        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation("RentalAgreementsIdGet")]
        [SwaggerResponse(200, type: typeof(HetRentalAgreement))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalAgreementsIdGet([FromRoute]int id)
        {
            return new ObjectResult(new HetsResponse(RentalAgreementHelper.GetRecord(id, _context)));
        }

        /// <summary>
        /// Update rental agreement
        /// </summary>
        /// <param name="id">id of RentalAgreement to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation("RentalAgreementsIdPut")]
        [SwaggerResponse(200, type: typeof(HetRentalAgreement))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalAgreementsIdPut([FromRoute]int id, [FromBody]HetRentalAgreement item)
        {
            if (item == null || id != item.RentalAgreementId)
            {
                // not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            bool exists = _context.HetRentalAgreement.Any(a => a.RentalAgreementId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            
            // get record
            HetRentalAgreement agreement = _context.HetRentalAgreement.First(a => a.RentalAgreementId == id);

            int? statusId = StatusHelper.GetStatusId(item.Status, "rentalAgreementStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get rate period type
            // set the rate period type id
            int? rateTypeId = StatusHelper.GetRatePeriodId(item.RatePeriod, _context);

            if (rateTypeId == null)
            {
                throw new DataException("Rate Period Id cannot be null");
            }

            agreement.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
            agreement.DatedOn = item.DatedOn;
            agreement.EquipmentRate = item.EquipmentRate;
            agreement.EstimateHours = item.EstimateHours;
            agreement.EstimateStartWork = item.EstimateStartWork;
            agreement.Note = item.Note;
            agreement.Number = item.Number;
            agreement.RateComment = item.RateComment;
            agreement.RatePeriod = item.RatePeriod;
            agreement.RatePeriodTypeId = (int)rateTypeId;
            agreement.RentalAgreementStatusTypeId = (int)statusId;
            agreement.ProjectId = item.ProjectId;
            agreement.EquipmentId = item.EquipmentId;

            // save the changes
            _context.SaveChanges();

            // retrieve updated rental agreement to return to ui
            return new ObjectResult(new HetsResponse(RentalAgreementHelper.GetRecord(id, _context)));
        }

        /// <summary>
        /// Create rental agreement
        /// </summary>
        /// <param name="item"></param>
        [HttpPost]
        [Route("")]
        [SwaggerOperation("RentalAgreementsPost")]
        [SwaggerResponse(200, type: typeof(HetRentalAgreement))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalAgreementsPost([FromBody]HetRentalAgreement item)
        {
            // not found
            if (item == null) return new ObjectResult(new HetsResponse("HETS-04", ErrorViewModel.GetDescription("HETS-04", _configuration)));

            // set the rate period type id
            int? rateTypeId = StatusHelper.GetRatePeriodId(item.RatePeriod, _context);

            if (rateTypeId == null)
            {
                throw new DataException("Rate Period Id cannot be null");
            }

            int? statusId = StatusHelper.GetStatusId(item.Status, "rentalAgreementStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));
        
            HetRentalAgreement agreement = new HetRentalAgreement
            {
                Number = RentalAgreementHelper.GetRentalAgreementNumber(item.Equipment, _context),
                DatedOn = item.DatedOn,
                EquipmentRate = item.EquipmentRate,
                EstimateHours = item.EstimateHours,
                EstimateStartWork = item.EstimateStartWork,
                Note = item.Note,
                RateComment = item.RateComment,
                RatePeriodTypeId = (int)rateTypeId,
                RentalAgreementStatusTypeId = (int)statusId,
                EquipmentId = item.EquipmentId,
                ProjectId = item.ProjectId
            };
                    
            // save the changes
            _context.SaveChanges();

            int id = agreement.RentalAgreementId;

            // retrieve updated rental agreement to return to ui
            return new ObjectResult(new HetsResponse(RentalAgreementHelper.GetRecord(id, _context)));
        }

        /// <summary>
        /// Release (terminate) a rental agreement
        /// </summary>
        /// <param name="id">id of RentalAgreement to release</param>
        [HttpPost]
        [Route("{id}/release")]
        [SwaggerOperation("RentalAgreementsIdReleasePost")]
        [SwaggerResponse(200, type: typeof(HetRentalAgreement))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalAgreementsIdReleasePost([FromRoute]int id)
        {
            bool exists = _context.HetRentalAgreement.Any(a => a.RentalAgreementId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            
            // get record
            HetRentalAgreement agreement = _context.HetRentalAgreement.First(a => a.RentalAgreementId == id);

            // release (terminate) rental agreement
            int? statusIdComplete = StatusHelper.GetStatusId(HetRentalAgreement.StatusComplete, "rentalAgreementStatus", _context);
            if (statusIdComplete == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            agreement.RentalAgreementStatusTypeId = (int)statusIdComplete;
            agreement.Status = "Complete";

            // save the changes
            _context.SaveChanges();

            // retrieve updated rental agreement to return to ui
            return new ObjectResult(new HetsResponse(RentalAgreementHelper.GetRecord(id, _context)));
        }

        #region Agreement Pdf

        /// <summary>
        /// Get a pdf version of a rental agreement
        /// </summary>
        /// <remarks>Returns a PDF version of the specified rental agreement</remarks>
        /// <param name="id">id of RentalAgreement to obtain the PDF for</param>
        [HttpGet]
        [Route("{id}/pdf")]
        [SwaggerOperation("RentalAgreementsIdPdfGet")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalAgreementsIdPdfGet([FromRoute]int id)
        {
            _logger.LogInformation("Rental Agreement Pdf [Id: {0}]", id);

            HetRentalAgreement rentalAgreement = _context.HetRentalAgreement.AsNoTracking()
                .Include(x => x.RatePeriodType)
                .Include(x => x.RentalAgreementStatusType)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.Owner)
                        .ThenInclude(z => z.PrimaryContact)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.DistrictEquipmentType)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.HetEquipmentAttachment)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.LocalArea.ServiceArea.District.Region)
                .Include(x => x.Project)
                    .ThenInclude(p => p.District.Region)
                .Include(x => x.HetRentalAgreementCondition)
                .Include(x => x.HetRentalAgreementRate)
                .FirstOrDefault(a => a.RentalAgreementId == id);

            if (rentalAgreement != null)
            {
                // construct the view model
                RentalAgreementPdfViewModel rentalAgreementPdfViewModel = RentalAgreementHelper.ToPdfModel(rentalAgreement);

                string payload = JsonConvert.SerializeObject(rentalAgreementPdfViewModel, new JsonSerializerSettings
                {
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

                // call the MicroService
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

                        fileName = fileName + ".pdf";

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

        #endregion

        #region Rental Agreement Time Records

        /// <summary>
        /// Get time records associated with a rental agreement
        /// </summary>
        /// <remarks>Gets a Rental Agreements Time Records</remarks>
        /// <param name="id">id of Rental Agreement to fetch Time Records for</param>
        [HttpGet]
        [Route("{id}/timeRecords")]
        [SwaggerOperation("RentalAgreementsIdTimeRecordsGet")]
        [SwaggerResponse(200, type: typeof(List<HetTimeRecord>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalAgreementsIdTimeRecordsGet([FromRoute]int id)
        {
            bool exists = _context.HetRentalAgreement.Any(a => a.RentalAgreementId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
           
            // return time records
            return new ObjectResult(new HetsResponse(RentalAgreementHelper.GetTimeRecords(id, _context, _configuration)));
        }

        /// <summary>
        /// Add or update a rental agreement time record
        /// </summary>
        /// <remarks>Adds Rental Agreement Time Records</remarks>
        /// <param name="id">id of Rental Agreement to add a time record for</param>
        /// <param name="item">Adds to Rental Agreement Time Records</param>
        [HttpPost]
        [Route("{id}/timeRecord")]
        [SwaggerOperation("RentalAgreementsIdTimeRecordsPost")]
        [SwaggerResponse(200, type: typeof(HetTimeRecord))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalAgreementsIdTimeRecordsPost([FromRoute]int id, [FromBody]HetTimeRecord item)
        {
            bool exists = _context.HetRentalAgreement.Any(a => a.RentalAgreementId == id);

            // not found
            if (!exists || item == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // set the time period type id
            int? timePeriodTypeId = StatusHelper.GetTimePeriodId(item.TimePeriod, _context);
            if (timePeriodTypeId == null) throw new DataException("Time Period Id cannot be null");

            // add or update time record
            if (item.TimeRecordId > 0)
            {
                // get record
                HetTimeRecord time = _context.HetTimeRecord.First(a => a.TimeRecordId == item.TimeRecordId);

                // not found
                if (time == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

                time.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                time.EnteredDate = DateTime.UtcNow;
                time.Hours = item.Hours;
                time.TimePeriod = item.TimePeriod;
                time.TimePeriodTypeId = (int)timePeriodTypeId;
                time.WorkedDate = item.WorkedDate;
            }
            else // add time record
            {
                HetTimeRecord time = new HetTimeRecord
                {
                    RentalAgreementId = id,
                    EnteredDate = DateTime.UtcNow,
                    Hours = item.Hours,
                    TimePeriod = item.TimePeriod,
                    TimePeriodTypeId = (int)timePeriodTypeId,
                    WorkedDate = item.WorkedDate
                };

                _context.HetTimeRecord.Add(time);
            }

            _context.SaveChanges();

            // retrieve updated time records to return to ui
            return new ObjectResult(new HetsResponse(RentalAgreementHelper.GetTimeRecords(id, _context, _configuration)));
        }

        /// <summary>
        /// Update or create an array of time records associated with a rental agreement
        /// </summary>
        /// <remarks>Adds Rental Agreement Time Records</remarks>
        /// <param name="id">id of Rental Agreement to add a time record for</param>
        /// <param name="items">Array of Rental Agreement Time Records</param>
        [HttpPost]
        [Route("{id}/timeRecords")]
        [SwaggerOperation("RentalAgreementsIdTimeRecordsBulkPostAsync")]
        [SwaggerResponse(200, type: typeof(HetTimeRecord))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalAgreementsIdTimeRecordsBulkPostAsync([FromRoute]int id, [FromBody]HetTimeRecord[] items)
        {
            bool exists = _context.HetRentalAgreement.Any(a => a.RentalAgreementId == id);

            // not found
            if (!exists || items == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                        
            // process each time record
            foreach (HetTimeRecord item in items)
            {
                // set the time period type id
                int? timePeriodTypeId = StatusHelper.GetTimePeriodId(item.TimePeriod, _context);
                if (timePeriodTypeId == null) throw new DataException("Time Period Id cannot be null");

                // add or update time record
                if (item.TimeRecordId > 0)
                {
                    // get record
                    HetTimeRecord time = _context.HetTimeRecord.First(a => a.TimeRecordId == item.TimeRecordId);

                    // not found
                    if (time == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

                    time.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                    time.EnteredDate = DateTime.UtcNow;
                    time.Hours = item.Hours;
                    time.TimePeriod = item.TimePeriod;
                    time.TimePeriodTypeId = (int)timePeriodTypeId;
                    time.WorkedDate = item.WorkedDate;
                }
                else // add time record
                {
                    HetTimeRecord time = new HetTimeRecord
                    {
                        RentalAgreementId = id,
                        EnteredDate = DateTime.UtcNow,
                        Hours = item.Hours,
                        TimePeriod = item.TimePeriod,
                        TimePeriodTypeId = (int)timePeriodTypeId,
                        WorkedDate = item.WorkedDate
                    };

                    _context.HetTimeRecord.Add(time);
                }

                _context.SaveChanges();
            }

            // retrieve updated time records to return to ui
            return new ObjectResult(new HetsResponse(RentalAgreementHelper.GetTimeRecords(id, _context, _configuration)));
        }

        #endregion

        #region Rental Agreement Rate Records

        /// <summary>
        /// Get rate records associated with a rental agreement
        /// </summary>
        /// <remarks>Gets a Rental Agreements Rate Records</remarks>
        /// <param name="id">id of Rental Agreement to fetch Rate Records for</param>
        [HttpGet]
        [Route("{id}/rateRecords")]
        [SwaggerOperation("RentalAgreementsIdRentalAgreementRatesGet")]
        [SwaggerResponse(200, type: typeof(List<HetRentalAgreementRate>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalAgreementsIdRentalAgreementRatesGet([FromRoute]int id)
        {
            bool exists = _context.HetRentalAgreement.Any(a => a.RentalAgreementId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // return rental agreement records
            return new ObjectResult(new HetsResponse(RentalAgreementHelper.GetRentalRates(id, _context, _configuration)));
        }

        /// <summary>
        /// Add or update a rental agreement rate record
        /// </summary>
        /// <remarks>Adds Rental Agreement Rate Records</remarks>
        /// <param name="id">id of Rental Agreement to add a rate record for</param>
        /// <param name="item">Adds to Rental Agreement Rate Records</param>
        [HttpPost]
        [Route("{id}/rateRecord")]
        [SwaggerOperation("RentalAgreementsIdRentalAgreementRatesPost")]
        [SwaggerResponse(200, type: typeof(HetRentalAgreementRate))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalAgreementsIdRentalAgreementRatesPost([FromRoute]int id, [FromBody]HetRentalAgreementRate item)
        {
            bool exists = _context.HetRentalAgreement.Any(a => a.RentalAgreementId == id);

            // not found
            if (!exists || item == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            
            // add or update rate records
            if (item.RentalAgreementRateId > 0)
            {
                // get record
                HetRentalAgreementRate rate = _context.HetRentalAgreementRate.FirstOrDefault(a => a.RentalAgreementRateId == item.RentalAgreementRateId);

                // not found
                if (rate == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

                // set the rate period type id
                int? rateTypeId = StatusHelper.GetRatePeriodId(item.RatePeriod, _context);
                if (rateTypeId == null) throw new DataException("Rate Period Id cannot be null");

                rate.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                rate.Comment = item.Comment;
                rate.ComponentName = item.ComponentName;
                rate.IsAttachment = item.IsAttachment;
                rate.IsIncludedInTotal = item.IsIncludedInTotal;
                rate.PercentOfEquipmentRate = item.PercentOfEquipmentRate;
                rate.Rate = item.Rate;
                rate.RatePeriod = item.RatePeriod;
                rate.RatePeriodTypeId = (int)rateTypeId;
            }
            else // add rate records
            {
                // set the rate period type id
                int? rateTypeId = StatusHelper.GetRatePeriodId(item.RatePeriod, _context);
                if (rateTypeId == null) throw new DataException("Rate Period Id cannot be null");

                int agreementId = item.RentalAgreement.RentalAgreementId;

                HetRentalAgreementRate rate = new HetRentalAgreementRate
                {
                    RentalAgreementId = agreementId,
                    Comment = item.Comment,
                    ComponentName = item.ComponentName,
                    IsAttachment = item.IsAttachment,
                    IsIncludedInTotal = item.IsIncludedInTotal,
                    PercentOfEquipmentRate = item.PercentOfEquipmentRate,
                    Rate = item.Rate,
                    RatePeriod = item.RatePeriod,
                    RatePeriodTypeId = (int)rateTypeId
                };

                _context.HetRentalAgreementRate.Add(rate);
            }

            _context.SaveChanges();

            // retrieve updated rate records to return to ui
            return new ObjectResult(new HetsResponse(RentalAgreementHelper.GetRentalRates(id, _context, _configuration)));
        }

        /// <summary>
        /// Update or create an array of rate records associated with a rental agreement
        /// </summary>
        /// <remarks>Adds Rental Agreement Rate Records</remarks>
        /// <param name="id">id of Rental Agreement to add rate records for</param>
        /// <param name="items">Array of Rental Agreement Rate Records</param>
        [HttpPost]
        [Route("{id}/rateRecords")]
        [SwaggerOperation("RentalAgreementsIdRentalAgreementRatesBulkPost")]
        [SwaggerResponse(200, type: typeof(HetRentalAgreementRate))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalAgreementsIdRentalAgreementRatesBulkPost([FromRoute]int id, [FromBody]HetRentalAgreementRate[] items)
        {
            bool exists = _context.HetRentalAgreement.Any(a => a.RentalAgreementId == id);

            // not found
            if (!exists || items == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // process each rate records
            foreach (HetRentalAgreementRate item in items)
            {
                // add or update rate records
                if (item.RentalAgreementRateId > 0)
                {
                    // get record
                    HetRentalAgreementRate rate = _context.HetRentalAgreementRate.FirstOrDefault(a => a.RentalAgreementRateId == item.RentalAgreementRateId);

                    // not found
                    if (rate == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

                    // set the rate period type id
                    int? rateTypeId = StatusHelper.GetRatePeriodId(item.RatePeriod, _context);
                    if (rateTypeId == null) throw new DataException("Rate Period Id cannot be null");                    

                    rate.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                    rate.Comment = item.Comment;
                    rate.ComponentName = item.ComponentName;
                    rate.IsAttachment = item.IsAttachment;
                    rate.IsIncludedInTotal = item.IsIncludedInTotal;
                    rate.PercentOfEquipmentRate = item.PercentOfEquipmentRate;
                    rate.Rate = item.Rate;
                    rate.RatePeriod = item.RatePeriod;                    
                    rate.RatePeriodTypeId = (int)rateTypeId;
                }
                else // add rate records
                {
                    // set the rate period type id
                    // set the rate period type id
                    int? rateTypeId = StatusHelper.GetRatePeriodId(item.RatePeriod, _context);
                    if (rateTypeId == null) throw new DataException("Rate Period Id cannot be null");

                    int agreementId = item.RentalAgreement.RentalAgreementId;

                    HetRentalAgreementRate rate = new HetRentalAgreementRate
                    {
                        RentalAgreementId = agreementId,
                        Comment = item.Comment,
                        ComponentName = item.ComponentName,
                        IsAttachment = item.IsAttachment,
                        IsIncludedInTotal = item.IsIncludedInTotal,
                        PercentOfEquipmentRate = item.PercentOfEquipmentRate,
                        Rate = item.Rate,
                        RatePeriod = item.RatePeriod,
                        RatePeriodTypeId = (int)rateTypeId
                };

                    _context.HetRentalAgreementRate.Add(rate);
                }

                _context.SaveChanges();
            }

            // retrieve updated rate records to return to ui
            return new ObjectResult(new HetsResponse(RentalAgreementHelper.GetRentalRates(id, _context, _configuration)));
        }

        #endregion

        #region Rental Agreement Condition Records

        /// <summary>
        /// Get condition records associated with a rental agreement
        /// </summary>
        /// <remarks>Gets a Rental Agreement&#39;s Condition Records</remarks>
        /// <param name="id">id of Rental Agreement to fetch Condition Records for</param>
        [HttpGet]
        [Route("{id}/conditionRecords")]
        [SwaggerOperation("RentalAgreementsIdRentalAgreementConditionsGet")]
        [SwaggerResponse(200, type: typeof(List<HetRentalAgreementCondition>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalAgreementsIdRentalAgreementConditionsGet([FromRoute]int id)
        {
            bool exists = _context.HetRentalAgreement.Any(a => a.RentalAgreementId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // return rental agreement records
            return new ObjectResult(new HetsResponse(RentalAgreementHelper.GetConditions(id, _context, _configuration)));            
        }

        /// <summary>
        /// Add a rental agreement condition record
        /// </summary>
        /// <remarks>Adds Rental Agreement Condition Records</remarks>
        /// <param name="id">id of Rental Agreement to add a condition record for</param>
        /// <param name="item">Adds to Rental Agreement Condition Records</param>
        [HttpPost]
        [Route("{id}/conditionRecord")]
        [SwaggerOperation("RentalAgreementsIdRentalAgreementConditionsPost")]
        [SwaggerResponse(200, type: typeof(HetRentalAgreementCondition))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalAgreementsIdRentalAgreementConditionsPost([FromRoute]int id, [FromBody]HetRentalAgreementCondition item)
        {
            bool exists = _context.HetRentalAgreement.Any(a => a.RentalAgreementId == id);

            // not found
            if (!exists || item == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                            
            // add or update condition records
            if (item.RentalAgreementConditionId > 0)
            {
                // get record
                HetRentalAgreementCondition condition = _context.HetRentalAgreementCondition
                    .FirstOrDefault(a => a.RentalAgreementConditionId == item.RentalAgreementConditionId);

                // not found
                if (condition == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

                condition.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                condition.Comment = item.Comment;
                condition.ConditionName = item.ConditionName;
            }
            else // add condition records
            {
                int agreementId = item.RentalAgreement.RentalAgreementId;

                HetRentalAgreementCondition condition = new HetRentalAgreementCondition
                {
                    RentalAgreementId = agreementId,
                    Comment = item.Comment,
                    ConditionName = item.ConditionName
                };

                _context.HetRentalAgreementCondition.Add(condition);
            }

            _context.SaveChanges();

            // return rental agreement condition records
            return new ObjectResult(new HetsResponse(RentalAgreementHelper.GetConditions(id, _context, _configuration)));
        }

        /// <summary>
        /// Update or create an array of condition records associated with a rental agreement
        /// </summary>
        /// <remarks>Adds Rental Agreement Condition Records</remarks>
        /// <param name="id">id of Rental Agreement to add condition records for</param>
        /// <param name="items">Array of Rental Agreement Condition Records</param>
        [HttpPost]
        [Route("{id}/conditionRecords")]
        [SwaggerOperation("RentalAgreementsIdRentalAgreementConditionsBulkPost")]
        [SwaggerResponse(200, type: typeof(HetRentalAgreementCondition))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalAgreementsIdRentalAgreementConditionsBulkPost([FromRoute]int id, [FromBody]HetRentalAgreementCondition[] items)
        {
            bool exists = _context.HetRentalAgreement.Any(a => a.RentalAgreementId == id);

            // not found
            if (!exists || items == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // process each rate records
            foreach (HetRentalAgreementCondition item in items)
            {
                // add or update condition records
                if (item.RentalAgreementConditionId > 0)
                {
                    // get record
                    HetRentalAgreementCondition condition = _context.HetRentalAgreementCondition
                        .FirstOrDefault(a => a.RentalAgreementConditionId == item.RentalAgreementConditionId);

                    // not found
                    if (condition == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

                    condition.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                    condition.Comment = item.Comment;
                    condition.ConditionName = item.ConditionName;
                }
                else // add condition records
                {
                    int agreementId = item.RentalAgreement.RentalAgreementId;

                    HetRentalAgreementCondition condition = new HetRentalAgreementCondition
                    {
                        RentalAgreementId = agreementId,
                        Comment = item.Comment,
                        ConditionName = item.ConditionName
                    };

                    _context.HetRentalAgreementCondition.Add(condition);
                }

                _context.SaveChanges();
            }

            // return rental agreement condition records
            return new ObjectResult(new HetsResponse(RentalAgreementHelper.GetConditions(id, _context, _configuration)));
        }

        #endregion

        #region Blank Rental Agreement

        /// <summary>
        /// Create a new blank rental agreement (need a project id)
        /// </summary>
        [HttpPost]
        [Route("createBlankAgreement")]
        [SwaggerOperation("BlankRentalAgreementPost")]
        [SwaggerResponse(200, type: typeof(HetRentalAgreement))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult BlankRentalAgreementPost()
        {
            // get current users district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            HetDistrict district = _context.HetDistrict.AsNoTracking()
                .FirstOrDefault(x => x.DistrictId.Equals(districtId));

            if (district == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // set the rate period type id
            int? rateTypeId = StatusHelper.GetRatePeriodId(HetRatePeriodType.PeriodWeekly, _context);

            if (rateTypeId == null)
            {
                throw new DataException("Rate Period Id cannot be null");
            }

            int? statusId = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));            

            HetRentalAgreement agreement = new HetRentalAgreement
            {
                Number = RentalAgreementHelper.GetRentalAgreementNumber(district, _context),
                DistrictId = districtId,
                RentalAgreementStatusTypeId = (int)statusId,
                RatePeriodTypeId = (int)rateTypeId
            };

            // save the changes
            _context.HetRentalAgreement.Add(agreement);
            _context.SaveChanges();

            int id = agreement.RentalAgreementId;

            // retrieve updated rental agreement to return to ui
            return new ObjectResult(new HetsResponse(RentalAgreementHelper.GetRecord(id, _context)));
        }

        /// <summary>
        /// Get blank rental agreements (for the current district)
        /// </summary>
        [HttpGet]
        [Route("blankAgreements")]
        [SwaggerOperation("BlankRentalAgreementGet")]
        [SwaggerResponse(200, type: typeof(List<HetRentalAgreement>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult BlankRentalAgreementGet()
        {
            // get the current district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            // get all "blank" agreements
            List<HetRentalAgreement> agreements = _context.HetRentalAgreement.AsNoTracking()
                .Include(x => x.RentalAgreementStatusType)
                .Include(x => x.District)
                .Include(x => x.Project)
                .Include(x => x.Equipment)
                .Where(x => x.District.DistrictId == districtId &&
                            x.RentalRequestId == null &&
                            x.RentalRequestRotationListId == null)
                .ToList();

            return new ObjectResult(new HetsResponse(agreements));
        }

        /// <summary>
        /// Get blank rental agreements (for the current district)
        /// By Project Id and District Equipment Type
        /// </summary>
        [HttpGet]
        [Route("blankAgreements/{projectId}/{districtEquipmentTypeId}")]
        [SwaggerOperation("BlankRentalAgreementLookupGet")]
        [SwaggerResponse(200, type: typeof(List<HetRentalAgreement>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult BlankRentalAgreementLookupGet([FromRoute]int projectId, [FromRoute]int districtEquipmentTypeId)
        {
            // get the current district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            // get "blank" agreements
            List<HetRentalAgreement> agreements = _context.HetRentalAgreement.AsNoTracking()
                .Include(x => x.RentalAgreementStatusType)
                .Include(x => x.District)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.Owner)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.DistrictEquipmentType)
                        .ThenInclude(d => d.EquipmentType)
                .Include(x => x.Project)
                .Where(x => x.District.DistrictId == districtId &&
                            x.RentalRequestId == null &&
                            x.RentalRequestRotationListId == null &&
                            x.ProjectId == projectId &&
                            x.Equipment.DistrictEquipmentTypeId == districtEquipmentTypeId)
                .ToList();

            return new ObjectResult(new HetsResponse(agreements));
        }

        /// <summary>
        /// Delete a blank rental agreement
        /// </summary>
        /// <param name="id">id of Blank RentalAgreement to delete</param>
        [HttpPost]
        [Route("deleteBlankAgreement/{id}")]
        [SwaggerOperation("DeleteBlankRentalAgreementPost")]
        [SwaggerResponse(200, type: typeof(HetRentalAgreement))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult DeleteBlankRentalAgreementPost([FromRoute]int id)
        {
            // get current users district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            HetDistrict district = _context.HetDistrict.AsNoTracking()
                .FirstOrDefault(x => x.DistrictId.Equals(districtId));

            if (district == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            int? statusId = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // validate agreement id
            bool exists = _context.HetRentalAgreement.Any(a => a.RentalAgreementId == id);

            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get agreement and validate
            HetRentalAgreement agreement = _context.HetRentalAgreement
                .First(a => a.RentalAgreementId == id);

            if (agreement.RentalAgreementStatusTypeId != statusId)
            {
                return new ObjectResult(new HetsResponse("HETS-25", ErrorViewModel.GetDescription("HETS-25", _configuration)));
            }

            if (agreement.DistrictId != districtId)
            {
                return new ObjectResult(new HetsResponse("HETS-26", ErrorViewModel.GetDescription("HETS-26", _configuration)));
            }

            if (agreement.RentalRequestId != null)
            {
                return new ObjectResult(new HetsResponse("HETS-27", ErrorViewModel.GetDescription("HETS-27", _configuration)));
            }

            // delete the agreement
            _context.HetRentalAgreement.Remove(agreement);
            _context.SaveChanges();
            
            // return rental agreement
            return new ObjectResult(new HetsResponse(agreement));
        }

        /// <summary>
        /// Update a blank rental agreement
        /// </summary>
        /// <param name="id">id of Blank RentalAgreement to delete</param>
        /// <param name="agreement"></param>
        [HttpPost]
        [Route("updateCloneBlankAgreement/{id}")]
        [SwaggerOperation("UpdateCloneBlankRentalAgreementPost")]
        [SwaggerResponse(200, type: typeof(HetRentalAgreement))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult UpdateCloneBlankRentalAgreementPost([FromRoute]int id, [FromBody]HetRentalAgreement agreement)
        {
            // check the ids 
            if (id != agreement.RentalAgreementId) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get current users district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            HetDistrict district = _context.HetDistrict.AsNoTracking()
                .FirstOrDefault(x => x.DistrictId.Equals(districtId));

            if (district == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // validate agreement id
            bool exists = _context.HetRentalAgreement.Any(a => a.RentalAgreementId == id);

            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get agreement and update
            HetRentalAgreement agreementUpd = _context.HetRentalAgreement
                .First(a => a.RentalAgreementId == id);

            agreementUpd.ConcurrencyControlNumber = agreement.ConcurrencyControlNumber;
            agreementUpd.ProjectId = agreement.ProjectId;
            agreementUpd.EquipmentId = agreement.EquipmentId;
            agreementUpd.EstimateHours = agreement.EstimateHours;
            agreementUpd.EstimateStartWork = agreement.EstimateStartWork;
            agreementUpd.RateComment = agreement.RateComment?.Trim();
            agreementUpd.RatePeriodTypeId = agreement.RatePeriodTypeId;
            agreementUpd.EquipmentRate = agreement.EquipmentRate;
            agreementUpd.Note = agreement.Note?.Trim();
            agreementUpd.DatedOn = agreement.DatedOn;

            // update conditions
            List<HetRentalAgreementCondition> curConditions = _context.HetRentalAgreementCondition.AsNoTracking()
                .Where(x => x.RentalAgreementId == id).ToList();
            
            List<HetRentalAgreementCondition> newConditions = agreement.HetRentalAgreementCondition.ToList();

            foreach (HetRentalAgreementCondition curCondition in curConditions)
            {
                bool remove = true;

                foreach (HetRentalAgreementCondition newCondition in newConditions)
                {
                    if (newCondition.RentalAgreementConditionId == curCondition.RentalAgreementConditionId)
                    {
                        remove = false;

                        curCondition.ConcurrencyControlNumber = newCondition.ConcurrencyControlNumber;
                        curCondition.Comment = newCondition.Comment;
                        curCondition.ConditionName = newCondition.ConditionName;

                        _context.HetRentalAgreementCondition.Update(curCondition);

                        break;
                    }
                }

                if (remove)
                {
                    _context.HetRentalAgreementCondition.Remove(curCondition);
                }                
            }

            foreach (HetRentalAgreementCondition newCondition in newConditions)
            {
                bool add = true;

                foreach (HetRentalAgreementCondition curCondition in curConditions)
                {
                    if (newCondition.RentalAgreementConditionId == curCondition.RentalAgreementConditionId)
                    {
                        add = false;
                        break;
                    }
                }

                if (add)
                {
                    HetRentalAgreementCondition condition = new HetRentalAgreementCondition
                    {
                        RentalAgreementId = id,
                        Comment = newCondition.Comment,
                        ConditionName = newCondition.ConditionName
                    };

                    _context.HetRentalAgreementCondition.Add(condition);
                }
            }

            // update rates
            List<HetRentalAgreementRate> curRates = _context.HetRentalAgreementRate.AsNoTracking()
                .Where(x => x.RentalAgreementId == id).ToList();

            List<HetRentalAgreementRate> newRates = agreement.HetRentalAgreementRate.ToList();

            foreach (HetRentalAgreementRate curRate in curRates)
            {
                bool remove = true;

                foreach (HetRentalAgreementRate newRate in newRates)
                {
                    if (newRate.RentalAgreementRateId == curRate.RentalAgreementRateId)
                    {
                        remove = false;

                        curRate.ConcurrencyControlNumber = newRate.ConcurrencyControlNumber;
                        curRate.Comment = newRate.Comment;
                        curRate.Rate = newRate.Rate;
                        curRate.RatePeriodTypeId = newRate.RatePeriodTypeId;
                        curRate.PercentOfEquipmentRate = newRate.PercentOfEquipmentRate;
                        curRate.ComponentName = newRate.ComponentName;
                        curRate.IsAttachment = newRate.IsAttachment;

                        _context.HetRentalAgreementRate.Update(curRate);

                        break;
                    }
                }

                if (remove)
                {
                    _context.HetRentalAgreementRate.Remove(curRate);
                }
            }

            foreach (HetRentalAgreementRate newRate in newRates)
            {
                bool add = true;

                foreach (HetRentalAgreementRate curRate in curRates)
                {
                    if (newRate.RentalAgreementRateId == curRate.RentalAgreementRateId)
                    {
                        add = false;
                        break;
                    }
                }

                if (add)
                {
                    HetRentalAgreementRate rate = new HetRentalAgreementRate
                    {
                        RentalAgreementId = id,
                        Comment = newRate.Comment,
                        Rate = newRate.Rate,
                        RatePeriodTypeId = newRate.RatePeriodTypeId,
                        PercentOfEquipmentRate = newRate.PercentOfEquipmentRate,
                        ComponentName = newRate.ComponentName,
                        IsAttachment = newRate.IsAttachment
                    };

                    _context.HetRentalAgreementRate.Add(rate);
                }
            }

            // update agreement
            _context.HetRentalAgreement.Update(agreementUpd);

            // create new blank agreement as a duplicate
            HetRentalAgreement newAgreement = new HetRentalAgreement
            {
                Number = RentalAgreementHelper.GetRentalAgreementNumber(district, _context),
                DistrictId = districtId,
                RentalAgreementStatusTypeId = agreementUpd.RentalAgreementStatusTypeId,
                RatePeriodTypeId = agreementUpd.RatePeriodTypeId,
                EstimateHours = agreementUpd.EstimateHours,
                EstimateStartWork = agreementUpd.EstimateStartWork,
                RateComment = agreementUpd.RateComment?.Trim(),
                EquipmentRate = agreementUpd.EquipmentRate,
                Note = agreementUpd.Note?.Trim(),
                DatedOn = agreementUpd.DatedOn
            };

            foreach (HetRentalAgreementCondition condition in agreement.HetRentalAgreementCondition)
            {
                HetRentalAgreementCondition newCondition = new HetRentalAgreementCondition
                {
                    RentalAgreementId = id,
                    Comment = condition.Comment,
                    ConditionName = condition.ConditionName
                };

                newAgreement.HetRentalAgreementCondition.Add(newCondition);
            }

            foreach (HetRentalAgreementRate rate in agreement.HetRentalAgreementRate)
            {
                HetRentalAgreementRate newRate = new HetRentalAgreementRate
                {
                    RentalAgreementId = id,
                    Comment = rate.Comment,
                    Rate = rate.Rate,
                    RatePeriodTypeId = rate.RatePeriodTypeId,
                    PercentOfEquipmentRate = rate.PercentOfEquipmentRate,
                    ComponentName = rate.ComponentName,
                    IsAttachment = rate.IsAttachment
                };

                newAgreement.HetRentalAgreementRate.Add(newRate);
            }

            // add new agreement and save changes
            _context.HetRentalAgreement.Add(newAgreement);
            _context.SaveChanges();

            int newAgreementId = newAgreement.RentalAgreementId;

            // retrieve updated rental agreement to return to ui
            return new ObjectResult(new HetsResponse(RentalAgreementHelper.GetRecord(newAgreementId, _context)));
        }

        #endregion
    }
}
