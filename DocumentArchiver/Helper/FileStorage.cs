using Microsoft.AspNetCore.Http;
using NLog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DocumentArchiver.Helper
{
    public static class FileStorage
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        public static async Task SaveUpload(IFormFile file, string fileName, string folderPath)
        {
            //Create if no directory
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            var fullPath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
        public static bool DeleteUpload(string fileName, string folderPath)
        {
            try
            {
                File.Delete(Path.Combine(folderPath, fileName));
                return true;
            }
            catch (FileNotFoundException ex)
            {
                Utility.LogException(ex, _logger);
                return false;
            }
            catch (IOException ex)
            {
                Utility.LogException(ex, _logger);
                return false;
            }
            catch (UnauthorizedAccessException ex)
            {
                Utility.LogException(ex, _logger);
                return false;
            }
        }
        public static bool GetUpload(string fileName, string folderPath, out FileStream stream)
        {
            stream = null;
            try
            {
                var fullPath = new FileInfo(Path.Combine(folderPath, fileName));
                stream = fullPath.OpenRead();
                return true;
            }
            catch (FileNotFoundException ex)
            {
                Utility.LogException(ex, _logger);
                return false;
            }
            catch (IOException ex)
            {
                Utility.LogException(ex, _logger);
                return false;
            }
            catch (UnauthorizedAccessException ex)
            {
                Utility.LogException(ex, _logger);
                return false;
            }

        }
        public static bool GetUpload(string fileName, string folderPath, out byte[] bytesArr)
        {
            bytesArr = null;
            try
            {
                var fullPath = new FileInfo(Path.Combine(folderPath, fileName));
                bytesArr = File.ReadAllBytes(fullPath.FullName);
                return true;
            }
            catch (FileNotFoundException ex)
            {
                Utility.LogException(ex, _logger);
                return false;
            }
            catch (IOException ex)
            {
                Utility.LogException(ex, _logger);
                return false;
            }
            catch (UnauthorizedAccessException ex)
            {
                Utility.LogException(ex, _logger);
                return false;
            }

        }
    }
}
