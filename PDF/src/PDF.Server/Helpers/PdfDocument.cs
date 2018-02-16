using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace PDF.Server.Helpers
{    
    public class PdfRequest
    {
        public string Html { get; set; }
        public string PdfFileName { get; set; }
    }    

    /// <summary>
    /// Generates a Pdf Document using html-pdf
    /// </summary>
    public static class PdfDocument
    {
        public static byte[] BuildPdf(IConfigurationRoot configuration, PdfRequest request)
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
                
                // pass the request on to the new (weasy) Pdf Micro Service
                string pdfService = configuration["Constants:WeasyPdfService"];

                if (string.IsNullOrEmpty(pdfService))
                {
                    throw new ArgumentException("Missing PdfService setting (WeasyPdfService)");
                }

                // append new filename
                pdfService = pdfService + "?filename=" + request.PdfFileName;                

                // call the microservice                
                HttpClient client = new HttpClient();
                StringContent stringContent = new StringContent(request.Html);
                HttpResponseMessage response = client.PostAsync(pdfService, stringContent).Result;

                // success
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var bytetask = response.Content.ReadAsByteArrayAsync();
                    bytetask.Wait();

                    return bytetask.Result;
                }

                throw new ApplicationException("PdfService Error (" + response.StatusCode + ")");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }            
        }
    }
}
