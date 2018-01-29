using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HETSAPI.ViewModels;
using Microsoft.AspNetCore.Http;

namespace HETSAPI.Helpers
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
        public static HomeViewModel UploadFiles(IList<IFormFile> files, string uploadPath)
        {
            HomeViewModel home = new HomeViewModel();
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
                    home.FileImportMessage = string.Format("Files Received: (Total Files: {0})", files.Count);
                    result.Append("<ul>");

                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            // Add a unique file prefix to allow for a file to be uploaded multiple times.
                            string filePrefix = "" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" +
                                                DateTime.Now.Day + "-" + DateTime.Now.Ticks + "-";

                            using (FileStream fileStream = new FileStream(Path.Combine(uploadPath, filePrefix + file.FileName), FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                                result.Append("<li>" + file.FileName + "</li>");
                            }
                        }
                        else
                        {
                            result.Append("<li>" + file.FileName + " (file size = 0 KB) - Not Uploaded</li>");
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
