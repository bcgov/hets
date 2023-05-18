using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.IO.Packaging;
using HetsData.Entities;
using HetsReport.Helpers;

namespace HetsReport
{
    public static class MailingLabel
    {
        private const string ResourceName = "HetsReport.Templates.MailingLabel-Template.docx";
        private const double CentimeterToPoint = 28.3464566929134;

        public static byte[] GetMailingLabel(List<HetOwner> owners, Action<string, Exception> logErrorAction)
        {
            try
            {
                var byteArray = GetTemplate();

                using (var templateStream = new MemoryStream())
                {
                    templateStream.Write(byteArray, 0, byteArray.Length);

                    using (var labelDoc = WordprocessingDocument.Open(templateStream, true))
                    {
                        if (labelDoc == null)
                            throw new Exception("Mailing label template not found");

                        var tableParagraph = FindTableParagraph(labelDoc);
                        if (tableParagraph != null)
                        {
                            Run run = tableParagraph.AppendChild(new Run());
                            run.AppendChild(CreateMailingLabels(owners, logErrorAction));
                        }

                        labelDoc.Save();
                        labelDoc.CompressionOption = CompressionOption.Maximum;
                        SecurityHelper.PasswordProtect(labelDoc, logErrorAction);

                        byteArray = GetByteArray(labelDoc);
                    }
                }

                return byteArray;
            }
            catch (Exception e)
            {
                logErrorAction("GetMailingLabel exception: ", e);
                throw;
            }
        }

        private static byte[] GetByteArray(WordprocessingDocument labelDoc)
        {
            using (var documentStream = new MemoryStream())
            using (var wordDocument = (WordprocessingDocument)labelDoc.Clone(documentStream))
            {
                documentStream.Seek(0, SeekOrigin.Begin);
                return documentStream.ToArray();
            }
        }

        private static byte[] GetTemplate()
        {
            var assembly = Assembly.GetExecutingAssembly();
            byte[] byteArray;

            using (var templateStream = assembly.GetManifestResourceStream(ResourceName))
            {
                byteArray = new byte[templateStream.Length];
                templateStream.Read(byteArray, 0, byteArray.Length);
                templateStream.Close();
            }

            return byteArray;
        }

        private static Paragraph FindTableParagraph(WordprocessingDocument labelDoc)
        {
            foreach (var paragraphs in labelDoc.MainDocumentPart.Document.Body.Elements())
            {
                foreach (var paragraphRun in paragraphs.Elements())
                {
                    foreach (var text in paragraphRun.Elements())
                    {
                        if (text.InnerText.Contains("MailingLabelTable"))
                        {
                            text.RemoveAllChildren();
                            return (Paragraph)paragraphRun.Parent;
                        }
                    }
                }
            }

            return null;
        }

        private static Table CreateMailingLabels(List<HetOwner> owners, Action<string, Exception> logErrorAction)
        {

            try
            {
                var table = new Table();

                var tableProperties1 = new TableProperties();
                var tableStyle1 = new TableStyle { Val = "TableGrid" };
                var tableWidth1 = new TableWidth { Width = "100", Type = TableWidthUnitValues.Auto };

                var tableLook1 = new TableLook
                {
                    Val = "04A0",
                    FirstRow = true,
                    LastRow = false,
                    FirstColumn = true,
                    LastColumn = false,
                    NoHorizontalBand = true,
                    NoVerticalBand = true
                };

                tableProperties1.AppendChild(tableStyle1);
                tableProperties1.AppendChild(tableWidth1);
                tableProperties1.AppendChild(tableLook1);

                table.AppendChild(tableProperties1);

                foreach (var ownerTuple in owners.ToTuples())
                {
                    var tableRow = new TableRow();

                    var rowProps = new TableRowProperties();
                    rowProps.AppendChild(new TableRowHeight { Val = CentimeterToDxa(5.08), HeightType = HeightRuleValues.Exact }); 
                    tableRow.AppendChild(rowProps);

                    tableRow.AppendChild(SetupCell(ownerTuple.Item1, 10.16, 0.27, logErrorAction));
                    tableRow.AppendChild(SetupCell(ownerTuple.Item2, 10.16, 0.75, logErrorAction)); //to add 0.48 cm

                    table.AppendChild(tableRow);
                }

                return table;
            }
            catch (Exception e)
            {
                logErrorAction("CreateMailingLabels exception: ", e);
                throw;
            }
        }

        private static List<Tuple<HetOwner, HetOwner>> ToTuples(this List<HetOwner> owners)
        {
            var ownerTuples = new List<Tuple<HetOwner, HetOwner>>();

            for (int i = 0; i < owners.Count; i = i + 2)
            {
                var l = owners[i];
                var r = i + 1 < owners.Count ? owners[i + 1] : null;
                ownerTuples.Add(new Tuple<HetOwner, HetOwner>(l, r));
            }

            return ownerTuples;
        }


        private static TableCell SetupCell(HetOwner owner, double widthInCm, double start, Action<string, Exception> logErrorAction)
        {
            try
            {
                var tableCell = new TableCell();

                var tableCellProperties = new TableCellProperties();
                tableCellProperties.AppendChild(new TableCellWidth { Width = CentimeterToDxa(widthInCm).ToString(), Type = TableWidthUnitValues.Dxa });
                tableCellProperties.AppendChild(new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center });
                tableCell.AppendChild(tableCellProperties);

                var paragraphProperties = new ParagraphProperties();

                var paragraphMarkRunProperties = new ParagraphMarkRunProperties();
                paragraphMarkRunProperties.AppendChild(new Color { Val = "000000" });
                paragraphMarkRunProperties.AppendChild(new RunFonts { Ascii = "Arial" });
                paragraphMarkRunProperties.AppendChild(new FontSize() { Val = "13pt" });
                paragraphProperties.AppendChild(paragraphMarkRunProperties);

                paragraphProperties.AppendChild(new Justification { Val = JustificationValues.Left });
                paragraphProperties.AppendChild(new Indentation { Start = CentimeterToDxa(start).ToString() });

                var paragraph = new Paragraph();
                paragraph.AppendChild(paragraphProperties);

                if (owner != null)
                {
                    PopulateParagraph(owner, paragraph);
                }

                tableCell.AppendChild(paragraph);

                return tableCell;
            }
            catch (Exception e)
            {
                logErrorAction("SetupCell exception: ", e);
                throw;
            }
        }

        private static UInt32 CentimeterToDxa(double cm)
        {
            double dxa = CentimeterToPoint * cm * 20;
            return Convert.ToUInt32(dxa);
        }

        private static void PopulateParagraph(HetOwner owner, Paragraph paragraph)
        {
            paragraph.AppendChild(new Text(owner.OrganizationName?.Trim()));
            paragraph.AppendChild(new Break());
            paragraph.AppendChild(new Text(owner.Address1?.Trim()));
            paragraph.AppendChild(new Break());

            if (!string.IsNullOrWhiteSpace(owner.Address2))
            {
                paragraph.AppendChild(new Text(owner.Address2.Trim()));
                paragraph.AppendChild(new Text(", "));
            }

            paragraph.AppendChild(new Text($"{owner.City?.Trim()}, {owner.Province?.Trim()} {owner.PostalCode?.Trim()}"));
        }
    }

}
