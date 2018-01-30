using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;
using Hangfire;
using HETSAPI.Import;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Admin Service
    /// </summary>
    public class AdminService : ServiceBase, IAdminService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly Object _thisLock = new Object();

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public AdminService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, DbAppContext context) : base(httpContextAccessor, context)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult AdminImportGetAsync(string path, string districts)
        {
            string result = "Created Job: ";

            lock (_thisLock)
            {
                string uploadPath = _configuration["UploadPath"];
                string connectionString = _context.Database.GetDbConnection().ConnectionString;

                if (districts != null && districts == "388888")
                {
                    // not using Hangfire
                    BCBidImport.ImportJob(null, connectionString, uploadPath + path);
                }
                else
                {
                    // use Hangfire
                    string jobId = BackgroundJob.Enqueue(() => BCBidImport.ImportJob(null, connectionString, uploadPath + path));
                    result += jobId;
                }
            }

            return new ObjectResult(result);
        }


        public async Task<IActionResult> AdminUserMap(string path)
        {
            // create an excel spreadsheet that will show the data.

            string sWebRootFolder = "/tmp";
            string sFileName = @"usermap.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("User Map");
                // Create the header row.

                IRow row = excelSheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("Table Name");
                row.CreateCell(1).SetCellValue("Mapped Column");
                row.CreateCell(2).SetCellValue("Original Value");
                row.CreateCell(3).SetCellValue("New Value");

                // use the import class to get data.

                List<ImportMapRecord> records = ImportUser.GetImportMap(_context, path);
                int currentRow = 1;  
                
                // convert the list to an excel spreadsheet.
                foreach (ImportMapRecord record in records)
                {
                    IRow newRow = excelSheet.CreateRow(currentRow);
                    newRow.CreateCell(0).SetCellValue(record.TableName);
                    newRow.CreateCell(1).SetCellValue(record.MappedColumn);
                    newRow.CreateCell(2).SetCellValue(record.OriginalValue);
                    newRow.CreateCell(3).SetCellValue(record.NewValue);

                    currentRow++;
                }


                 workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            var fileStreamResult = new FileStreamResult(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fileStreamResult.FileDownloadName = "UserMap.xlsx";

            return fileStreamResult;
        }
    }
}
