using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.NodeServices;

namespace Pdf.Server.Helpers
{    
    public class PdfRequest
    {
        public string Html { get; set; }
        public string PdfFileName { get; set; }
        public string RenderJsUrl { get; set; }
    }    

    /// <summary>
    /// Generates a Pdf Document using html-pdf
    /// </summary>
    public static class PdfDocument
    {
        public static async Task<byte[]> BuildPdf(INodeServices nodeServices, PdfRequest request)
        {
            try
            {
                // validate request
                if (string.IsNullOrEmpty(request.PdfFileName))
                {
                    throw new ArgumentException("Missing PdfFileName");
                }

                if (string.IsNullOrEmpty(request.Html))
                {
                    throw new ArgumentException("Missing Html content");
                }

                // call report js to generate pdf response
                byte[] result = await nodeServices.InvokeAsync<byte[]>(request.RenderJsUrl, request.Html);
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
