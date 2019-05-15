using DocumentFormat.OpenXml.Wordprocessing;

namespace HetsReport.Helpers
{
    public static class TableHelper
    {
        public static Table CreateTable()
        {
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

            TableGrid tableGrid1 = new TableGrid();
            GridColumn gridColumn1 = new GridColumn() { Width = "2394" };
            GridColumn gridColumn2 = new GridColumn() { Width = "2394" };
            GridColumn gridColumn3 = new GridColumn() { Width = "2394" };
            GridColumn gridColumn4 = new GridColumn() { Width = "2394" };

            tableGrid1.AppendChild(gridColumn1);
            tableGrid1.AppendChild(gridColumn2);
            tableGrid1.AppendChild(gridColumn3);
            tableGrid1.AppendChild(gridColumn4);

            TableRow tableRow1 = new TableRow() { RsidTableRowAddition = "00607D74", RsidTableRowProperties = "0055020F", ParagraphId = "2D079EFF", TextId = "77777777" };

            TableCell tableCell1 = new TableCell();

            TableCellProperties tableCellProperties1 = new TableCellProperties();
            TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "2394", Type = TableWidthUnitValues.Dxa };
            Shading shading1 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "1F497D", ThemeFill = ThemeColorValues.Text1 };

            tableCellProperties1.AppendChild(tableCellWidth1);
            tableCellProperties1.AppendChild(shading1);

            Paragraph paragraph1 = new Paragraph() { RsidParagraphAddition = "00607D74", RsidRunAdditionDefault = "00607D74", ParagraphId = "6ED85602", TextId = "77777777" };

            ParagraphProperties paragraphPropertiesA = new ParagraphProperties();
            ParagraphMarkRunProperties paragraphMarkRunPropertiesA = new ParagraphMarkRunProperties();
            Color color = new Color { Val = "FFFFFF" };
            RunFonts rFont = new RunFonts { Ascii = "Arial" };

            paragraphMarkRunPropertiesA.AppendChild(color);
            paragraphMarkRunPropertiesA.AppendChild(rFont);
            paragraphMarkRunPropertiesA.AppendChild(new FontSize() { Val = "8pt" });
            paragraphMarkRunPropertiesA.AppendChild(new Bold());

            paragraphPropertiesA.AppendChild(paragraphMarkRunPropertiesA);
            paragraph1.AppendChild(paragraphPropertiesA);

            paragraph1.AppendChild(new Text("Heading1"));

            tableCell1.AppendChild(tableCellProperties1);
            tableCell1.AppendChild(paragraph1);

            TableCell tableCell2 = new TableCell();

