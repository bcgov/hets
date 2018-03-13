using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// User District Controller
    /// </summary>    
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class UserDistrictController : Controller
    {
        private readonly IUserDistrictService _service;

        /// <summary>
        /// User District Controller Constructor
        /// </summary>
        public UserDistrictController(IUserDistrictService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk user district type records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="200">User Districts created</response>
        [HttpPost]
        [Route("/api/userdistricts/bulk")]
        [SwaggerOperation("UserDistrictsBulkPost")]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult UserDistrictsBulkPost([FromBody]UserDistrict[] items)
        {
            return _service.UserDistrictsBulkPostAsync(items);
        }

        /// <summary>
        /// Get all user districts for the logged on user
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/userdistricts")]
        [SwaggerOperation("UserDistrictsGet")]        
        [SwaggerResponse(200, type: typeof(List<UserDistrict>))]
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult UserDistrictsGet()
        {
            return _service.UserDistrictsGetAsync();
        }
        
        /// <summary>	
        /// Delete user district	
        /// </summary>	
        /// <param name="id">id of User District to delete</param>	
        /// <response code="200">OK</response>	
        [HttpPost]
        [Route("/api/userdistricts/{id}/delete")]
        [SwaggerOperation("UserDistrictsIdDeletePost")]
        [RequiresPermission(Permission.UserManagement)]
        public virtual IActionResult UserDistrictsIdDeletePost([FromRoute]int id)
        {
            return _service.UserDistrictsIdDeletePostAsync(id);
        }

        /// <summary>
        /// Create or update a User District
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <response code="200">User District created or updated</response>
        [HttpPost]
        [Route("/api/userdistricts/{id}")]
        [SwaggerOperation("UserDistrictsIdPost")]
        [SwaggerResponse(200, type: typeof(UserDistrict))]
        [RequiresPermission(Permission.UserManagement)]
        public virtual IActionResult UserDistrictsIdPost([FromRoute]int id, [FromBody]UserDistrict item)
        {
            return _service.UserDistrictsIdPostAsync(id, item);
        }

        /// <summary>
        /// Switch User District
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">User District switched</response>
        [HttpPost]
        [Route("/api/userdistricts/{id}/switch")]
        [SwaggerOperation("UserDistrictsIdSwitchPost")]
        [SwaggerResponse(200, type: typeof(UserDistrict))]
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult UserDistrictsIdSwitchPost([FromRoute]int id)
        {
            return _service.UserDistrictsIdSwitchPostAsync(id);
        }
    }
}
