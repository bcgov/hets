using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PDF.Server.Helpers;

namespace PDF.Server.Controllers
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
                _logger.LogInformation("GetRentalAgreementPdf [FileName: {0}]", name);

                // *************************************************************
                // Create output using json and mustache template
                // *************************************************************
                RenderRequest request = new RenderRequest()
                {
                    JsonString = rentalAgreementJson,
                    RenderJsUrl = _configuration.GetSection("Constants").GetSection("RenderJsUrl").Value,
                    Template = _configuration.GetSection("Constants").GetSection("RentalTemplate").Value
                };

                _logger.LogInformation("GetRentalAgreementPdf [FileName: {0}] - Render Html", name);
                string result = await TemplateHelper.RenderDocument(_nodeServices, request);

                _logger.LogInformation("GetRentalAgreementPdf [FileName: {0}] - Html Length: {1}", name, result.Length);

                // *************************************************************
                // Convert results to Pdf
                // ************************************************************* 
                string fileName = "RentalAgreement_" + name + ".pdf"; // to do - add id

                PdfRequest pdfRequest = new PdfRequest()
                {
                    Html = result,
                    PdfFileName = fileName
                };

                _logger.LogInformation("GetRentalAgreementPdf [FileName: {0}] - Gen Pdf", name);
                byte[] pdfResponseBytes = PdfDocument.BuildPdf(_configuration, pdfRequest);

                // convert to string and log
                string pdfResponse = System.Text.Encoding.UTF8.GetString(pdfResponseBytes);
                _logger.LogInformation("GetRentalAgreementPdf [FileName: {0}] - Pdf Length: {1}", name, pdfResponse.Length);

                _logger.LogInformation("GetRentalAgreementPdf [FileName: {0}] - Done", name);
                return File(pdfResponse, "application/pdf");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
