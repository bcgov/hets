using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PDF.Server.Helpers;

namespace PDF.Server.Controllers
{
    /// <summary>
    /// Rental Agreement - used to submit data to generate a new rental document
    /// </summary>
    public class RentalAgreement
    {
        public string JsonString { get; set; }
    }

    /// <summary>
    /// Pdf Controller - Main Pdf generation functionality
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

        [HttpPost]
        [Route("pdf/rentalAgreement")]
        public async Task<IActionResult> GetRentalAgreementPdf([FromBody]RentalAgreement rentalAgreement)
        {
            try
            {
                // *************************************************************
                // Create output using json and mustache template
                // *************************************************************
                RenderRequest request = new RenderRequest()
                {
                    JsonString = rentalAgreement.JsonString,
                    RenderJsUrl = _configuration.GetSection("Constants").GetSection("RenderJsUrl").Value,
                    Template = _configuration.GetSection("Constants").GetSection("SampleTemplate").Value
                };

                string result = await TemplateHelper.RenderDocument(_nodeServices, request);

                // *************************************************************
                // Convert results to Pdf
                // *************************************************************
                string options = @"{""height"": ""10.5in"",""width"": ""8in"",""orientation"": ""portrait""}";

                PdfRequest pdfRequest = new PdfRequest()
                {
                    Html = result,
                    Options = options,
                    PdfJsUrl = _configuration.GetSection("Constants").GetSection("PdfJsUrl").Value
                };

                JsonResponse jsonResult = await PdfDocument.BuildPdf(_nodeServices, pdfRequest);

                return File(jsonResult.Data, "application/pdf");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
