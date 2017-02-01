/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using System;
using System.Security.Claims;

namespace HETSAPI.Services.Impl
{
    public abstract class ServiceBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDbAppContext _context;

        public ServiceBase(IHttpContextAccessor httpContextAccessor, DbAppContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            DbContext = context;
        }

        protected IDbAppContext DbContext { get; private set;  }
        protected HttpRequest Request
        {
            get { return _httpContextAccessor.HttpContext.Request; }
        }

        protected ClaimsPrincipal User
        {
            get { return _httpContextAccessor.HttpContext.User; }
        }

        /// <summary>
        /// Returns the current user ID
        /// </summary>
        /// <returns></returns>
        protected int? GetCurrentUserId()
        {
            int? result = null;
            
            try
            {
                string rawuid = User.FindFirst(HETSAPI.Models.User.USERID_CLAIM).Value;
                result = int.Parse(rawuid);
            }
            catch (Exception e)
            {
                result = null;
            }
            return result;
        }

        protected OkObjectResult Ok(object value)
        {
            return new OkObjectResult(value);
        }
    }
}
