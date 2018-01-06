using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using System.Threading.Tasks;
using PdfServerLib.PdfHelper;

namespace PdfServerLib.Controllers
{
    public class PdfController : Controller
    {
        private INodeServices _nodeServices;

        public PdfController(INodeServices nodeServices)
        {
            _nodeServices = nodeServices;
        }
        
        [HttpGet]
        [Route("pdf/test")]
        public async Task<IActionResult> TestPdf()
        {
            string options = @"{""height"": ""10.5in"",""width"": ""8in"",""orientation"": ""portrait""}";

            PdfRequest request = new PdfRequest()
            {
                Html = "<h1>Hello World<h1>",
                Options = options,
                PdfJsUrl = "./wwwroot/js/pdf.js"
            };           
            
            JsonResponse result = await PdfDocument.BuildPDF(_nodeServices, request);            

            return File(result.Data, "application/pdf");
        }        
    }
}
