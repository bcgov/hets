using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HetsData.Helpers;
using HetsData.Model;
using HetsReport.Helpers;

namespace HetsReport
{
    public static class OwnerVerification
    {
        private const string ResourceName = "HetsReport.Templates.OwnerVerification-Template.docx";

        public static byte[] GetOwnerVerification(OwnerVerificationReportModel reportModel, string name)
        {
            try
            {
                // ******************************************************
                // get document template
                // ******************************************************
                Assembly assembly = Assembly.GetExecutingAssembly();
                byte[] byteArray;
                int ownerCount = 0;

                using (Stream templateStream = assembly.GetManifestResourceStream(ResourceName))
                {
                    byteArray = new byte[templateStream.Length];
                    templateStream.Read(byteArray, 0, byteArray.Length);
                    templateStream.Close();
                }

                using (MemoryStream documentStream = new MemoryStream())
                {
                    WordprocessingDocument wordDocument = WordprocessingDocument.Create(documentStream, WordprocessingDocumentType.Document, true);

                    // add a main document part
                    wordDocument.AddMainDocumentPart();

                    using (MemoryStream templateStream = new MemoryStream())
                    {
                        templateStream.Write(byteArray, 0, byteArray.Length);
                        WordprocessingDocument wordTemplate = WordprocessingDocument.Open(templateStream, true);

                        if (wordTemplate == null)
                            throw new Exception("Owner Verification template not found");

                        // ******************************************************
                        // merge document content
                        // ******************************************************
                        foreach (HetOwner owner in reportModel.Owners)
                        {
                            ownerCount++;

                            using (MemoryStream ownerStream = new MemoryStream())
                            {
                                WordprocessingDocument ownerDocument = (WordprocessingDocument)wordTemplate.Clone(ownerStream);
                                ownerDocument.Save();

                                Dictionary<string, string> values = new Dictionary<string, string>
                                {
                                    { "districtAddress", reportModel.DistrictAddress },
                                    { "districtContact", reportModel.DistrictContact },
                                    { "organizationName", owner.OrganizationName },
                                    { "address1", owner.Address1 },
                                    { "address2", owner.Address2 },
                                    { "reportDate", reportModel.ReportDate },
                                    { "ownerCode", owner.OwnerCode },
                                    { "sharedKeyHeader", owner.SharedKeyHeader },
                                    { "sharedKey", owner.SharedKey },
                                    { "workPhoneNumber", owner.PrimaryContactPhone },
                                    { "mobilePhoneNumber", "" },
                                    { "faxPhoneNumber", "" }
                                };

                                // update main document
                                MergeHelper.ConvertFieldCodes(ownerDocument.MainDocumentPart.Document);
                                MergeHelper.MergeFieldsInElement(values, ownerDocument.MainDocumentPart.Document);
                                ownerDocument.MainDocumentPart.Document.Save();

                                if (ownerCount == 1)
                                {
                                    // update document header
                                    foreach (HeaderPart headerPart in ownerDocument.MainDocumentPart.HeaderParts)
                                    {
                                        MergeHelper.ConvertFieldCodes(headerPart.Header);
                                        MergeHelper.MergeFieldsInElement(values, headerPart.Header);
                                        headerPart.Header.Save();
                                    }

                                    wordDocument = (WordprocessingDocument) ownerDocument.Clone(documentStream);

                                    ownerDocument.Close();
                                    ownerDocument.Dispose();
                                }
                                else
                                {
                                    // insert page break
                                    MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                                    Paragraph para = new Paragraph(new Run((new Break() {Type = BreakValues.Page})));
                                    mainPart.Document.Body.InsertAfter(para, mainPart.Document.Body.LastChild);

                                    // append document body
                                    string altChunkId = $"AltChunkId{ownerCount}";

                                    AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.WordprocessingML, altChunkId);

                                    ownerDocument.Close();
                                    ownerDocument.Dispose();

                                    ownerStream.Seek(0, SeekOrigin.Begin);
                                    chunk.FeedData(ownerStream);

                                    AltChunk altChunk = new AltChunk {Id = altChunkId };
                                    mainPart.Document.Body.InsertAfter(altChunk, mainPart.Document.Body.Elements<Paragraph>().Last());
                                    mainPart.Document.Save();
                                }
                            }
                        }

                        wordTemplate.Close();
                        wordTemplate.Dispose();
                        templateStream.Close();
                    }

                    // ******************************************************
                    // secure & return completed document
                    // ******************************************************
                    SecurityHelper.PasswordProtect(wordDocument);

                    wordDocument.Close();
                    wordDocument.Dispose();

                    documentStream.Seek(0, SeekOrigin.Begin);
                    byteArray = documentStream.ToArray();
                }

                return byteArray;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
