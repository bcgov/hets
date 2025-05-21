using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace HetsReport.Helpers
{
    public static class MergeHelper
    {
        private static readonly Regex InstructionRegEx =
            new Regex(
                @"^[\s]*MERGEFIELD[\s]+(?<name>[#\w]*){1}                      # This retrieves the field's name (Named Capture Group -> name)
                            [\s]*(\\\*[\s]+(?<Format>[\w]*){1})?               # Retrieves field's format flag (Named Capture Group -> Format)
                            [\s]*(\\b[\s]+[""]?(?<PreText>[^\\]*){1})?         # Retrieves text to display before field data (Named Capture Group -> PreText)
                                                                               # Retrieves text to display after field data (Named Capture Group -> PostText)
                            [\s]*(\\f[\s]+[""]?(?<PostText>[^\\]*){1})?",
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);

        public static void MergeFieldsInElement(Dictionary<string, string> values, OpenXmlElement element)
        {
            Dictionary<SimpleField, string[]> emptyFields = new Dictionary<SimpleField, string[]>();

            // first pass: fill in data but do not delete empty fields
            SimpleField[] list = element.Descendants<SimpleField>().ToArray();

            foreach (SimpleField field in list)
            {
                string fieldName = GetFieldNameWithOptions(field, out var switches, out var options);

                if (!string.IsNullOrEmpty(fieldName))
                {
                    if (values.ContainsKey(fieldName) && !string.IsNullOrEmpty(values[fieldName]))
                    {
                        var formattedText = ApplyFormatting(options[0], values[fieldName], options[1], options[2]);

                        // prepend any text specified to appear before the data in the MergeField
                        if (!string.IsNullOrEmpty(options[1]))
                        {
                            field.Parent.InsertBeforeSelf(GetPreOrPostParagraphToInsert(formattedText[1], field));
                        }

                        // append any text specified to appear after the data in the MergeField
                        if (!string.IsNullOrEmpty(options[2]))
                        {
                            field.Parent.InsertAfterSelf(GetPreOrPostParagraphToInsert(formattedText[2], field));
                        }

                        // replace MergeField with text
                        Run newFieldRun = GetRunElementForText(formattedText[0], field);
                        field.Parent.AppendChild(newFieldRun);

                        int index = 0;

                        foreach (OpenXmlElement item in field.Parent.ChildElements)
                        {
                            if (item.Equals(field)) break;
                            index++;
                        }

                        field.Parent.ChildElements[index].Remove();
                    }
                    else
                    {
                        // keep track of unknown or empty fields
                        emptyFields[field] = switches;
                    }
                }
            }

            // second pass : clear empty fields
            foreach (KeyValuePair<SimpleField, string[]> kvp in emptyFields)
            {
                // if field is unknown or empty: execute switches and remove it from document
                ExecuteSwitches(kvp.Key, kvp.Value);
                kvp.Key.Remove();
            }
        }

        private static void ExecuteSwitches(OpenXmlElement element, string[] switches)
        {
            if (switches == null || !switches.Any())
            {
                return;
            }

            // check switches (switches are always lowercase)
            if (switches.Contains("dp"))
            {
                Paragraph p = GetFirstParent<Paragraph>(element);
                p?.Remove();
            }
            else if (switches.Contains("dr"))
            {
                TableRow row = GetFirstParent<TableRow>(element);
                row?.Remove();
            }
            else if (switches.Contains("dt"))
            {
                Table table = GetFirstParent<Table>(element);
                table?.Remove();
            }
        }

        private static T GetFirstParent<T>(OpenXmlElement element) where T : OpenXmlElement
        {
            if (element.Parent == null)
            {
                return null;
            }

            if (element.Parent.GetType() == typeof(T))
            {
                return element.Parent as T;
            }

            return GetFirstParent<T>(element.Parent);
        }

        private static Paragraph GetPreOrPostParagraphToInsert(string text, SimpleField fieldToMimic)
        {
            Run runToInsert = GetRunElementForText(text, fieldToMimic);
            Paragraph paragraphToInsert = new Paragraph();
            if (runToInsert != null) paragraphToInsert.AppendChild(runToInsert);

            return paragraphToInsert;
        }

        private static Run GetRunElementForText(string text, SimpleField placeHolder)
        {
            string rpr = null;

            if (placeHolder != null)
            {
                foreach (RunProperties placeholderRpr in placeHolder.Descendants<RunProperties>())
                {
                    rpr = placeholderRpr.OuterXml;
                    break;
                }
            }

            Run r = new Run();

            if (!string.IsNullOrEmpty(rpr))
            {
                r.AppendChild(new RunProperties(rpr));
            }

            if (!string.IsNullOrEmpty(text))
            {
                // first process line breaks
                string[] split = text.Split(new[] { "\n" }, StringSplitOptions.None);
                bool first = true;

                foreach (string s in split)
                {
                    if (!first)
                    {
                        r.AppendChild(new Break());
                    }

                    first = false;

                    // then process tabs
                    bool firstTab = true;
                    string[] tabSplit = s.Split(new[] { "\t" }, StringSplitOptions.None);

                    foreach (string tabText in tabSplit)
                    {
                        if (!firstTab)
                        {
                            r.AppendChild(new TabChar());
                        }

                        r.AppendChild(new Text(tabText));
                        firstTab = false;
                    }
                }
            }

            return r;
        }

        private static string GetFieldNameWithOptions(SimpleField field, out string[] switches, out string[] options)
        {
            OpenXmlAttribute a = field.GetAttribute("instr", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            switches = new string[0];
            options = new string[3];
            string fieldName = string.Empty;
            string instruction = a.Value;

            if (!string.IsNullOrEmpty(instruction))
            {
                Match m = InstructionRegEx.Match(instruction);

                if (m.Success)
                {
                    fieldName = m.Groups["name"].ToString().Trim();
                    options[0] = m.Groups["Format"].Value.Trim();
                    options[1] = m.Groups["PreText"].Value.Trim();
                    options[2] = m.Groups["PostText"].Value.Trim();

                    int pos = fieldName.IndexOf('#');

                    if (pos > 0)
                    {
                        // process the switches, correct the fieldName
                        switches = fieldName.Substring(pos + 1).ToLower().Split(new[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                        fieldName = fieldName.Substring(0, pos);
                    }
                }
            }

            return fieldName;
        }

        private static string[] ApplyFormatting(string format, string fieldValue, string preText, string postText)
        {
            string[] valuesToReturn = new string[3];

            switch (format)
            {
                case "UPPER":
                    // convert everything to uppercase
                    valuesToReturn[0] = fieldValue.ToUpper(CultureInfo.CurrentCulture);
                    valuesToReturn[1] = preText.ToUpper(CultureInfo.CurrentCulture);
                    valuesToReturn[2] = postText.ToUpper(CultureInfo.CurrentCulture);
                    break;

                case "LOWER":
                    // convert everything to lowercase
                    valuesToReturn[0] = fieldValue.ToLower(CultureInfo.CurrentCulture);
                    valuesToReturn[1] = preText.ToLower(CultureInfo.CurrentCulture);
                    valuesToReturn[2] = postText.ToLower(CultureInfo.CurrentCulture);
                    break;

                case "FirstCap":
                    {
                        // capitalize the first letter, everything else is lowercase.
                        if (!string.IsNullOrEmpty(fieldValue))
                        {
                            valuesToReturn[0] = fieldValue.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture);
                            if (fieldValue.Length > 1)
                            {
                                valuesToReturn[0] = valuesToReturn[0] + fieldValue.Substring(1).ToLower(CultureInfo.CurrentCulture);
                            }
                        }

                        if (!string.IsNullOrEmpty(preText))
                        {
                            valuesToReturn[1] = preText.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture);
                            if (fieldValue != null && fieldValue.Length > 1)
                            {
                                valuesToReturn[1] = valuesToReturn[1] + preText.Substring(1).ToLower(CultureInfo.CurrentCulture);
                            }
                        }

                        if (!string.IsNullOrEmpty(postText))
                        {
                            valuesToReturn[2] = postText.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture);
                            if (fieldValue != null && fieldValue.Length > 1)
                            {
                                valuesToReturn[2] = valuesToReturn[2] + postText.Substring(1).ToLower(CultureInfo.CurrentCulture);
                            }
                        }

                        break;
                    }

                case "Caps":
                    // title casing: the first letter of every word should be capitalized.
                    valuesToReturn[0] = ToTitleCase(fieldValue);
                    valuesToReturn[1] = ToTitleCase(preText);
                    valuesToReturn[2] = ToTitleCase(postText);
                    break;

                default:
                    valuesToReturn[0] = fieldValue;
                    valuesToReturn[1] = preText;
                    valuesToReturn[2] = postText;
                    break;
            }

            return valuesToReturn;
        }

        private static string ToTitleCase(string toConvert)
        {
            return ToTitleCaseHelper(toConvert, string.Empty);
        }

        private static string ToTitleCaseHelper(string toConvert, string alreadyConverted)
        {
            if (string.IsNullOrEmpty(toConvert))
            {
                return alreadyConverted;
            }

            int indexOfFirstSpace = toConvert.IndexOf(' ');
            string firstWord, restOfString;

            // check to see if we're on the last word or if there are more
            if (indexOfFirstSpace != -1)
            {
                firstWord = toConvert.Substring(0, indexOfFirstSpace);
                restOfString = toConvert.Substring(indexOfFirstSpace).Trim();
            }
            else
            {
                firstWord = toConvert.Substring(0);
                restOfString = string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(alreadyConverted);
            sb.Append(" ");
            sb.Append(firstWord.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture));

            if (firstWord.Length > 1)
            {
                sb.Append(firstWord.Substring(1).ToLower(CultureInfo.CurrentCulture));
            }

            return ToTitleCaseHelper(restOfString, sb.ToString());
        }

        public static void ConvertFieldCodes(OpenXmlElement mainElement)
        {
            //  search for all the Run elements
            Run[] runs = mainElement.Descendants<Run>().ToArray();
            if (runs.Length == 0) return;

            Dictionary<Run, Run[]> newFields = new Dictionary<Run, Run[]>();

            int cursor = 0;

            do
            {
                Run run = runs[cursor];

                if (run.HasChildren &&
                    run.Descendants<FieldChar>().Any() &&
                    run.Descendants<FieldChar>().FirstOrDefault()?.FieldCharType?.Value == FieldCharValues.Begin)
                {
                    List<Run> innerRuns = new List<Run> { run };

                    //  loop until we find the 'end' FieldChar
                    bool found = false;
                    string instruction = null;
                    RunProperties runProp = null;

                    do
                    {
                        cursor++;
                        run = runs[cursor];

                        innerRuns.Add(run);

                        if (run.HasChildren && run.Descendants<FieldCode>().Any())
                            instruction += run.GetFirstChild<FieldCode>().Text;

                        if (run.HasChildren &&
                            run.Descendants<FieldChar>().Any() &&
                            run.Descendants<FieldChar>().FirstOrDefault()?.FieldCharType?.Value == FieldCharValues.End)
                        {
                            found = true;
                        }

                        if (run.HasChildren && run.Descendants<RunProperties>().Any())
                            runProp = run.GetFirstChild<RunProperties>();

                    } while (found == false && cursor < runs.Length);

                    //  something went wrong : found Begin but no End. Throw exception
                    if (!found) throw new Exception("Found a Begin FieldChar but no End !");

                    if (!string.IsNullOrEmpty(instruction))
                    {
                        //  build new Run containing a SimpleField
                        Run newRun = new Run();

                        if (runProp != null) newRun.AppendChild(runProp.CloneNode(true));

                        SimpleField simpleField = new SimpleField { Instruction = instruction };

                        newRun.AppendChild(simpleField);

                        newFields.Add(newRun, innerRuns.ToArray());
                    }
                }

                cursor++;

            } while (cursor < runs.Length);

            //  replace all FieldCodes by old-style SimpleFields
            foreach (KeyValuePair<Run, Run[]> kvp in newFields)
            {
                kvp.Value[0].Parent.ReplaceChild(kvp.Key, kvp.Value[0]);

                for (int i = 1; i < kvp.Value.Length; i++)
                    kvp.Value[i].Remove();
            }
        }
    }
}
