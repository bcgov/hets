using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// City Controller
    /// </summary>
    public class CityController : Controller
    {
        private readonly ICityService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public CityController(ICityService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">City created</response>
        [HttpPost]
        [Route("/api/cities/bulk")]
        [SwaggerOperation("CitiesBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult CitiesBulkPost([FromBody]City[] items)
        {
            return this._service.CitiesBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/cities")]
        [SwaggerOperation("CitiesGet")]
        [SwaggerResponse(200, type: typeof(List<City>))]
        public virtual IActionResult CitiesGet()
        {
            return this._service.CitiesGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of City to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">City not found</response>
        [HttpPost]
        [Route("/api/cities/{id}/delete")]
        [SwaggerOperation("CitiesIdDeletePost")]
        public virtual IActionResult CitiesIdDeletePost([FromRoute]int id)
        {
            return this._service.CitiesIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of City to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">City not found</response>
        [HttpGet]
        [Route("/api/cities/{id}")]
        [SwaggerOperation("CitiesIdGet")]
        [SwaggerResponse(200, type: typeof(City))]
        public virtual IActionResult CitiesIdGet([FromRoute]int id)
        {
            return this._service.CitiesIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of City to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">City not found</response>
        [HttpPut]
        [Route("/api/cities/{id}")]
        [SwaggerOperation("CitiesIdPut")]
        [SwaggerResponse(200, type: typeof(City))]
        public virtual IActionResult CitiesIdPut([FromRoute]int id, [FromBody]City item)
        {
            return this._service.CitiesIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">City created</response>
        [HttpPost]
        [Route("/api/cities")]
        [SwaggerOperation("CitiesPost")]
        [SwaggerResponse(200, type: typeof(City))]
        public virtual IActionResult CitiesPost([FromBody]City item)
        {
            return this._service.CitiesPostAsync(item);
        }
    }
}
