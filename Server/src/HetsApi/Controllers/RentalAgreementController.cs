using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
                return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            bool exists = _context.HetRentalAgreement.Any(a => a.RentalAgreementId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetRentalAgreement agreement = _context.HetRentalAgreement
                .Include(a => a.HetRentalAgreementRate)
                .First(a => a.RentalAgreementId == id);

            // populate overtime rates
            agreement.HetRentalAgreementOvertimeRate = agreement.HetRentalAgreementRate.Where(x => x.Overtime).ToList();

            int? statusId = StatusHelper.GetStatusId(item.Status, "rentalAgreementStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get overtime records
            List<HetProvincialRateType> overtime = _context.HetProvincialRateType.AsNoTracking()
                .Where(x => x.Overtime)
                .ToList();

            // get rate period type for the agreement
            int? rateTypeId = StatusHelper.GetRatePeriodId(item.RatePeriod, _context);

            if (rateTypeId == null)
            {
                throw new DataException("Rate Period Id cannot be null");
            }

            string city = item.AgreementCity;

            if (!string.IsNullOrEmpty(city))
            {
                city = city.Trim();
            }

            // update the agreement record
            agreement.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
            agreement.DatedOn = item.DatedOn;
            agreement.AgreementCity = city;
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

            // update the agreement overtime records (default overtime flag)
            if (item.HetRentalAgreementOvertimeRate != null)
            {
                foreach (HetRentalAgreementRate rate in item.HetRentalAgreementOvertimeRate)
                {
                    bool found = false;

                    foreach (HetRentalAgreementRate agreementRate in agreement.HetRentalAgreementRate)
                    {
                        if (agreementRate.RentalAgreementRateId == rate.RentalAgreementRateId)
                        {
                            agreementRate.ConcurrencyControlNumber = rate.ConcurrencyControlNumber;
                            agreementRate.Comment = rate.Comment;
                            agreementRate.Overtime = true;
                            agreementRate.Active = rate.Active;
                            agreementRate.IsIncludedInTotal = rate.IsIncludedInTotal;
                            agreementRate.Rate = rate.Rate;

                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        // add the rate
                        HetRentalAgreementRate newAgreementRate = new HetRentalAgreementRate
                        {
                            ConcurrencyControlNumber = rate.ConcurrencyControlNumber,
                            Comment = rate.Comment,
                            ComponentName = rate.ComponentName,
                            Overtime = true,
                            Active = true,
                            IsIncludedInTotal = rate.IsIncludedInTotal,
                            Rate = rate.Rate
                        };

                        HetProvincialRateType overtimeRate = overtime.FirstOrDefault(x => x.Description == rate.Comment);

                        if (overtimeRate != null)
                        {
                            newAgreementRate.ComponentName = overtimeRate.RateType;
                        }

                        if (agreement.HetRentalAgreementRate == null)
                        {
                            agreement.HetRentalAgreementRate = new List<HetRentalAgreementRate>();
                        }

                        agreement.HetRentalAgreementRate.Add(newAgreementRate);
                    }
                }
            }

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
            if (item == null) return new BadRequestObjectResult(new HetsResponse("HETS-04", ErrorViewModel.GetDescription("HETS-04", _configuration)));

            // set the rate period type id
            int? rateTypeId = StatusHelper.GetRatePeriodId(item.RatePeriod, _context);

            if (rateTypeId == null)
            {
                throw new DataException("Rate Period Id cannot be null");
            }

            // get overtime records
            List<HetProvincialRateType> overtime = _context.HetProvincialRateType.AsNoTracking()
                .Where(x => x.Overtime)
                .ToList();

            // get status for new agreement
            int? statusId = StatusHelper.GetStatusId(item.Status, "rentalAgreementStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get user info - agreement city
            User user = UserAccountHelper.GetUser(_context, _httpContext);
            string agreementCity = user.AgreementCity;

            // create agreement
            HetRentalAgreement agreement = new HetRentalAgreement
            {
                Number = RentalAgreementHelper.GetRentalAgreementNumber(item.Equipment, _context),
                DatedOn = item.DatedOn,
                AgreementCity = agreementCity,
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

            // agreement overtime records (default overtime flag)
            if (item.HetRentalAgreementOvertimeRate != null)
            {
                foreach (HetRentalAgreementRate rate in item.HetRentalAgreementOvertimeRate)
                {
                    // add the rate
                    HetRentalAgreementRate newAgreementRate = new HetRentalAgreementRate
                    {
                        ConcurrencyControlNumber = rate.ConcurrencyControlNumber,
                        Comment = rate.Comment,
                        ComponentName = rate.ComponentName,
                        Overtime = true,
                        Active = true,
                        IsIncludedInTotal = rate.IsIncludedInTotal,
                        Rate = rate.Rate
                    };

                    HetProvincialRateType overtimeRate = overtime.FirstOrDefault(x => x.Description == rate.Comment);

                    if (overtimeRate != null)
                    {
                        newAgreementRate.ComponentName = overtimeRate.RateType;
                    }

                    if (agreement.HetRentalAgreementRate == null)
                    {
                        agreement.HetRentalAgreementRate = new List<HetRentalAgreementRate>();
                    }

                    agreement.HetRentalAgreementRate.Add(newAgreementRate);
                }
            }

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
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetRentalAgreement agreement = _context.HetRentalAgreement.First(a => a.RentalAgreementId == id);

            // release (terminate) rental agreement
            int? statusIdComplete = StatusHelper.GetStatusId(HetRentalAgreement.StatusComplete, "rentalAgreementStatus", _context);
            if (statusIdComplete == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

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

            // get user info - agreement city
            User user = UserAccountHelper.GetUser(_context, _httpContext);
            string agreementCity = user.AgreementCity;

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
                RentalAgreementPdfViewModel rentalAgreementPdfViewModel = RentalAgreementHelper.ToPdfModel(rentalAgreement, agreementCity);

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
                string ownerName = rentalAgreement.Equipment?.Owner?.OrganizationName?.Trim().ToLower();
                ownerName = CleanName(ownerName);
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
                    return new BadRequestObjectResult(new HetsResponse("HETS-05", ErrorViewModel.GetDescription("HETS-05", _configuration)));
                }

                // problem occured
                return new BadRequestObjectResult(new HetsResponse("HETS-05", ErrorViewModel.GetDescription("HETS-05", _configuration)));
            }

            // record not found
            return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
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

        private static string CleanName(string name)
        {
            if (name == null) return "";

            name = name.Replace("'", "");
            name = name.Replace("<", "");
            name = name.Replace(">", "");
            name = name.Replace("\"", "");
            name = name.Replace("|", "");
            name = name.Replace("?", "");
            name = name.Replace("*", "");
            name = name.Replace(":", "");
            name = name.Replace("/", "");
            name = name.Replace("\\", "");

            return name;
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
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get current district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            // return time records
            return new ObjectResult(new HetsResponse(RentalAgreementHelper.GetTimeRecords(id, districtId, _context, _configuration)));
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
            if (!exists || item == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // set the time period type id
            int? timePeriodTypeId = StatusHelper.GetTimePeriodId(item.TimePeriod, _context);
            if (timePeriodTypeId == null) throw new DataException("Time Period Id cannot be null");

            // get current district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            // add or update time record
            if (item.TimeRecordId > 0)
            {
                // get record
                HetTimeRecord time = _context.HetTimeRecord.First(a => a.TimeRecordId == item.TimeRecordId);

                // not found
                if (time == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

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
            return new ObjectResult(new HetsResponse(RentalAgreementHelper.GetTimeRecords(id, districtId, _context, _configuration)));
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
            if (!exists || items == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get current district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

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
                    if (time == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

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
            return new ObjectResult(new HetsResponse(RentalAgreementHelper.GetTimeRecords(id, districtId, _context, _configuration)));
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
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

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
            if (!exists || item == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // add or update rate records
            if (item.RentalAgreementRateId > 0)
            {
                // get record
                HetRentalAgreementRate rate = _context.HetRentalAgreementRate.FirstOrDefault(a => a.RentalAgreementRateId == item.RentalAgreementRateId);

                // not found
                if (rate == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

                rate.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                rate.Comment = item.Comment;
                rate.ComponentName = item.ComponentName;
                rate.Overtime = false;
                rate.Active = true;
                rate.IsIncludedInTotal = item.IsIncludedInTotal;
                rate.Rate = item.Rate;
                rate.Set = item.Set;
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
                    Overtime = false,
                    Active = true,
                    IsIncludedInTotal = item.IsIncludedInTotal,
                    Rate = item.Rate,
                    Set = item.Set
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
            if (!exists || items == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // process each rate records
            foreach (HetRentalAgreementRate item in items)
            {
                // add or update rate records
                if (item.RentalAgreementRateId > 0)
                {
                    // get record
                    HetRentalAgreementRate rate = _context.HetRentalAgreementRate.FirstOrDefault(a => a.RentalAgreementRateId == item.RentalAgreementRateId);

                    // not found
                    if (rate == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

                    rate.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                    rate.Comment = item.Comment;
                    rate.ComponentName = item.ComponentName;
                    rate.Overtime = false;
                    rate.Active = true;
                    rate.IsIncludedInTotal = item.IsIncludedInTotal;
                    rate.Rate = item.Rate;
                    rate.Set = item.Set;
                }
                else // add rate records
                {
                    int agreementId = item.RentalAgreement.RentalAgreementId;

                    HetRentalAgreementRate rate = new HetRentalAgreementRate
                    {
                        RentalAgreementId = agreementId,
                        Comment = item.Comment,
                        ComponentName = item.ComponentName,
                        Overtime = false,
                        Active = true,
                        IsIncludedInTotal = item.IsIncludedInTotal,
                        Rate = item.Rate,
                        Set = item.Set
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
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

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
            if (!exists || item == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // add or update condition records
            if (item.RentalAgreementConditionId > 0)
            {
                // get record
                HetRentalAgreementCondition condition = _context.HetRentalAgreementCondition
                    .FirstOrDefault(a => a.RentalAgreementConditionId == item.RentalAgreementConditionId);

                // not found
                if (condition == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

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
            if (!exists || items == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

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
                    if (condition == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

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

            if (district == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get active status id
            int? statusId = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // HETS-825 - MAX number of Blank Rental Agreements and limit the functionality to ADMINS only
            // * Limit Blank rental agreements to a maximum of 3
            List<HetRentalAgreement> agreements = _context.HetRentalAgreement.AsNoTracking()
                .Include(x => x.RentalAgreementStatusType)
                .Include(x => x.District)
                .Include(x => x.Project)
                .Include(x => x.Equipment)
                .Where(x => x.District.DistrictId == districtId &&
                            x.RentalRequestId == null &&
                            x.RentalRequestRotationListId == null &&
                            x.RentalAgreementStatusTypeId == statusId)
                .ToList();

            string tempMax = _configuration.GetSection("Constants:Maximum-Blank-Agreements").Value;
            bool isNumeric = int.TryParse(tempMax, out int max);
            if (!isNumeric) max = 3; // default to 3

            if (agreements.Count >= max)
            {
                return new BadRequestObjectResult(new HetsResponse("HETS-29", ErrorViewModel.GetDescription("HETS-29", _configuration)));
            }

            // set the rate period type id
            int? rateTypeId = StatusHelper.GetRatePeriodId(HetRatePeriodType.PeriodHourly, _context);
            if (rateTypeId == null) return new BadRequestObjectResult(new HetsResponse("HETS-24", ErrorViewModel.GetDescription("HETS-24", _configuration)));

            // create new agreement
            HetRentalAgreement agreement = new HetRentalAgreement
            {
                Number = RentalAgreementHelper.GetRentalAgreementNumber(district, _context),
                DistrictId = districtId,
                RentalAgreementStatusTypeId = (int)statusId,
                RatePeriodTypeId = (int)rateTypeId
            };

            // add overtime rates
            List<HetProvincialRateType> overtime = _context.HetProvincialRateType.AsNoTracking()
                .Where(x => x.Overtime)
                .ToList();

            // agreement overtime records (default overtime flag)
            foreach (HetProvincialRateType rate in overtime)
            {
                // add the rate
                HetRentalAgreementRate newAgreementRate = new HetRentalAgreementRate
                {
                    Comment = rate.Description,
                    ComponentName = rate.RateType,
                    Overtime = true,
                    Active = rate.Active,
                    IsIncludedInTotal = rate.IsIncludedInTotal,
                    Rate = rate.Rate
                };

                if (agreement.HetRentalAgreementRate == null)
                {
                    agreement.HetRentalAgreementRate = new List<HetRentalAgreementRate>();
                }

                agreement.HetRentalAgreementRate.Add(newAgreementRate);
            }

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
        [SwaggerOperation("BlankRentalAgreementsGet")]
        [SwaggerResponse(200, type: typeof(List<HetRentalAgreement>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult BlankRentalAgreementsGet()
        {
            // get the current district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            // get active status id
            int? statusId = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get all active "blank" agreements
            List<HetRentalAgreement> agreements = _context.HetRentalAgreement.AsNoTracking()
                .Include(x => x.RentalAgreementStatusType)
                .Include(x => x.District)
                .Include(x => x.Project)
                .Include(x => x.Equipment)
                .Where(x => x.District.DistrictId == districtId &&
                            x.RentalRequestId == null &&
                            x.RentalRequestRotationListId == null &&
                            x.RentalAgreementStatusTypeId == statusId)
                .ToList();

            return new ObjectResult(new HetsResponse(agreements));
        }

        /// <summary>
        /// Get blank rental agreements (for the current district)
        /// By Project Id and Equipment Id (ACTIVE AGREEMENTS ONLY)
        /// </summary>
        [HttpGet]
        [Route("blankAgreements/{projectId}/{equipmentId}")]
        [SwaggerOperation("BlankRentalAgreementLookupGet")]
        [SwaggerResponse(200, type: typeof(List<HetRentalAgreement>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult BlankRentalAgreementLookupGet([FromRoute]int projectId, [FromRoute]int equipmentId)
        {
            // get the current district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            // get agreement status
            int? statusId = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

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
                            x.EquipmentId == equipmentId &&
                            x.RentalAgreementStatusTypeId == statusId)
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

            if (district == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            int? statusId = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // validate agreement id
            bool exists = _context.HetRentalAgreement.Any(a => a.RentalAgreementId == id);

            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get agreement and validate
            HetRentalAgreement agreement = _context.HetRentalAgreement
                .Include(a => a.HetRentalAgreementRate)
                .Include(a => a.HetRentalAgreementCondition)
                .First(a => a.RentalAgreementId == id);

            if (agreement.RentalAgreementStatusTypeId != statusId)
            {
                return new BadRequestObjectResult(new HetsResponse("HETS-25", ErrorViewModel.GetDescription("HETS-25", _configuration)));
            }

            if (agreement.DistrictId != districtId)
            {
                return new BadRequestObjectResult(new HetsResponse("HETS-26", ErrorViewModel.GetDescription("HETS-26", _configuration)));
            }

            if (agreement.RentalRequestId != null)
            {
                return new BadRequestObjectResult(new HetsResponse("HETS-27", ErrorViewModel.GetDescription("HETS-27", _configuration)));
            }

            // delete rate
            foreach (HetRentalAgreementRate item in agreement.HetRentalAgreementRate)
            {
                _context.HetRentalAgreementRate.Remove(item);
            }

            // delete conditions
            foreach (HetRentalAgreementCondition item in agreement.HetRentalAgreementCondition)
            {
                _context.HetRentalAgreementCondition.Remove(item);
            }

            // delete the agreement
            _context.HetRentalAgreement.Remove(agreement);
            _context.SaveChanges();

            // return rental agreement
            return new ObjectResult(new HetsResponse(agreement));
        }

        /// <summary>
        /// Clone a blank rental agreement
        /// </summary>
        /// <param name="id">id of Blank RentalAgreement to clone</param>
        /// <param name="agreement"></param>
        [HttpPost]
        [Route("updateCloneBlankAgreement/{id}")]
        [SwaggerOperation("CloneBlankRentalAgreementPost")]
        [SwaggerResponse(200, type: typeof(HetRentalAgreement))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult CloneBlankRentalAgreementPost([FromRoute]int id, [FromBody]HetRentalAgreement agreement)
        {
            // check the ids
            if (id != agreement.RentalAgreementId) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get current users district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            HetDistrict district = _context.HetDistrict.AsNoTracking()
                .FirstOrDefault(x => x.DistrictId.Equals(districtId));

            if (district == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // validate agreement id
            bool exists = _context.HetRentalAgreement.Any(a => a.RentalAgreementId == id);

            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // add overtime rates
            List<HetProvincialRateType> overtime = _context.HetProvincialRateType.AsNoTracking()
                .Where(x => x.Overtime)
                .ToList();

            // get agreement and clone
            HetRentalAgreement oldAgreement = _context.HetRentalAgreement.AsNoTracking()
                .Include(a => a.HetRentalAgreementRate)
                .Include(a => a.HetRentalAgreementCondition)
                .First(a => a.RentalAgreementId == id);

            // create new blank agreement as a duplicate
            HetRentalAgreement newAgreement = new HetRentalAgreement
            {
                Number = RentalAgreementHelper.GetRentalAgreementNumber(district, _context),
                DistrictId = districtId,
                RentalAgreementStatusTypeId = oldAgreement.RentalAgreementStatusTypeId,
                RatePeriodTypeId = oldAgreement.RatePeriodTypeId,
                EstimateHours = oldAgreement.EstimateHours,
                EstimateStartWork = oldAgreement.EstimateStartWork,
                RateComment = oldAgreement.RateComment?.Trim(),
                EquipmentRate = oldAgreement.EquipmentRate,
                Note = oldAgreement.Note?.Trim(),
                DatedOn = oldAgreement.DatedOn,
                AgreementCity = oldAgreement.AgreementCity
            };

            foreach (HetRentalAgreementCondition condition in oldAgreement.HetRentalAgreementCondition)
            {
                HetRentalAgreementCondition newCondition = new HetRentalAgreementCondition
                {
                    RentalAgreementId = id,
                    Comment = condition.Comment,
                    ConditionName = condition.ConditionName
                };

                newAgreement.HetRentalAgreementCondition.Add(newCondition);
            }

            if (oldAgreement.HetRentalAgreementRate != null)
            {
                foreach (HetRentalAgreementRate rate in oldAgreement.HetRentalAgreementRate)
                {
                    HetRentalAgreementRate newRate = new HetRentalAgreementRate
                    {
                        RentalAgreementId = id,
                        Comment = rate.Comment,
                        Rate = rate.Rate,
                        ComponentName = rate.ComponentName,
                        Active = rate.Active,
                        IsIncludedInTotal = rate.IsIncludedInTotal,
                        Overtime = rate.Overtime
                    };

                    newAgreement.HetRentalAgreementRate.Add(newRate);
                }
            }

            // update overtime rates (and add if they don't exist)
            foreach (HetProvincialRateType overtimeRate in overtime)
            {
                bool found = newAgreement.HetRentalAgreementRate.Any(x => x.ComponentName == overtimeRate.RateType);

                if (found)
                {
                    HetRentalAgreementRate rate = newAgreement.HetRentalAgreementRate
                        .First(x => x.ComponentName == overtimeRate.RateType);

                    rate.Rate = overtimeRate.Rate;
                }
                else
                {
                    HetRentalAgreementRate newRate = new HetRentalAgreementRate
                    {
                        RentalAgreementId = id,
                        Comment = overtimeRate.Description,
                        Rate = overtimeRate.Rate,
                        ComponentName = overtimeRate.RateType,
                        Active = overtimeRate.Active,
                        IsIncludedInTotal = overtimeRate.IsIncludedInTotal,
                        Overtime = overtimeRate.Overtime
                    };

                    newAgreement.HetRentalAgreementRate.Add(newRate);
                }
            }

            // remove non-existent overtime rates
            List<string> remove =
                (from overtimeRate in newAgreement.HetRentalAgreementRate
                    where overtimeRate.Overtime
                    let found = overtime.Any(x => x.RateType == overtimeRate.ComponentName)
                    where !found select overtimeRate.ComponentName).ToList();

            if (remove.Count > 0)
            {
                foreach (string component in remove)
                {
                    newAgreement.HetRentalAgreementRate.Remove(
                        newAgreement.HetRentalAgreementRate.First(x => x.ComponentName == component));
                }
            }

            // add new agreement and save changes
            _context.HetRentalAgreement.Add(newAgreement);
            _context.SaveChanges();

            int newAgreementId = newAgreement.RentalAgreementId;

            // retrieve updated rental agreement to return to ui
            return new ObjectResult(new HetsResponse(RentalAgreementHelper.GetRecord(newAgreementId, _context)));
        }

        #endregion

        #region Search Rental Requests

        /// <summary>
        /// Find the latest agreement by project and equipment id
        /// </summary>
        /// <remarks>Used for the time entry page.</remarks>
        /// <param name="equipmentId">Equipment Id</param>
        /// <param name="projectId">Project Id</param>
        [HttpGet]
        [Route("latest/{projectId}/{equipmentId}")]
        [SwaggerOperation("RentalAgreementLatestGet")]
        [SwaggerResponse(200, type: typeof(List<RentalRequestLite>))]
        public virtual IActionResult RentalRequestsSearchGet([FromRoute]int projectId, [FromRoute]int equipmentId)
        {
            // find the latest rental agreement
            HetRentalAgreement agreement = _context.HetRentalAgreement.AsNoTracking()
                .OrderByDescending(x => x.AppCreateTimestamp)
                .FirstOrDefault(x => x.EquipmentId == equipmentId &&
                                     x.ProjectId == projectId);

            // if nothing exists - return an error message
            if (agreement == null) return new NotFoundObjectResult(new HetsResponse("HETS-35", ErrorViewModel.GetDescription("HETS-35", _configuration)));

            // get user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            // get fiscal year
            HetDistrictStatus status = _context.HetDistrictStatus.AsNoTracking()
                .First(x => x.DistrictId == districtId);

            int? fiscalYearStart = status.CurrentFiscalYear;
            if (fiscalYearStart == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            DateTime fiscalStart = new DateTime((int)fiscalYearStart, 4, 1);

            // validate that agreement is in the current fiscal year
            DateTime agreementDate = agreement.DatedOn ?? agreement.DbCreateTimestamp;

            if (agreementDate < fiscalStart) return new NotFoundObjectResult(new HetsResponse("HETS-36", ErrorViewModel.GetDescription("HETS-36", _configuration)));

            // return to the client
            return new ObjectResult(new HetsResponse(agreement));
        }

        #endregion

        #region AIT Report

        /// <summary>
        /// Get rental agreements for AIT Report
        /// </summary>
        /// <param name="projects">Projects (comma separated list of id numbers)</param>
        /// <param name="districtEquipmentType">District Equipment Types (comma separated list of equipment types)</param>
        /// <param name="equipment">Equipment (comma separated list of id numbers)</param>
        /// <param name="rentalAgreementNumber">Rental Agreement Number</param>
        /// <param name="startDate">Start date for Dated On</param>
        /// <param name="endDate">End date for Dated On</param>
        /// <returns>AIT report</returns>
        [HttpGet]
        [Route("aitReport")]
        [SwaggerOperation("AitReportGet")]
        [SwaggerResponse(200, type: typeof(List<RentalRequestHires>))]
        public virtual IActionResult AitReportGet([FromQuery]string projects,
            [FromQuery]string districtEquipmentType, [FromQuery]string equipment, [FromQuery]string rentalAgreementNumber,
            [FromQuery]DateTime? startDate, [FromQuery]DateTime? endDate)
        {
            int?[] projectArray = ArrayHelper.ParseIntArray(projects);
            int?[] districtEquipmentTypeArray = ArrayHelper.ParseIntArray(districtEquipmentType);
            int?[] equipmentArray = ArrayHelper.ParseIntArray(equipment);

            List<AitReport> result;

            IQueryable<HetRentalAgreement> agreements = _context.HetRentalAgreement.AsNoTracking()
                .Include(x => x.RentalAgreementStatusType)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.DistrictEquipmentType)
                        .ThenInclude(d => d.EquipmentType)
                .Include(x => x.Project);

            // limit to user's current district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);
            agreements = agreements.Where(x => x.DistrictId == districtId);

            // HET-1137 do not show placeholder rental agreements created to imported from BCBid, (ones with agreement# BCBid-XX-XXXX)
            agreements = agreements.Where(x => !x.Number.StartsWith("BCBid"));

            if (!string.IsNullOrWhiteSpace(rentalAgreementNumber))
            {
                result = agreements
                    .Where(x => x.Number.Contains(rentalAgreementNumber.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    .Select(x => AitReport.MapFromHetRentalAgreement(x))
                    .ToList();

                return new ObjectResult(new HetsResponse(result));
            }

            if (projectArray != null && projectArray.Length > 0)
            {
                agreements = agreements.Where(x => projectArray.Contains(x.ProjectId));
            }

            if (districtEquipmentTypeArray != null && districtEquipmentTypeArray.Length > 0)
            {
                agreements = agreements.Where(x => districtEquipmentTypeArray.Contains(x.Equipment.DistrictEquipmentTypeId));
            }

            if (equipmentArray != null && equipmentArray.Length > 0)
            {
                agreements = agreements.Where(x => equipmentArray.Contains(x.EquipmentId));
            }

            if (startDate != null)
            {
                agreements = agreements.Where(x => x.DatedOn >= startDate);
            }

            if (endDate != null)
            {
                agreements = agreements.Where(x => x.DatedOn <= endDate);
            }

            result = agreements
                .Select(x => AitReport.MapFromHetRentalAgreement(x))
                .ToList();

            return new ObjectResult(new HetsResponse(result));
        }

        #endregion

    }
}
