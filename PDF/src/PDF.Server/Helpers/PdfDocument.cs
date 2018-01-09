using System;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.NodeServices;
using System.Threading.Tasks;

namespace PDF.Server.Helpers
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
        public static async Task<JsonResponse> BuildPdf(INodeServices nodeServices, PdfRequest request)
        {
            try
            {
                JObject options = JObject.Parse(request.Options);
                JsonResponse result = await nodeServices.InvokeAsync<JsonResponse>(request.PdfJsUrl, request.Html, options);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }            
        }
    }
}
