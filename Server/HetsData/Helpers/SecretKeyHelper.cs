using System;

namespace HetsData.Helpers
{
    public static class SecretKeyHelper
    {
        private const string AllowedChars = "23456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghkmnpqrsuvwxyz@#";

        public static string RandomString(int maxLength, int id)
        {
            // seed random number generator
            decimal temp = (DateTime.Now.Millisecond * 1000 / id) + (id * DateTime.Now.Millisecond);
            int seed = Convert.ToInt32(Math.Round(temp, 0));
            Random rnd = new(seed);

            // create random string
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
