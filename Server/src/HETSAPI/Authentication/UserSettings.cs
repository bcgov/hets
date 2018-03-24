using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using HETSAPI.Models;
using Microsoft.Extensions.Logging;

namespace HETSAPI.Authentication
{
    /// <summary>
    /// Object to track and manage the authenticated user session
    /// </summary>
    public class UserSettings
    {
        /// <summary>
        /// True if user is authenticated
        /// </summary>
        public bool UserAuthenticated { get; set; }

        /// <summary>
        /// HETS/SiteMinder User Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// SiteMinder Guid
        /// </summary>
        public string SiteMinderGuid { get; set; }

        /// <summary>
        /// HETS User Model
        /// </summary>
        public User HetsUser { get; set; }

        /// <summary>
        /// Serializes UserSettings as a Json String
        /// </summary>
        /// <returns></returns>
        public string GetJson()
        {
            // write metadata
            string json = JsonConvert.SerializeObject(this, Formatting.Indented,
                new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }
                );

            return json;
        }

        /// <summary>
        /// Create UserSettings object from a Serialized Json String
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static UserSettings CreateFromJson(string json)
        {
            UserSettings temp = JsonConvert.DeserializeObject<UserSettings>(json);
            return temp;
        }

        /// <summary>
        /// Save UserSettings to Session
        /// </summary>
        /// <param name="userSettings"></param>
        /// <param name="context"></param>
        public static void SaveUserSettings(UserSettings userSettings, HttpContext context)
        {
            string temp = userSettings.GetJson();

            context.Response.Cookies.Append(
                "UserSettings",
                temp,
                new CookieOptions
                {
                    Path = "/",
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                });          
        }

        /// <summary>
        /// Clear UserSettings
        /// </summary>
        /// <param name="context"></param>
        public static void ClearUserSettings(HttpContext context)
        {
            context.Response.Cookies.Delete("UserSettings");
        }

        /// <summary>
        /// Retrieve UserSettings from Session
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static UserSettings ReadUserSettings(HttpContext context, ILogger logger)
        {
            UserSettings userSettings = new UserSettings();

            if (context.Request.Cookies["UserSettings"] == null)
            {
                logger.LogInformation("UserSettings cookie not found");
                return userSettings;
            }

            logger.LogInformation("UserSettings cookie found - deserializing");
            string settingsTemp = context.Request.Cookies["UserSettings"];

            return !string.IsNullOrEmpty(settingsTemp) ? CreateFromJson(settingsTemp) : userSettings;
        }
    }
}

