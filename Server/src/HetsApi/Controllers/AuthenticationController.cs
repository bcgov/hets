using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HetsApi.Authentication;
using Microsoft.AspNetCore.Http.Extensions;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Authentication Controller
    /// </summary>
    [Route("api/authentication")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class AuthenticationController : Controller
    {
        private readonly SiteMinderAuthOptions _options = new SiteMinderAuthOptions();
        private readonly IHostingEnvironment _env;
        private readonly HttpContext _httpContext;

        /// <summary>
        /// Authentication Controller Constructor
        /// </summary>
        /// <param name="env"></param>
        /// <param name="httpContextAccessor"></param>
        public AuthenticationController(IHostingEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContext = httpContextAccessor.HttpContext;
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
            bool accessEnabled = false;
            string url = _httpContext.Request.GetDisplayUrl().ToLower();

            if ((_httpContext.Connection.RemoteIpAddress.ToString().StartsWith("::1") ||
                 _httpContext.Connection.RemoteIpAddress.ToString().StartsWith("::ffff:127.0.0.1")) &&
                     url.StartsWith("http://localhost:8080"))
            {
                accessEnabled = true;
            }

            if (!_env.IsDevelopment() && !accessEnabled) return BadRequest("This API is not available outside a development environment.");

            if (string.IsNullOrEmpty(userId)) return BadRequest("Missing required userid query parameter.");

            userId = userId.Trim();

            if (userId.ToLower() == "default")
            {
                userId = _options.DevDefaultUserId;
            }
                        
            // create new "dev" user cookie
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
        /// Injects an authentication token cookie into the response for use with the 
        /// SiteMinder authentication middleware - Business User
        /// </summary>
        [HttpGet]
        [Route("dev/businessToken/{userid}/{guid}")]
        [AllowAnonymous]
        public virtual IActionResult GetDevBusinessCookie(string userId, string guid)
        {
            if (!_env.IsDevelopment()) return BadRequest("This API is not available outside a development environment.");

            if (string.IsNullOrEmpty(userId)) return BadRequest("Missing required userid query parameter.");
            if (string.IsNullOrEmpty(guid)) return BadRequest("Missing required userid query parameter.");

            userId = userId.Trim();
            guid = guid.Trim();

            string temp = userId + "," + guid;

            // create new "dev" user cookie
            Response.Cookies.Append(
                _options.DevBusinessTokenKey,
                temp,
                new CookieOptions
                {
                    Path = "/",
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(7)
                }
            );

            Debug.WriteLine("New Cookie User: " + userId);
            Debug.WriteLine("New Cookie Guid: " + guid);

            return Ok();
        }        

        /// <summary>
        /// Clear out any existing dev authentication tokens
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("dev/clearToken")]
        [AllowAnonymous]
        public virtual IActionResult ClearDevAuthenticationCookie()
        {
            bool accessEnabled = false;
            string url = _httpContext.Request.GetDisplayUrl().ToLower();

            if ((_httpContext.Connection.RemoteIpAddress.ToString().StartsWith("::1") ||
                 _httpContext.Connection.RemoteIpAddress.ToString().StartsWith("127.0.0.1")) &&
                url.StartsWith("http://localhost:8080"))
            {
                accessEnabled = true;
            }

            if (!_env.IsDevelopment() && !accessEnabled) return BadRequest("This API is not available outside a development environment.");

            // *************************
            // clear up user cookie
            // *************************
            string temp = HttpContext.Request.Cookies[_options.DevAuthenticationTokenKey];
            
            if (temp != null)
            {
                Debug.WriteLine("Current Cookie User: " + temp);

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
            }

            // *************************
            // clear up business cookie
            // *************************
            temp = HttpContext.Request.Cookies[_options.DevBusinessTokenKey];

            if (temp != null)
            {
                Debug.WriteLine("Current Cookie User: " + temp);

                // expire "dev" user cookie
                Response.Cookies.Append(
                    _options.DevBusinessTokenKey,
                    temp,
                    new CookieOptions
                    {
                        Path = "/",
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddDays(-1)
                    }
                );
            }

            Debug.WriteLine("Cookie Expired!");

            return Ok();            
        }        
    }
}

