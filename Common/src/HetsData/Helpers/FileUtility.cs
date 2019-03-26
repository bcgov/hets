using System;
using System.IO;

namespace HetsData.Helpers
{
    public static class FileUtility
    {
        public static bool ByteArrayToFile(string folder, string fileName, byte[] byteArray)
        {
            try
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                fileName = folder + fileName;

                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static byte[] FileToByteArray(string fileName)
        {
            try
            {
                byte[] buffer;

                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, (int)fs.Length);
                }

                return buffer;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void DeleteFile(string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    throw new ArgumentException("Report file not found");
                }

                File.Delete(fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
