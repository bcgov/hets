using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{ 
    /// <summary>
    /// History Service
    /// </summary>
    public class HistoryService : IHistoryService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// History Service Constructor
        /// </summary>
        public HistoryService (DbAppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Create bulk history records
        /// </summary>        
        /// <param name="items"></param>
        /// <response code="201">Histories created</response>
        public virtual IActionResult HistoriesBulkPostAsync(History[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }
            foreach (History item in items)
            {
                _context.Historys.Add(item);
            }

            // Save the changes
            _context.SaveChanges();

            return new NoContentResult();
        }

        /// <summary>
        /// Get all history records
        /// </summary>        
        /// <response code="200">OK</response>
        public virtual IActionResult HistoriesGetAsync()        
        {
            var result = _context.Historys.ToList();
            return new ObjectResult(new HetsResponse(result));
        }
        /// <summary>
        /// Delete history
        /// </summary>        
        /// <param name="id">id of History to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">History not found</response>
        public virtual IActionResult HistoriesIdDeletePostAsync(int id)        
        {
            bool exists = _context.Historys.Any(a => a.Id == id);

            if (exists)
            {
                History item = _context.Historys.First(a => a.Id == id);

                _context.Historys.Remove(item);

                // Save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get history by id
        /// </summary>        
        /// <param name="id">id of History to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">History not found</response>
        public virtual IActionResult HistoriesIdGetAsync(int id)        
        {
            bool exists = _context.Historys.Any(a => a.Id == id);

            if (exists)
            {
                History result = _context.Historys.First(a => a.Id == id);
                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update history
        /// </summary>
        /// <param name="id">id of History to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">History not found</response>
        public virtual IActionResult HistoriesIdPutAsync(int id, History item)        
        {
            bool exists = _context.Historys.Any(a => a.Id == id);

            if (exists && id == item.Id)
            {
                _context.Historys.Update(item);    
                
                // Save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create history
        /// </summary>        
        /// <param name="item"></param>
        /// <response code="201">History created</response>
        public virtual IActionResult HistoriesPostAsync(History item)        
        {
            _context.Historys.Add(item);
            _context.SaveChanges();
            return new ObjectResult(new HetsResponse(item));
        }
    }
}
