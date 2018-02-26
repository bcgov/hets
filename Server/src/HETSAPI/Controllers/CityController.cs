using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// City Controller
    /// </summary>    
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class CityController : Controller
    {
        private readonly ICityService _service;

        /// <summary>
        /// City Controller Constructor
        /// </summary>
        public CityController(ICityService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk city records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">City created</response>
        [HttpPost]
        [Route("/api/cities/bulk")]
        [SwaggerOperation("CitiesBulkPost")]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult CitiesBulkPost([FromBody]City[] items)
        {
            return _service.CitiesBulkPostAsync(items);
        }

        /// <summary>
        /// Get all cities
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/cities")]
        [SwaggerOperation("CitiesGet")]
        [SwaggerResponse(200, type: typeof(List<City>))]
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult CitiesGet()
        {
            return _service.CitiesGetAsync();
        }        
    }
}
