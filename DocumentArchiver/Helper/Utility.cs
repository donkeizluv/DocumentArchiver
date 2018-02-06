using Microsoft.AspNetCore.Http;
using NLog;
using System;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DocumentArchiver.Helper
{
    public static class Utility
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
        public static bool GetUpload(string fileName, string folderPath, out FileStream stream)
        {
            stream = null;
            try
            {
                var fullPath = new FileInfo(Path.Combine(folderPath, fileName));
                stream = fullPath.OpenRead();
                return true;
            }
            catch(FileNotFoundException ex)
            {
                EnviromentHelper.LogException(ex, _logger);
                return false;
            }
            catch(IOException ex)
            {
                EnviromentHelper.LogException(ex, _logger);
                return false;
            }
            catch(UnauthorizedAccessException ex)
            {
                EnviromentHelper.LogException(ex, _logger);
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
                EnviromentHelper.LogException(ex, _logger);
                return false;
            }
            catch (IOException ex)
            {
                EnviromentHelper.LogException(ex, _logger);
                return false;
            }
            catch (UnauthorizedAccessException ex)
            {
                EnviromentHelper.LogException(ex, _logger);
                return false;
            }

        }
        public static string GetContextUsername(HttpContext context)
        {
            return context.User.FindFirst(ClaimTypes.Name).Value;
        }
        public static bool Decode64(string base64, out int decoded)
        {
            decoded = -1;
            if (string.IsNullOrEmpty(base64)) return false;
            try
            {
                var data = Convert.FromBase64String(base64);
                string decodedString = Encoding.UTF8.GetString(data);
                decoded = int.Parse(decodedString);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
