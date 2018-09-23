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
        private readonly ILogger _logger;

        public RentalAgreementController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory)
        {
            _context = context;
            _configuration = configuration;
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

            agreement.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
            agreement.DatedOn = item.DatedOn;
            agreement.EquipmentRate = item.EquipmentRate;
            agreement.EstimateHours = item.EstimateHours;
            agreement.EstimateStartWork = item.EstimateStartWork;
            agreement.Note = item.Note;
            agreement.Number = item.Number;
            agreement.RateComment = item.RateComment;
            agreement.RatePeriod = item.RatePeriod;
            agreement.RentalAgreementStatusTypeId = (int)statusId;

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
                .Include(x => x.Equipment).ThenInclude(y => y.Owner).ThenInclude(z => z.PrimaryContact)
                .Include(x => x.Equipment).ThenInclude(y => y.DistrictEquipmentType)
                .Include(x => x.Equipment).ThenInclude(y => y.HetEquipmentAttachment)
                .Include(x => x.Equipment).ThenInclude(y => y.LocalArea.ServiceArea.District.Region)
                .Include(x => x.Project).ThenInclude(p => p.District.Region)
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
                time.WorkedDate = item.WorkedDate;
            }
            else // add time record
            {
                HetTimeRecord time = new HetTimeRecord
                {
                    RentalAgreementId = item.RentalAgreementId,
                    EnteredDate = DateTime.UtcNow,
                    Hours = item.Hours,
                    TimePeriod = item.TimePeriod,
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
                    time.WorkedDate = item.WorkedDate;
                }
                else // add time record
                {
                    HetTimeRecord time = new HetTimeRecord
                    {
                        RentalAgreementId = item.RentalAgreementId,
                        EnteredDate = DateTime.UtcNow,
                        Hours = item.Hours,
                        TimePeriod = item.TimePeriod,
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

                rate.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                rate.Comment = item.Comment;
                rate.ComponentName = item.ComponentName;
                rate.IsAttachment = item.IsAttachment;
                rate.IsIncludedInTotal = item.IsIncludedInTotal;
                rate.PercentOfEquipmentRate = item.PercentOfEquipmentRate;
                rate.Rate = item.Rate;
                rate.RatePeriod = item.RatePeriod;
            }
            else // add rate records
            {
                HetRentalAgreementRate rate = new HetRentalAgreementRate
                {
                    RentalAgreementId = item.RentalAgreementId,
                    Comment = item.Comment,
                    ComponentName = item.ComponentName,
                    IsAttachment = item.IsAttachment,
                    IsIncludedInTotal = item.IsIncludedInTotal,
                    PercentOfEquipmentRate = item.PercentOfEquipmentRate,
                    Rate = item.Rate,
                    RatePeriod = item.RatePeriod
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

                    rate.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                    rate.Comment = item.Comment;
                    rate.ComponentName = item.ComponentName;
                    rate.IsAttachment = item.IsAttachment;
                    rate.IsIncludedInTotal = item.IsIncludedInTotal;
                    rate.PercentOfEquipmentRate = item.PercentOfEquipmentRate;
                    rate.Rate = item.Rate;
                    rate.RatePeriod = item.RatePeriod;

                    // set the rate period type id
                    int? rateTypeId = StatusHelper.GetRatePeriodId(item.RatePeriod, _context);

                    if (rateTypeId == null)
                    {
                        throw new DataException("Rate Period Id cannot be null");
                    }

                    rate.RatePeriodTypeId = (int)rateTypeId;
                }
                else // add rate records
                {
                    HetRentalAgreementRate rate = new HetRentalAgreementRate
                    {
                        RentalAgreementId = item.RentalAgreementId,
                        Comment = item.Comment,
                        ComponentName = item.ComponentName,
                        IsAttachment = item.IsAttachment,
                        IsIncludedInTotal = item.IsIncludedInTotal,
                        PercentOfEquipmentRate = item.PercentOfEquipmentRate,
                        Rate = item.Rate,
                        RatePeriod = item.RatePeriod
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
                HetRentalAgreementCondition condition = new HetRentalAgreementCondition
                {
                    RentalAgreementId = item.RentalAgreementId,
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
                    HetRentalAgreementCondition condition = new HetRentalAgreementCondition
                    {
                        RentalAgreementId = item.RentalAgreementId,
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
    }
}
