using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.NodeServices;
using Newtonsoft.Json.Linq;

namespace Pdf.Server.Helpers
{
    public class HtmlRequest
    {
        public string JsonString { get; set; }
        public string Template { get; set; }
        public string RenderJsUrl { get; set; }
    }

    public class TemplateHelper
    {
        // Read Template
        // Pass data + template to mustache and generate output
        public static async Task<string> RenderDocument(INodeServices nodeServices, HtmlRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.JsonString))
                    throw new Exception("Missing Json data to generate document");

                // read template content
                Assembly assembly = Assembly.GetExecutingAssembly();
                string resourceName = "Pdf.Server.Templates." + request.Template;
                string templateContent;

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        templateContent = reader.ReadToEnd();
                    }
                }

                if (string.IsNullOrEmpty(templateContent))
                    throw new Exception("Mustache template not found");

                // create json object
                JObject json = JObject.Parse(request.JsonString);

                // call mustache js to generate html response
                string result = await nodeServices.InvokeAsync<string>(request.RenderJsUrl, templateContent, json);
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
