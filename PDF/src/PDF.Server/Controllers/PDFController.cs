using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace PDF.Server.Controllers
{
    /// <summary>
    /// Pdf Request
    /// </summary>
    public class PdfRequest
    {
        /// <summary>
        /// Html Content
        /// </summary>
        public string Html { get; set; }

        /// <summary>
        /// Pdf Options
        /// </summary>
        public string Options { get; set; }
    }

    /// <summary>
    /// Json Response
    /// </summary>
    public class JsonResponse
    {
        /// <summary>
        /// Response Type
        /// </summary>
        public string Type;

        /// <summary>
        /// Response Data
        /// </summary>
        public byte[] Data;
    }

    /// <summary>
    /// Pdf Controller
    /// </summary>
    [Route("api/[controller]")] 
    public class PdfController : Controller
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Pdf Controller Constructor
        /// </summary>
        /// <param name="loggerFactory"></param>
        public PdfController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(PdfController));
        }

        /// <summary>
        /// Build PDF document
        /// </summary>
        /// <param name="nodeServices"></param>
        /// <param name="rawdata"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BuildPDF")]
        public async Task<IActionResult> BuildPdf([FromServices] INodeServices nodeServices, [FromBody]  PdfRequest rawdata)
        {
            _logger.LogInformation("[BuildPdf] Starting pdf rendering");

            // get rendering options (portrait, etc.)
            JObject options = JObject.Parse(rawdata.Options);

            // execute the Node.js component to generate a PDF
            JsonResponse result = await nodeServices.InvokeAsync<JsonResponse>("./pdf.js", rawdata.Html, options);

            _logger.LogInformation("Rendered document");

            return new FileContentResult(result.Data, "application/pdf");            
        }
    }
}
