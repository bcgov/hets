using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using HetsApi.Model;

namespace HetsApi.Helpers
{
    /// <summary>
    /// Upload Helper - Upload files for data conversion (from legacy system @ BC Bid)
    /// </summary>
    public static class UploadHelper
    {
        /// <summary>
        /// Upload Files
        /// </summary>
        /// <param name="files"></param>
        /// <param name="uploadPath"></param>
        /// <returns></returns>
        public static HomeModel UploadFiles(IList<IFormFile> files, string uploadPath)
        {
            HomeModel home = new HomeModel();
            StringBuilder result = new StringBuilder();

            if (string.IsNullOrEmpty(uploadPath))
            {
                home.FileImportResult = "ERROR";
                home.FileImportMessage = "UploadPath environment variable is empty.  Set it to the path where files will be stored.";
            }
            else
            {
                try
                {
                    home.FileImportResult = "OK";
                    home.FileImportMessage = string.Format("Total Files Received: {0}", files.Count);
                    result.Append("<ul>");

                    foreach (IFormFile file in files)
                    {
                        bool importFile = true;

                        // ************************************************************
                        // check file size
                        // ************************************************************
                        long fileSize = file.Length;
                        if (fileSize <= 0)
                        {
                            result.Append("<li>" + file.FileName + " (file size = 0 KB) - Not Uploaded</li>");
                            importFile = false;
                        }

                        // ************************************************************
                        // check the file extension is ".zip"
                        // ************************************************************
                        string extension = "";
                        int extStart = file.FileName.LastIndexOf('.');
                        if (extStart > 0)
                        {
                            extension = file.FileName.Substring(extStart + 1).ToLower();                            
                        }

                        if (string.IsNullOrEmpty(extension) || extension != "zip")
                        {
                            result.Append("<li>" + file.FileName + " (must be a .zip file) - File Not Uploaded</li>");
                            importFile = false;
                        }

                        // ************************************************************
                        // import the file...
                        // ************************************************************
                        if (importFile)
                        {
                            // Add a unique file prefix to allow for a file to be uploaded multiple times.
                            string filePrefix = "" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" +
                                                DateTime.Now.Day + "-" + DateTime.Now.Ticks + "-";

                            // combine for new file name
                            string fullName = filePrefix + "hets.zip";

                            using (FileStream fileStream = new FileStream(Path.Combine(uploadPath, fullName), FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                                

                                result.Append("<li>" + file.FileName + " (file Size = " + fileSize + ")</li>");
                            }
                        }
                    }

                    result.Append("</ul>");
                    home.FileNames = result.ToString();
                }
                catch (Exception e)
                {
                    home.FileImportResult = "ERROR";
                    home.FileImportMessage = e.Message;
                }
            }

            return home;
        }
    }
}