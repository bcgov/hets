using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
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
            temp = EncryptString(temp);
            
            context.Response.Cookies.Append(
                "UserSettings",
                temp,
                new CookieOptions
                {
                    Path = "/",
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                }
            );
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
            settingsTemp = DecryptString(settingsTemp);

            return !string.IsNullOrEmpty(settingsTemp) ? CreateFromJson(settingsTemp) : userSettings;
        }

        private static string EncryptString(string text)
        {
            const string key = "t#$fecvbt^&%4fhgh7&TFdw33wsx";

            if (text == null || text.Length <= 0)
            {                
                throw new ArgumentException("Cookie content cannot be null");
            }

            byte[] encrypted;

            byte[] keyBytes = Encoding.ASCII.GetBytes(key.PadLeft(32));

            using (Aes aesAlg = Aes.Create())
            {
                if (aesAlg == null)
                {
                    throw new CryptographicException("Cannot instantiate encryption algortihm");
                }

                aesAlg.Key = keyBytes;

                byte[] iv = aesAlg.IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream stream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            stream.Write(iv, 0, iv.Length);
                            streamWriter.Write(text);
                        }

                        encrypted = stream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        public static string DecryptString(string cipherText)
        {
            const string key = "t#$fecvbt^&%4fhgh7&TFdw33wsx";

            var fullCipher = Convert.FromBase64String(cipherText);

            if (fullCipher == null || fullCipher.Length <= 0)
            {
                throw new ArgumentException("Cookie content cannot be null");
            }

            string plaintext;

            byte[] keyBytes = Encoding.ASCII.GetBytes(key.PadLeft(32));

            using (Aes aesAlg = Aes.Create())
            {
                if (aesAlg == null)
                {
                    throw new CryptographicException("Cannot instantiate encryption algortihm");
                }

                aesAlg.Key = keyBytes;

                using (MemoryStream stream = new MemoryStream(fullCipher))
                {
                    byte[] iv = new byte[16];
                    stream.Read(iv, 0, 16);
                    aesAlg.IV = iv;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (CryptoStream cryptoStream = new CryptoStream(stream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            plaintext = streamReader.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}

