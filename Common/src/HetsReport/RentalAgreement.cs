using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using HetsData.Helpers;
using HetsData.Model;
using HetsReport.Helpers;

namespace HetsReport
{
    public static class RentalAgreement

    {
        private const string ResourceName = "HetsReport.Templates.RentalAgreement-Template.docx";

        public static byte[] GetRentalAgreement(RentalAgreementDocViewModel reportModel, string name)
        {
            try
            {
                char[] characters = System.Text.Encoding.ASCII.GetChars(new byte[] { 10 });
                char crLf = characters[0];

                // ******************************************************
                // get document template
                // ******************************************************
                Assembly assembly = Assembly.GetExecutingAssembly();
                byte[] byteArray;

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
                            throw new Exception("Rental Agreement template not found");

                        // ******************************************************
                        // merge document content
                        // ******************************************************
                        using (MemoryStream agreementStream = new MemoryStream())
                        {
                            WordprocessingDocument agreementDocument = (WordprocessingDocument) wordTemplate.Clone(agreementStream);
                            agreementDocument.Save();

                            // create note
                            string note = "";
                            if (reportModel.Note != null)
                            {
                                foreach (NoteLine line in reportModel.Note)
                                {
                                    if (!string.IsNullOrEmpty(note))
                                    {
                                        note += $"{crLf.ToString()}{line.Line}";
                                    }
                                    else
                                    {
                                        note += $"{line.Line}";
                                    }
                                }
                            }

                            // equipment full name
                            string equipmentName = $"{reportModel.Equipment.Year}/" +
                                $"{reportModel.Equipment.Make}/" +
                                $"{reportModel.Equipment.Model}/" +
                                $"{reportModel.Equipment.Size}/" +
                                $"{reportModel.Equipment.SerialNumber}";

                            // rates included in total
                            string rateString1 = "";
                            string comment1 = "";
                            if (reportModel.RentalAgreementRatesWithTotal != null)
                            {
                                foreach (HetRentalAgreementRate rate in reportModel.RentalAgreementRatesWithTotal)
                                {
                                    if (!string.IsNullOrEmpty(rateString1))
                                    {
                                        rateString1 += $"{crLf.ToString()}{rate.RateString}";
                                        comment1 += $"{crLf.ToString()}{rate.Comment}";
                                    }
                                    else
                                    {
                                        rateString1 += $"{rate.RateString}";
                                        comment1 += $"{rate.Comment}";
                                    }
                                }
                            }

                            // rates not included in total
                            string rateString2 = "";
                            string comment2 = "";
                            if (reportModel.RentalAgreementRatesWithoutTotal != null)
                            {
                                foreach (HetRentalAgreementRate rate in reportModel.RentalAgreementRatesWithoutTotal)
                                {
                                    if (!string.IsNullOrEmpty(rateString1))
                                    {
                                        rateString2 += $"{crLf.ToString()}{rate.RateString}";
                                        comment2 += $"{crLf.ToString()}{rate.Comment}";
                                    }
                                    else
                                    {
                                        rateString2 += $"{rate.RateString}";
                                        comment2 += $"{rate.Comment}";
                                    }
                                }
                            }

                            // agreement conditions
                            string comment3 = "";
                            if (reportModel.RentalAgreementConditions != null)
                            {
                                foreach (HetRentalAgreementCondition cond in reportModel.RentalAgreementConditions)
                                {
                                    if (!string.IsNullOrEmpty(rateString1))
                                    {
                                        comment3 += $"{crLf.ToString()}{cond.Comment}";
                                    }
                                    else
                                    {
                                        comment3 += $"{cond.Comment}";
                                    }
                                }
                            }

                            // rates not included in total
                            string overtimeRate = "";
                            string overtimeComment = "";
                            if (reportModel.RentalAgreementRatesOvertime != null)
                            {
                                foreach (HetRentalAgreementRate rate in reportModel.RentalAgreementRatesOvertime.OrderByDescending(x => x.Comment))
                                {
                                    if (!string.IsNullOrEmpty(rateString1))
                                    {
                                        overtimeRate += $"{crLf.ToString()}{rate.RateString}";
                                        overtimeComment += $"{crLf.ToString()}{rate.Comment}";
                                    }
                                    else
                                    {
                                        overtimeRate += $"{rate.RateString}";
                                        overtimeComment += $"{rate.Comment}";
                                    }
                                }
                            }

                            Dictionary<string, string> values = new Dictionary<string, string>
                            {
                                {"equipmentCode", reportModel.Equipment.EquipmentCode},
                                {"number", reportModel.Number},
                                {"organizationName", reportModel.Equipment.Owner.OrganizationName},
                                {"address1", reportModel.Equipment.Owner.Address1},
                                {"address2", reportModel.Equipment.Owner.Address2},
                                {"ownerCode", reportModel.Equipment.Owner.OwnerCode},
                                {"workPhoneNumber", reportModel.Equipment.Owner.PrimaryContact.WorkPhoneNumber},
                                {"mobilePhoneNumber", reportModel.Equipment.Owner.PrimaryContact.MobilePhoneNumber},
                                {"faxPhoneNumber", reportModel.Equipment.Owner.PrimaryContact.FaxPhoneNumber},
                                {"equipmentFullName", equipmentName},
                                {"noteLine", note},
                                {"baseRateString", reportModel.BaseRateString},
                                {"rateComment", reportModel.RateComment},
                                {"projectNumber", reportModel.Project.ProvincialProjectNumber},
                                {"projectName", reportModel.Project.Name},
                                {"district", reportModel.Project.District.Name},
                                {"estimateHours", reportModel.EstimateHours.ToString()},
                                {"estimateStartWork", reportModel.EstimateStartWork},
                                {"localAreaName", reportModel.Equipment.LocalArea.Name},
                                {"workSafeBcpolicyNumber", reportModel.Equipment.Owner.WorkSafeBcpolicyNumber},
                                {"agreementCity", reportModel.AgreementCity},
                                {"datedOn", reportModel.DatedOn},
                                {"rateString1", rateString1},
                                {"comment1", comment1},
                                {"agreementTotalString", reportModel.AgreementTotalString},
                                {"rateString2", rateString2},
                                {"comment2", comment2},
                                {"comment3", comment3},
                                {"overtimeRate", overtimeRate},
                                {"overtimeComment", overtimeComment}
                            };

                            // update main document
                            MergeHelper.ConvertFieldCodes(agreementDocument.MainDocumentPart.Document);
                            MergeHelper.MergeFieldsInElement(values, agreementDocument.MainDocumentPart.Document);
                            agreementDocument.MainDocumentPart.Document.Save();

                            wordDocument = (WordprocessingDocument) agreementDocument.Clone(documentStream);

                            agreementDocument.Close();
                            agreementDocument.Dispose();
                        }

                        wordTemplate.Close();
                        wordTemplate.Dispose();
                        templateStream.Close();
                    }

                    // ******************************************************
                    // secure & return completed document
                    // ******************************************************
                    wordDocument.CompressionOption = CompressionOption.Maximum;
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
