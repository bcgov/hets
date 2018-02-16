using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HETSAPI.Models;

namespace HETSAPI.Services.Impl
{
    public abstract class ServiceBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected ServiceBase(IHttpContextAccessor httpContextAccessor, DbAppContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            DbContext = context;
        }

        protected IDbAppContext DbContext { get; private set;  }

        protected HttpRequest Request => _httpContextAccessor.HttpContext.Request;

        protected ClaimsPrincipal User => _httpContextAccessor.HttpContext.User;

        /// <summary>
        /// Returns the current user ID
        /// </summary>
        /// <returns></returns>
        protected int? GetCurrentUserId()
        {
            int? result;
            
            try
            {
                string rawuid = User.FindFirst(Models.User.UseridClaim).Value;
                result = int.Parse(rawuid);
            }
            catch
            {
                result = null;
            }

            return result;
        }

        protected OkObjectResult Ok(object value)
        {
            return new OkObjectResult(value);
        }

        // parse a string of ints into an array.
        public int?[] ParseIntArray (string source)
        {
            int?[] result;

            try
            {
                string[] tokens = source.Split(',');
                result = new int?[tokens.Length];

                for (int i = 0; i < tokens.Length; i++)
                {
                    result[i] = int.Parse(tokens[i]);
                }
            }
            catch
            {
                result = null;
            }

            return result;
        }
    }
}
