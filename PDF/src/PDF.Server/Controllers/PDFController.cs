using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace PDF.Controllers
{
    public class PDFRequest
    {
        public string html { get; set; }
        public string options { get; set; }
    }
    public class JSONResponse
    {
        public string type;
        public byte[] data;
    }
    [Route("api/[controller]")] 
    public class PDFController : Controller
    {
        protected ILogger _logger;

        public PDFController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(PDFController));
        }


        [HttpPost]
        [Route("BuildPDF")]

        public async Task<IActionResult> BuildPDF([FromServices] INodeServices nodeServices, [FromBody]  PDFRequest rawdata )
        {
            JObject options = JObject.Parse(rawdata.options);
            JSONResponse result = null;
            //var options = new { format="letter", orientation= "portrait" }; 

            // execute the Node.js component to generate a PDF
            result = await nodeServices.InvokeAsync<JSONResponse>("./pdf.js", rawdata.html, options);
            options = null;

            return new FileContentResult(result.data, "application/pdf");

            _logger.LogInformation("Rendered document.");
        }

    }
}
