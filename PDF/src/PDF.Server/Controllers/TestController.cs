using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PDF.Server.Helpers;

namespace PDF.Server.Controllers
{
    /// <summary>
    /// Test Controller - Used to test Pdf generation functionality
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class TestController : Controller
    {
        private readonly INodeServices _nodeServices;
        private readonly IConfigurationRoot _configuration;

        public TestController(INodeServices nodeServices, IConfigurationRoot configuration)
        {
            _nodeServices = nodeServices;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("pdf/testPdf")]
        public async Task<IActionResult> TestPdf()
        {
            string options = @"{""height"": ""10.5in"",""width"": ""8in"",""orientation"": ""portrait""}";

            PdfRequest request = new PdfRequest()
            {
                Html = "<h1>Hello World<h1>",
                Options = options,
                PdfJsUrl = _configuration.GetSection("Constants").GetSection("PdfJsUrl").Value
            };

            JsonResponse result = await PdfDocument.BuildPdf(_nodeServices, request);

            return File(result.Data, "application/pdf");
        }

        [HttpGet]
        [Route("pdf/testTemplate")]
        public async Task<IActionResult> TestTemplate()
        {
            try
            {
                // *************************************************************
                // Create output using json and mustache template
                // *************************************************************
                RenderRequest request = new RenderRequest()
                {
                    JsonString = "{\"title\": \"Sample Template\", \"name\": \"McTesty\"}",
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
                    Html = "<h1>" + result + "<h1>",
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
