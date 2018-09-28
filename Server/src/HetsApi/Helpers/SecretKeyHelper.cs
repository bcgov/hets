using System;

namespace HetsApi.Helpers
{
    public static class SecretKeyHelper
    {
        private const string AllowedChars = "123456789ABCDEFGHIJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz!@$#%&";
        
        public static string RandomString(int maxLength)
        {
            Random rnd = new Random(DateTime.UtcNow.Millisecond);

            char[] chars = new char[maxLength];

            for (int i = 0; i < maxLength; i++)
            {
                int random = rnd.Next(0, AllowedChars.Length);
                chars[i] = AllowedChars[random];
            }

            return new string(chars);
        }
    }
}
