using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using HETSAPI.Authentication;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Development Environment Authentication Service
    /// </summary>
    [Route("api/authentication")]
    public class AuthenticationController : Controller
    {
        private readonly SiteMinderAuthOptions _options = new SiteMinderAuthOptions();
        private readonly IHostingEnvironment _env;

        /// <summary>
        /// AuthenticationController Constructor
        /// </summary>
        /// <param name="env"></param>
        public AuthenticationController(IHostingEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// Injects an authentication token cookie into the response for use with the 
        /// SiteMinder authentication middleware
        /// </summary>
        [HttpGet]
        [Route("dev/token/{userid}")]
        [AllowAnonymous]
        public virtual IActionResult GetDevAuthenticationCookie(string userId)
        {
            if(!_env.IsDevelopment()) return BadRequest("This API is not available outside a development environment.");
            
            if (string.IsNullOrEmpty(userId)) return BadRequest("Missing required userid query parameter.");

            if (userId.ToLower() == "default")
                userId = _options.DevDefaultUserId;

            string temp = HttpContext.Request.Cookies[_options.DevAuthenticationTokenKey];
            Debug.WriteLine("Current Cookie User: " + temp);

            // clear session
            HttpContext.Session.Clear();

            // crearte new "dev" user cookie
            Response.Cookies.Append(
                _options.DevAuthenticationTokenKey,
                userId,
                new CookieOptions
                {
                    Path = "/",
                    SameSite = SameSiteMode.None,                    
                    Expires = DateTime.UtcNow.AddDays(7)
                }
            );

            Debug.WriteLine("New Cookie User: " + userId);

            return Ok();
        }

        /// <summary>
        /// Clear out any existing dev authentication tokens
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("dev/cleartoken")]
        [AllowAnonymous]
        public virtual IActionResult ClearDevAuthenticationCookie()
        {
            if (!_env.IsDevelopment()) return BadRequest("This API is not available outside a development environment.");

            string temp = HttpContext.Request.Cookies[_options.DevAuthenticationTokenKey];
            Debug.WriteLine("Current Cookie User: " + temp);

            // clear session
            HttpContext.Session.Clear();

            // expire "dev" user cookie
            Response.Cookies.Append(
                _options.DevAuthenticationTokenKey,
                temp,
                new CookieOptions
                {       
                    Path = "/",
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(-1)
                }
            );

            Debug.WriteLine("Cookie Expired!");

            return Ok();
        }
    }
}
