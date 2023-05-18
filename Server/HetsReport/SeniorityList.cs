using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HetsData.Helpers;
using HetsReport.Helpers;

namespace HetsReport
{
    public static class SeniorityList
    {
        private const string ResourceName = "HetsReport.Templates.SeniorityList-Template.docx";

        public static byte[] GetSeniorityList(SeniorityListReportViewModel reportModel, string name, bool counterCopy, Action<string, Exception> logErrorAction)
        {
            try
            {
                // ******************************************************
                // get document template
                // ******************************************************
                Assembly assembly = Assembly.GetExecutingAssembly();
                byte[] byteArray;
                int recordCount = 0;

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
                        foreach (SeniorityListRecord seniorityList in reportModel.SeniorityListRecords)
                        {
                            recordCount++;

                            using (MemoryStream listStream = new MemoryStream())
                            {
                                WordprocessingDocument listDocument = (WordprocessingDocument)wordTemplate.Clone(listStream);
                                listDocument.Save();

                                Dictionary<string, string> values = new Dictionary<string, string>
                                {
                                    {"classification", reportModel.Classification},
                                    {"generatedOn", reportModel.GeneratedOn},
                                    {"seniorityListType", reportModel.SeniorityListType },
                                    {"districtName", seniorityList.DistrictName},
                                    {"localAreaName", seniorityList.LocalAreaName},
                                    {"districtEquipmentTypeName", seniorityList.DistrictEquipmentTypeName}
                                };

                                // update main document
                                MergeHelper.ConvertFieldCodes(listDocument.MainDocumentPart.Document);
                                MergeHelper.MergeFieldsInElement(values, listDocument.MainDocumentPart.Document);
                                listDocument.MainDocumentPart.Document.Save();

                                // setup table for seniority list
                                Table seniorityTable = GenerateSeniorityTable(seniorityList.SeniorityList, seniorityList, counterCopy, logErrorAction);

                                // find our paragraph
                                Paragraph tableParagraph = null;
                                bool found = false;

                                foreach (OpenXmlElement paragraphs in listDocument.MainDocumentPart.Document.Body.Elements())
                                {
                                    foreach (OpenXmlElement paragraphRun in paragraphs.Elements())
                                    {
                                        foreach (OpenXmlElement text in paragraphRun.Elements())
                                        {
                                            if (text.InnerText.Contains("SeniorityListTable"))
                                            {
                                                // insert table here...
                                                text.RemoveAllChildren();
                                                tableParagraph = (Paragraph)paragraphRun.Parent;
                                                found = true;
                                                break;
                                            }
                                        }

                                        if (found) break;
                                    }

                                    if (found) break;
                                }

                                // append table to document
                                if (tableParagraph != null)
                                {
                                    Run run = tableParagraph.AppendChild(new Run());
                                    run.AppendChild(seniorityTable);
                                }

                                listDocument.MainDocumentPart.Document.Save();
                                listDocument.Save();

                                // merge owner into the master document
                                if (recordCount == 1)
                                {
                                    // update document header
                                    foreach (HeaderPart headerPart in listDocument.MainDocumentPart.HeaderParts)
                                    {
                                        MergeHelper.ConvertFieldCodes(headerPart.Header);
                                        MergeHelper.MergeFieldsInElement(values, headerPart.Header);
                                        headerPart.Header.Save();
                                    }

                                    wordDocument = (WordprocessingDocument)listDocument.Clone(documentStream);

                                    listDocument.Close();
                                    listDocument.Dispose();
                                }
                                else
                                {
                                    // DELETE document header from owner document
                                    listDocument.MainDocumentPart.DeleteParts(listDocument.MainDocumentPart.HeaderParts);

                                    List<HeaderReference> headers = listDocument.MainDocumentPart.Document.Descendants<HeaderReference>().ToList();

                                    foreach (HeaderReference header in headers)
                                    {
                                        header.Remove();
                                    }

                                    // DELETE document footers from owner document
                                    listDocument.MainDocumentPart.DeleteParts(listDocument.MainDocumentPart.FooterParts);

                                    List<FooterReference> footers = listDocument.MainDocumentPart.Document.Descendants<FooterReference>().ToList();

                                    foreach (FooterReference footer in footers)
                                    {
                                        footer.Remove();
                                    }

                                    // DELETE section properties from owner document
                                    List<SectionProperties> properties = listDocument.MainDocumentPart.Document.Descendants<SectionProperties>().ToList();

                                    foreach (SectionProperties property in properties)
                                    {
                                        property.Remove();
                                    }

                                    listDocument.Save();

                                    // insert section break in master
                                    MainDocumentPart mainPart = wordDocument.MainDocumentPart;

                                    Paragraph pageBreak = new Paragraph(new Run(new Break { Type = BreakValues.Page }));

                                    mainPart.Document.Body.InsertAfter(pageBreak, mainPart.Document.Body.LastChild);
                                    mainPart.Document.Save();

                                    // append document body
                                    string altChunkId = $"AltChunkId{recordCount}";

                                    AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.WordprocessingML, altChunkId);

                                    listDocument.Close();
                                    listDocument.Dispose();

                                    listStream.Seek(0, SeekOrigin.Begin);
                                    chunk.FeedData(listStream);

                                    AltChunk altChunk = new AltChunk { Id = altChunkId };

                                    Paragraph para3 = new Paragraph();
                                    Run run3 = para3.InsertAfter(new Run(), para3.LastChild);
                                    run3.AppendChild(altChunk);

                                    mainPart.Document.Body.InsertAfter(para3, mainPart.Document.Body.LastChild);
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
                    wordDocument.CompressionOption = CompressionOption.Maximum;
                    SecurityHelper.PasswordProtect(wordDocument, logErrorAction);

                    wordDocument.Close();
                    wordDocument.Dispose();

                    documentStream.Seek(0, SeekOrigin.Begin);
                    byteArray = documentStream.ToArray();
                }

                return byteArray;
            }
            catch (Exception e)
            {
                logErrorAction("GetSeniorityList exception: ", e);
                throw;
            }
        }

        private static Table GenerateSeniorityTable(
            IEnumerable<SeniorityViewModel> seniorityList, SeniorityListRecord seniorityRecord, bool counterCopy, Action<string, Exception> logErrorAction)
        {
            try
            {
                // create an empty table
                Table table = new Table();

                TableProperties tableProperties1 = new TableProperties();
                TableStyle tableStyle1 = new TableStyle() { Val = "TableGrid" };
                TableWidth tableWidth1 = new TableWidth() { Width = "0", Type = TableWidthUnitValues.Auto };

                TableLook tableLook1 = new TableLook()
                {
                    Val = "04A0",
                    FirstRow = true,
                    LastRow = false,
                    FirstColumn = true,
                    LastColumn = false,
                    NoHorizontalBand = false,
                    NoVerticalBand = true
                };

                tableProperties1.AppendChild(tableStyle1);
                tableProperties1.AppendChild(tableWidth1);
                tableProperties1.AppendChild(tableLook1);

                table.AppendChild(tableProperties1);

                // setup headers
                TableRow tableRow1 = new TableRow();

                TableRowProperties rowProperties = new TableRowProperties();

                rowProperties.AppendChild(new TableRowHeight() { Val = 200, HeightType = HeightRuleValues.AtLeast });
                rowProperties.AppendChild(new TableHeader() { Val = OnOffOnlyValues.On });

                tableRow1.AppendChild(rowProperties);

                // add columns
                tableRow1.AppendChild(SetupHeaderCell("Block", "800", logErrorAction, true));
                tableRow1.AppendChild(SetupHeaderCell("Equip ID", "1000", logErrorAction));

                if (!counterCopy)
                {
                    tableRow1.AppendChild(SetupHeaderCell("Working Now", "850", logErrorAction, true));
                    tableRow1.AppendChild(SetupHeaderCell("Last Called", "850", logErrorAction, true));
                }

                tableRow1.AppendChild(SetupHeaderCell("Company Name", "3000", logErrorAction));
                tableRow1.AppendChild(SetupHeaderCell("Year/Make/Model/Size", "3000", logErrorAction));
                tableRow1.AppendChild(SetupHeaderCell("YTD", "1000", logErrorAction, true));
                tableRow1.AppendChild(SetupHeaderCell(seniorityRecord.YearMinus1, "1000", logErrorAction, true));
                tableRow1.AppendChild(SetupHeaderCell(seniorityRecord.YearMinus2, "1000", logErrorAction, true));
                tableRow1.AppendChild(SetupHeaderCell(seniorityRecord.YearMinus3, "1000", logErrorAction, true));
                tableRow1.AppendChild(SetupHeaderCell("Yrs Reg", "1000", logErrorAction, true));
                tableRow1.AppendChild(SetupHeaderCell("Seniority", "1000", logErrorAction, true));

                table.AppendChild(tableRow1);

                string prevBlock = "1";

                // add rows for each record
                foreach (SeniorityViewModel seniority in seniorityList)
                {
                    //if block changes add an empty row first
                    if (!prevBlock.Equals(seniority.Block))
                    {
                        table.AppendChild(createTableRow(new SeniorityViewModel(), counterCopy, logErrorAction));
                        prevBlock = seniority.Block;
                    }

                    table.AppendChild(createTableRow(seniority, counterCopy, logErrorAction));
                }

                return table;
            }
            catch (Exception e)
            {
                logErrorAction("GenerateSeniorityTable exception: ", e);
                throw;
            }
        }

        private static TableRow createTableRow(SeniorityViewModel seniority, bool counterCopy, Action<string, Exception> logErrorAction)
        {
            //Tip: To create an empty row pass in "new SeniorityViewModel()" as an argument
            TableRow tableRowEquipment = new TableRow();

            TableRowProperties equipmentRowProperties = new TableRowProperties();
            equipmentRowProperties.AppendChild(new TableRowHeight() { Val = 200, HeightType = HeightRuleValues.AtLeast });
            tableRowEquipment.AppendChild(equipmentRowProperties);

            // add equipment data
            tableRowEquipment.AppendChild(SetupCell(seniority.Block, logErrorAction, true));
            tableRowEquipment.AppendChild(SetupCell(seniority.EquipmentCode, logErrorAction));

            if (!counterCopy)
            {
                tableRowEquipment.AppendChild(SetupCell(seniority.IsHired, logErrorAction, true));
                tableRowEquipment.AppendChild(SetupCell(seniority.LastCalled, logErrorAction, true));
            }

            tableRowEquipment.AppendChild(SetupCell(seniority.OwnerName, logErrorAction));
            tableRowEquipment.AppendChild(SetupCell(seniority.YearMakeModelSize, logErrorAction));
            tableRowEquipment.AppendChild(SetupCell(seniority.YtdHours, logErrorAction, true));
            tableRowEquipment.AppendChild(SetupCell(seniority.HoursYearMinus1, logErrorAction, true));
            tableRowEquipment.AppendChild(SetupCell(seniority.HoursYearMinus2, logErrorAction, true));
            tableRowEquipment.AppendChild(SetupCell(seniority.HoursYearMinus3, logErrorAction, true));
            tableRowEquipment.AppendChild(SetupCell(seniority.YearsRegistered, logErrorAction, true));
            tableRowEquipment.AppendChild(SetupCell(seniority.Seniority, logErrorAction, true));

            return tableRowEquipment;
        }

        private static TableCell SetupHeaderCell(string text, string width, Action<string, Exception> logErrorAction, bool center = false)
        {
            try
            {
                TableCell tableCell = new TableCell();

                TableCellProperties tableCellProperties = new TableCellProperties();
                TableCellWidth tableCellWidth = new TableCellWidth() { Width = width, Type = TableWidthUnitValues.Dxa };
                Shading shading = new Shading() { Val = ShadingPatternValues.Clear, Fill = "FFFFFF", Color = "auto" };

                // border & padding
                TableCellBorders borders = new TableCellBorders();

                TopBorder topBorder = new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Thick), Color = "000000" };
                borders.AppendChild(topBorder);

                BottomBorder bottomBorder = new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Thick), Color = "000000" };
                borders.AppendChild(bottomBorder);

