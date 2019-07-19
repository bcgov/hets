using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.IO.Packaging;
using HetsData.Model;
using HetsReport.Helpers;

namespace HetsReport
{
    public static class MailingLabel
    {
        private const string ResourceName = "HetsReport.Templates.MailingLabel-Template.docx";
        public static byte[] GetMailingLabel(List<HetOwner> owners)
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
                            run.AppendChild(CreateMailingLabels(owners));
                        }

                        labelDoc.CompressionOption = CompressionOption.Maximum;
                        SecurityHelper.PasswordProtect(labelDoc);

                        byteArray = GetByteArray(labelDoc);
                    }
                }

                return byteArray;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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

        private static Table CreateMailingLabels(List<HetOwner> owners)
        {

            try
            {
                var ownerTuples = owners.ToTuples();

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

                foreach (var ownerTuple in ownerTuples)
                {
                    var tableRow = new TableRow();

                    var rowProps = new TableRowProperties();
                    rowProps.AppendChild(new TableRowHeight { Val = 3000, HeightType = HeightRuleValues.Exact });
                    tableRow.AppendChild(rowProps);

                    tableRow.AppendChild(SetupCell(ownerTuple.Item1));
                    tableRow.AppendChild(SetupCell(ownerTuple.Item2));

                    table.AppendChild(tableRow);
                }

                return table;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static List<Tuple<HetOwner, HetOwner>> ToTuples(this List<HetOwner> owners)
        {
            var ownerTuples = new List<Tuple<HetOwner, HetOwner>>();

            for (int i = 0; i < owners.Count; i++)
            {
                var l = owners[i];

                if (i + 1 < owners.Count)
                {
                    var r = owners[i + 1];

                    ownerTuples.Add(new Tuple<HetOwner, HetOwner>(l, r));
                }
                else
                {
                    ownerTuples.Add(new Tuple<HetOwner, HetOwner>(l, null));
                }
            }

            return ownerTuples;
        }


        private static TableCell SetupCell(HetOwner owner)
        {
            try
            {
                var tableCell = new TableCell();

                var tableCellProperties = new TableCellProperties();
                tableCellProperties.AppendChild(new TableCellWidth { Width = "50", Type = TableWidthUnitValues.Pct });
                tableCell.AppendChild(tableCellProperties);

                var margin = new TableCellMargin
                {
                    StartMargin = new StartMargin { Width = "1000" },
                    TopMargin = new TopMargin { Width = "1000" }
                };
                tableCell.AppendChild(margin);

                var paragraphProperties = new ParagraphProperties();

                var paragraphMarkRunProperties = new ParagraphMarkRunProperties();
                paragraphMarkRunProperties.AppendChild(new Color { Val = "000000" });
                paragraphMarkRunProperties.AppendChild(new RunFonts { Ascii = "Arial" });
                paragraphMarkRunProperties.AppendChild(new FontSize() { Val = "13pt" });
                paragraphProperties.AppendChild(paragraphMarkRunProperties);

                paragraphProperties.AppendChild(new Justification { Val = JustificationValues.Left });

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
                Console.WriteLine(e);
                throw;
            }
        }

        private static void PopulateParagraph(HetOwner owner, Paragraph paragraph)
        {
            paragraph.AppendChild(new Text(owner.OrganizationName.Trim()));
            paragraph.AppendChild(new Break());
            paragraph.AppendChild(new Text(owner.Address1.Trim()));
            paragraph.AppendChild(new Break());

            if (!string.IsNullOrWhiteSpace(owner.Address2))
            {
                paragraph.AppendChild(new Text(owner.Address2.Trim()));
                paragraph.AppendChild(new Text(", "));
            }

            paragraph.AppendChild(new Text($"{owner.City.Trim()}, {owner.Province.Trim()} {owner.PostalCode.Trim()}"));
        }
    }

}
