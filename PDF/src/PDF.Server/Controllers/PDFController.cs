using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
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

        public PdfController(INodeServices nodeServices, IConfigurationRoot configuration)
        {
            _nodeServices = nodeServices;
            _configuration = configuration;            
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
                // *************************************************************
                // Create output using json and mustache template
                // *************************************************************
                RenderRequest request = new RenderRequest()
                {
                    JsonString = rentalAgreementJson,
                    RenderJsUrl = _configuration.GetSection("Constants").GetSection("RenderJsUrl").Value,
                    Template = _configuration.GetSection("Constants").GetSection("RentalTemplate").Value
                };

                string result = await TemplateHelper.RenderDocument(_nodeServices, request);

                // *************************************************************
                // Convert results to Pdf
                // ************************************************************* 
                string fileName = "RentalAgreement_" + name + ".pdf"; // to do - add id

                PdfRequest pdfRequest = new PdfRequest()
                {
                    Html = result,
                    PdfFileName = fileName
                };

                byte[] pdfResponse = PdfDocument.BuildPdf(_configuration, pdfRequest);

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