                TableCellMargin margin = new TableCellMargin();
                TopMargin topMargin = new TopMargin() { Width = "40" };
                BottomMargin bottomMargin = new BottomMargin() { Width = "40" };
                margin.AppendChild(topMargin);
                margin.AppendChild(bottomMargin);

                tableCellProperties.AppendChild(tableCellWidth);
                tableCellProperties.AppendChild(shading);
                tableCellProperties.AppendChild(borders);
                tableCellProperties.AppendChild(margin);
                tableCellProperties.AppendChild(new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center });

                tableCell.AppendChild(tableCellProperties);

                // add text (with specific formatting)
                Paragraph paragraph = new Paragraph() { RsidParagraphAddition = "00607D74", RsidRunAdditionDefault = "00607D74", ParagraphId = "6ED85602", TextId = "77777777" };

                ParagraphProperties paragraphProperties = new ParagraphProperties();
                ParagraphMarkRunProperties paragraphMarkRunProperties = new ParagraphMarkRunProperties();

                paragraphMarkRunProperties.AppendChild(new Color { Val = "000000" });
                paragraphMarkRunProperties.AppendChild(new RunFonts { Ascii = "Arial" });
                paragraphMarkRunProperties.AppendChild(new FontSize() { Val = "8pt" });
                paragraphMarkRunProperties.AppendChild(new Bold());

                Justification justification = new Justification() { Val = JustificationValues.Left };
                if (center) justification.Val = JustificationValues.Center;

                paragraphProperties.AppendChild(paragraphMarkRunProperties);
                paragraphProperties.AppendChild(justification);
                paragraph.AppendChild(paragraphProperties);

                paragraph.AppendChild(new Text(text));

                // add to table cell
                tableCell.AppendChild(paragraph);

                return tableCell;
            }
            catch (Exception e)
            {
                logErrorAction("SetupHeaderCell exception: ", e);
                throw;
            }
        }

        private static TableCell SetupCell(string text, Action<string, Exception> logErrorAction, bool center = false)
        {
            try
            {
                TableCell tableCell = new TableCell();

                TableCellProperties tableCellProperties = new TableCellProperties();

                // border & padding
                TableCellBorders borders = new TableCellBorders();

                TopBorder topBorder = new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Thick), Color = "A1A2A3" };
                borders.AppendChild(topBorder);

                BottomBorder bottomBorder = new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Thick), Color = "A1A2A3" };
                borders.AppendChild(bottomBorder);

                TableCellMargin margin = new TableCellMargin();
                TopMargin topMargin = new TopMargin() { Width = "40" };
                BottomMargin bottomMargin = new BottomMargin() { Width = "40" };
                margin.AppendChild(topMargin);
                margin.AppendChild(bottomMargin);

                tableCellProperties.AppendChild(borders);
                tableCellProperties.AppendChild(margin);

                tableCell.AppendChild(tableCellProperties);

                // add text (with specific formatting)
                Paragraph paragraph = new Paragraph() { RsidParagraphAddition = "00607D74", RsidRunAdditionDefault = "00607D74", ParagraphId = "6ED85602", TextId = "77777777" };

                ParagraphProperties paragraphProperties = new ParagraphProperties();
                ParagraphMarkRunProperties paragraphMarkRunProperties = new ParagraphMarkRunProperties();

                paragraphMarkRunProperties.AppendChild(new Color { Val = "000000" });
                paragraphMarkRunProperties.AppendChild(new RunFonts { Ascii = "Arial" });
                paragraphMarkRunProperties.AppendChild(new FontSize() { Val = "8pt" });

                Justification justification = new Justification() { Val = JustificationValues.Left };
                if (center) justification.Val = JustificationValues.Center;

                paragraphProperties.AppendChild(paragraphMarkRunProperties);
                paragraphProperties.AppendChild(justification);
                paragraph.AppendChild(paragraphProperties);

                paragraph.AppendChild(new Text(text));

                // add to table cell
                tableCell.AppendChild(paragraph);

                return tableCell;
            }
            catch (Exception e)
            {
                logErrorAction("SetupCell exception: ", e);
                throw;
            }
        }
    }
}
