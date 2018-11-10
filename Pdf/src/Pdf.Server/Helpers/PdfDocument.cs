using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using jsreport.Binary;
using jsreport.Local;
using jsreport.Types;

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
        public static async Task<byte[]> BuildPdf(PdfRequest request)
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

                // report server
                ILocalUtilityReportingService rs = new LocalReporting()
                    .UseBinary(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
                        JsReportBinary.GetBinary() :
                        jsreport.Binary.Linux.JsReportBinary.GetBinary())
                    .Configure(cfg => cfg.FileSystemStore().BaseUrlAsWorkingDirectory())
                    .AsUtility()
                    .Create();

                // call report js to generate pdf response
                Report report = await rs.RenderAsync(new RenderRequest()
                {
                    Template = new Template
                    {
                        Recipe = Recipe.ChromePdf,
                        Engine = Engine.None,
                        Content = request.Html
                    },
                    Options = new RenderOptions
                    {                                                
                        Timeout = 90000
                    }                
                });

                if (report == null) throw new ArgumentNullException(nameof(report));

                MemoryStream memoryStream = new MemoryStream();
                await report.Content.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                await rs.KillAsync();

                //return result;               
                return memoryStream.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }            
        }
    }
}
