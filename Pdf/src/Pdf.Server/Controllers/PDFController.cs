using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pdf.Server.Helpers;

namespace Pdf.Server.Controllers
{    
    /// <summary>
    /// Pdf Controller - Main Pdf generation functionality for HETS
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class PdfController : Controller
    {
        private readonly INodeServices _nodeServices;
        private readonly IConfigurationRoot _configuration;
        private readonly ILogger _logger;

        public PdfController(INodeServices nodeServices, IConfigurationRoot configuration, ILoggerFactory loggerFactory)
        {
            _nodeServices = nodeServices;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<PdfController>();
        }

        /// <summary>
        /// Get HETS Rental Agreement
        /// </summary>
        /// <param name="rentalAgreementJson">Serialized rental agreement</param>
        /// <param name="name">Unique name for the generated Pdf (Result: 'RentalAgreement_' + name + '.pdf')</param>
        /// <returns></returns>
        [HttpPost]
        [Route("pdf/rentalAgreement/{name}")]
        public async Task<IActionResult> GetRentalAgreementPdf([FromBody]string rentalAgreementJson, [FromRoute]string name)
        {
            try
            {
                name = CleanName(name);

                string fileName = "RentalAgreement_" + name + ".pdf";
                _logger.LogInformation("GetRentalAgreementPdf [FileName: {0}]", fileName);

                // *************************************************************
                // Create output using json and mustache template
                // *************************************************************
                HtmlRequest request = new HtmlRequest()
                {
                    JsonString = rentalAgreementJson,
                    RenderJsUrl = _configuration.GetSection("Constants").GetSection("RenderJsUrl").Value,
                    Template = _configuration.GetSection("Constants").GetSection("RentalTemplate").Value
                };

                _logger.LogInformation("GetRentalAgreementPdf [FileName: {0}] - Render Html", fileName);
                string result = await TemplateHelper.RenderDocument(_nodeServices, request);

                _logger.LogInformation("GetRentalAgreementPdf [FileName: {0}] - Html Length: {1}", fileName, result.Length);

                // *************************************************************
                // Convert results to Pdf
                // *************************************************************                 
                PdfRequest pdfRequest = new PdfRequest()
                {
                    Html = result,
                    RenderJsUrl = _configuration.GetSection("Constants").GetSection("PdfJsUrl").Value,
                    PdfFileName = fileName
                };

                _logger.LogInformation("GetRentalAgreementPdf [FileName: {0}] - Gen Pdf", fileName);
                byte[] pdfResponseBytes = await PdfDocument.BuildPdf(_nodeServices, pdfRequest);

                // convert to string and log
                string pdfResponse = System.Text.Encoding.Default.GetString(pdfResponseBytes);
                _logger.LogInformation("GetRentalAgreementPdf [FileName: {0}] - Pdf Length: {1}", fileName, pdfResponse.Length);

                _logger.LogInformation("GetRentalAgreementPdf [FileName: {0}] - Done", fileName);
                return File(pdfResponseBytes, "application/pdf", fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Get HETS Owner Verification Notices
        /// </summary>
        /// <param name="ownersJson">Serialized owner data</param>
        /// <param name="name">Unique name for the generated Pdf</param>
        /// <returns></returns>
        [HttpPost]
        [Route("pdf/ownerVerification/{name}")]
        public async Task<IActionResult> GetOwnerVerificationPdf([FromBody]string ownersJson, [FromRoute]string name)
        {
            try
            {
                name = CleanName(name);

                string fileName = name + ".pdf";
                _logger.LogInformation("GetOwnerVerificationPdf [FileName: {0}]", fileName);

                // *************************************************************
                // Create output using json and mustache template
                // *************************************************************
                HtmlRequest request = new HtmlRequest()
                {
                    JsonString = ownersJson,
                    RenderJsUrl = _configuration.GetSection("Constants").GetSection("RenderJsUrl").Value,
                    Template = _configuration.GetSection("Constants").GetSection("OwnerVerificationTemplate").Value
                };

                _logger.LogInformation("GetOwnerVerificationPdf [FileName: {0}] - Render Html", fileName);
                string result = await TemplateHelper.RenderDocument(_nodeServices, request);

                _logger.LogInformation("GetOwnerVerificationPdf [FileName: {0}] - Html Length: {1}", fileName, result.Length);

                // *************************************************************
                // Convert results to Pdf
                // *************************************************************                 
                PdfRequest pdfRequest = new PdfRequest()
                {
                    Html = result,
                    RenderJsUrl = _configuration.GetSection("Constants").GetSection("PdfJsUrl").Value,
                    PdfFileName = fileName
                };

                _logger.LogInformation("GetOwnerVerificationPdf [FileName: {0}] - Gen Pdf", fileName);
                byte[] pdfResponseBytes = await PdfDocument.BuildPdf(_nodeServices, pdfRequest);

                // convert to string and log
                string pdfResponse = System.Text.Encoding.Default.GetString(pdfResponseBytes);
                _logger.LogInformation("GetOwnerVerificationPdf [FileName: {0}] - Pdf Length: {1}", fileName, pdfResponse.Length);

                _logger.LogInformation("GetOwnerVerificationPdf [FileName: {0}] - Done", fileName);
                return File(pdfResponseBytes, "application/pdf", fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Get HETS Seniority List
        /// </summary>
        /// <param name="seniorityListJson">Serialized seniority list data</param>
        /// <param name="name">Unique name for the generated Pdf (Result: name + '.pdf')</param>
        /// <returns></returns>
        [HttpPost]
        [Route("pdf/seniorityList/{name}")]
        public async Task<IActionResult> GetSeniorityListPdf([FromBody]string seniorityListJson, [FromRoute]string name)
        {
            try
            {
                name = CleanName(name);

                string fileName = name + ".pdf";
                _logger.LogInformation("GetSeniorityListPdf [FileName: {0}]", fileName);

                // *************************************************************
                // Create output using json and mustache template
                // *************************************************************
                if (seniorityListJson == "[]")
                {
                    seniorityListJson = @"{""Empty"": ""true""}";
                }

                HtmlRequest request = new HtmlRequest()
                {
                    JsonString = seniorityListJson,
                    RenderJsUrl = _configuration.GetSection("Constants").GetSection("RenderJsUrl").Value,
                    Template = _configuration.GetSection("Constants").GetSection("SeniorityListTemplate").Value
                };

                _logger.LogInformation("GetSeniorityListPdf [FileName: {0}] - Render Html", fileName);
                string result = await TemplateHelper.RenderDocument(_nodeServices, request);

                _logger.LogInformation("GetSeniorityListPdf [FileName: {0}] - Html Length: {1}", fileName, result.Length);

                // *************************************************************
                // Convert results to Pdf
                // *************************************************************                 
                PdfRequest pdfRequest = new PdfRequest()
                {
                    Html = result,
                    RenderJsUrl = _configuration.GetSection("Constants").GetSection("PdfJsUrl").Value,
                    PdfFileName = fileName
                };

                _logger.LogInformation("GetSeniorityListPdf [FileName: {0}] - Gen Pdf", fileName);
                byte[] pdfResponseBytes = await PdfDocument.BuildPdf(_nodeServices, pdfRequest, true);

                // convert to string and log
                string pdfResponse = System.Text.Encoding.Default.GetString(pdfResponseBytes);
                _logger.LogInformation("GetSeniorityListPdf [FileName: {0}] - Pdf Length: {1}", fileName, pdfResponse.Length);

                _logger.LogInformation("GetSeniorityListPdf [FileName: {0}] - Done", fileName);
                return File(pdfResponseBytes, "application/pdf", fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static string CleanName(string name)
        {
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
    }
}
