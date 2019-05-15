using System;
using System.IO;
using System.Reflection;
using DocumentFormat.OpenXml.Packaging;
using HetsData.Helpers;
using HetsReport.Helpers;

namespace HetsReport
{
    public static class OwnerVerification
    {
        private const string ResourceName = "HetsReport.Templates.OwnerVerification.docx";

        public static byte[] GetOwnerVerification(OwnerVerificationReportModel reportModel, string name)
        {
            try
            {
                // ******************************************************
                // get document template
                // ******************************************************
                Assembly assembly = Assembly.GetExecutingAssembly();
                string template;

                using (Stream stream = assembly.GetManifestResourceStream(ResourceName))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        template = reader.ReadToEnd();
                    }
                }

                if (string.IsNullOrEmpty(template))
                    throw new Exception("Owner Verification template not found");

                WordprocessingDocument wordDocument = WordprocessingDocument.Open(template, true);

                // ******************************************************
                // merge document content
                // ******************************************************



                // ******************************************************
                // secure & return completed document
                // ******************************************************
                wordDocument = SecurityHelper.PasswordProtect(wordDocument);

                using (MemoryStream documentStream = new MemoryStream())
                {
                    wordDocument.Clone(documentStream);
                    byte[] docBytes = documentStream.ToArray();
                    return docBytes;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
