using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.NodeServices;
using System.Threading.Tasks;

namespace PdfServerLib.PdfHelper
{
    public class PdfRequest
    {
        public string Html { get; set; }
        public string Options { get; set; }
        public string PdfJsUrl { get; set; }
    }

    public class JsonResponse
    {
        public string Type;
        public byte[] Data;
    }

    /// <summary>
    /// Generates a Pdf Document using html-pdf
    /// </summary>
    public static class PdfDocument
    {
        public static async Task<JsonResponse> BuildPDF(INodeServices nodeServices, PdfRequest request)
        {
            JObject options = JObject.Parse(request.Options);
            JsonResponse result = await nodeServices.InvokeAsync<JsonResponse>(request.PdfJsUrl, request.Html, options);
            return result;
        }
    }
}
