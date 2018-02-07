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
        public IActionResult TestPdf()
        {
            PdfRequest pdfRequest = new PdfRequest()
            {
                Html = "<h1>Hello World<h1>",
                PdfFileName = "HelloWorld"
            };

            byte[] pdfResponse = PdfDocument.BuildPdf(_configuration, pdfRequest);

            return File(pdfResponse, "application/pdf");
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
                PdfRequest pdfRequest = new PdfRequest()
                {
                    Html = "<h1>" + result + "<h1>",
                    PdfFileName = "TestTemplate"
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