            TableCellProperties tableCellProperties2 = new TableCellProperties();
            TableCellWidth tableCellWidth2 = new TableCellWidth() { Width = "2394", Type = TableWidthUnitValues.Dxa };
            Shading shading2 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "1F497D", ThemeFill = ThemeColorValues.Text2 };

            tableCellProperties2.AppendChild(tableCellWidth2);
            tableCellProperties2.AppendChild(shading2);
            Paragraph paragraph2 = new Paragraph() { RsidParagraphAddition = "00607D74", RsidRunAdditionDefault = "00607D74", ParagraphId = "7C687A4B", TextId = "77777777" };

            tableCell2.AppendChild(tableCellProperties2);
            tableCell2.AppendChild(paragraph2);

            TableCell tableCell3 = new TableCell();

            TableCellProperties tableCellProperties3 = new TableCellProperties();
            TableCellWidth tableCellWidth3 = new TableCellWidth() { Width = "2394", Type = TableWidthUnitValues.Dxa };
            Shading shading3 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "1F497D", ThemeFill = ThemeColorValues.Text2 };

            tableCellProperties3.AppendChild(tableCellWidth3);
            tableCellProperties3.AppendChild(shading3);
            Paragraph paragraph3 = new Paragraph() { RsidParagraphAddition = "00607D74", RsidRunAdditionDefault = "00607D74", ParagraphId = "4F287AEB", TextId = "77777777" };

            tableCell3.AppendChild(tableCellProperties3);
            tableCell3.AppendChild(paragraph3);

            TableCell tableCell4 = new TableCell();

            TableCellProperties tableCellProperties4 = new TableCellProperties();
            TableCellWidth tableCellWidth4 = new TableCellWidth() { Width = "2394", Type = TableWidthUnitValues.Dxa };
            Shading shading4 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "1F497D", ThemeFill = ThemeColorValues.Text2 };

            tableCellProperties4.AppendChild(tableCellWidth4);
            tableCellProperties4.AppendChild(shading4);
            Paragraph paragraph4 = new Paragraph() { RsidParagraphAddition = "00607D74", RsidRunAdditionDefault = "00607D74", ParagraphId = "5F0063F5", TextId = "77777777" };

            tableCell4.AppendChild(tableCellProperties4);
            tableCell4.AppendChild(paragraph4);

            tableRow1.AppendChild(tableCell1);
            tableRow1.AppendChild(tableCell2);
            tableRow1.AppendChild(tableCell3);
            tableRow1.AppendChild(tableCell4);

            TableRow tableRow2 = new TableRow() { RsidTableRowAddition = "00607D74", RsidTableRowProperties = "00607D74", ParagraphId = "2558F9F0", TextId = "77777777" };

            TableCell tableCell5 = new TableCell();

            TableCellProperties tableCellProperties5 = new TableCellProperties();
            TableCellWidth tableCellWidth5 = new TableCellWidth() { Width = "2394", Type = TableWidthUnitValues.Dxa };

            tableCellProperties5.AppendChild(tableCellWidth5);

            Paragraph paragraph5 = new Paragraph() { RsidParagraphMarkRevision = "0055020F", RsidParagraphAddition = "00607D74", RsidRunAdditionDefault = "00607D74", ParagraphId = "186B732E", TextId = "77777777" };

            ParagraphProperties paragraphProperties1 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();
            Bold bold1 = new Bold();

            paragraphMarkRunProperties1.AppendChild(bold1);

            paragraphProperties1.AppendChild(paragraphMarkRunProperties1);

            paragraph5.AppendChild(paragraphProperties1);

            tableCell5.AppendChild(tableCellProperties5);
            tableCell5.AppendChild(paragraph5);

            TableCell tableCell6 = new TableCell();

            TableCellProperties tableCellProperties6 = new TableCellProperties();
            TableCellWidth tableCellWidth6 = new TableCellWidth() { Width = "2394", Type = TableWidthUnitValues.Dxa };

            tableCellProperties6.AppendChild(tableCellWidth6);
            Paragraph paragraph6 = new Paragraph() { RsidParagraphAddition = "00607D74", RsidRunAdditionDefault = "00607D74", ParagraphId = "6D2E7DAE", TextId = "77777777" };

            tableCell6.AppendChild(tableCellProperties6);
            tableCell6.AppendChild(paragraph6);

            TableCell tableCell7 = new TableCell();

            TableCellProperties tableCellProperties7 = new TableCellProperties();
            TableCellWidth tableCellWidth7 = new TableCellWidth() { Width = "2394", Type = TableWidthUnitValues.Dxa };

            tableCellProperties7.AppendChild(tableCellWidth7);
            Paragraph paragraph7 = new Paragraph() { RsidParagraphAddition = "00607D74", RsidRunAdditionDefault = "00607D74", ParagraphId = "2B98717A", TextId = "77777777" };

            tableCell7.AppendChild(tableCellProperties7);
            tableCell7.AppendChild(paragraph7);

            TableCell tableCell8 = new TableCell();

            TableCellProperties tableCellProperties8 = new TableCellProperties();
            TableCellWidth tableCellWidth8 = new TableCellWidth() { Width = "2394", Type = TableWidthUnitValues.Dxa };

            tableCellProperties8.AppendChild(tableCellWidth8);
            Paragraph paragraph8 = new Paragraph() { RsidParagraphAddition = "00607D74", RsidRunAdditionDefault = "00607D74", ParagraphId = "12461324", TextId = "77777777" };

            tableCell8.AppendChild(tableCellProperties8);
            tableCell8.AppendChild(paragraph8);

            tableRow2.AppendChild(tableCell5);
            tableRow2.AppendChild(tableCell6);
            tableRow2.AppendChild(tableCell7);
            tableRow2.AppendChild(tableCell8);

            TableRow tableRow3 = new TableRow() { RsidTableRowAddition = "00607D74", RsidTableRowProperties = "00607D74", ParagraphId = "56FE196D", TextId = "77777777" };

            TableCell tableCell9 = new TableCell();

            TableCellProperties tableCellProperties9 = new TableCellProperties();
            TableCellWidth tableCellWidth9 = new TableCellWidth() { Width = "2394", Type = TableWidthUnitValues.Dxa };

            tableCellProperties9.AppendChild(tableCellWidth9);

            Paragraph paragraph9 = new Paragraph() { RsidParagraphMarkRevision = "0055020F", RsidParagraphAddition = "00607D74", RsidRunAdditionDefault = "00607D74", ParagraphId = "617925F5", TextId = "77777777" };

            ParagraphProperties paragraphProperties2 = new ParagraphProperties();

            ParagraphMarkRunProperties paragraphMarkRunProperties2 = new ParagraphMarkRunProperties();
            Color color1 = new Color() { Val = "FF0000" };

            paragraphMarkRunProperties2.AppendChild(color1);

            paragraphProperties2.AppendChild(paragraphMarkRunProperties2);
            BookmarkStart bookmarkStart1 = new BookmarkStart() { Name = "_GoBack", Id = "0" };
            BookmarkEnd bookmarkEnd1 = new BookmarkEnd() { Id = "0" };

            paragraph9.AppendChild(paragraphProperties2);
            paragraph9.AppendChild(bookmarkStart1);
            paragraph9.AppendChild(bookmarkEnd1);

            tableCell9.AppendChild(tableCellProperties9);
            tableCell9.AppendChild(paragraph9);

            TableCell tableCell10 = new TableCell();

            TableCellProperties tableCellProperties10 = new TableCellProperties();
            TableCellWidth tableCellWidth10 = new TableCellWidth() { Width = "2394", Type = TableWidthUnitValues.Dxa };

            tableCellProperties10.AppendChild(tableCellWidth10);
            Paragraph paragraph10 = new Paragraph() { RsidParagraphAddition = "00607D74", RsidRunAdditionDefault = "00607D74", ParagraphId = "7FB20390", TextId = "77777777" };

            tableCell10.AppendChild(tableCellProperties10);
            tableCell10.AppendChild(paragraph10);

            TableCell tableCell11 = new TableCell();

            TableCellProperties tableCellProperties11 = new TableCellProperties();
            TableCellWidth tableCellWidth11 = new TableCellWidth() { Width = "2394", Type = TableWidthUnitValues.Dxa };

            tableCellProperties11.AppendChild(tableCellWidth11);
            Paragraph paragraph11 = new Paragraph() { RsidParagraphAddition = "00607D74", RsidRunAdditionDefault = "00607D74", ParagraphId = "6B3FAA2A", TextId = "77777777" };

            tableCell11.AppendChild(tableCellProperties11);
            tableCell11.AppendChild(paragraph11);

            TableCell tableCell12 = new TableCell();

            TableCellProperties tableCellProperties12 = new TableCellProperties();
            TableCellWidth tableCellWidth12 = new TableCellWidth() { Width = "2394", Type = TableWidthUnitValues.Dxa };

            tableCellProperties12.AppendChild(tableCellWidth12);
            Paragraph paragraph12 = new Paragraph() { RsidParagraphAddition = "00607D74", RsidRunAdditionDefault = "00607D74", ParagraphId = "046FFB08", TextId = "77777777" };

            tableCell12.AppendChild(tableCellProperties12);
            tableCell12.AppendChild(paragraph12);

            tableRow3.AppendChild(tableCell9);
            tableRow3.AppendChild(tableCell10);
            tableRow3.AppendChild(tableCell11);
            tableRow3.AppendChild(tableCell12);

            table.AppendChild(tableProperties1);
            table.AppendChild(tableGrid1);
            table.AppendChild(tableRow1);
            table.AppendChild(tableRow2);
            table.AppendChild(tableRow3);

            return table;
        }
    }
}
