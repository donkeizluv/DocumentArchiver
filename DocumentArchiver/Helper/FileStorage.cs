using Microsoft.AspNetCore.Http;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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

        public static async Task<MemoryStream> CreateZipStream(IEnumerable<KeyValuePair<string, string>> fileList, string folder, CompressionLevel level = CompressionLevel.Fastest)
        {
            var stream = new MemoryStream();
            var archive = new ZipArchive(stream, ZipArchiveMode.Create, true);

            foreach (var file in fileList)
            {
                var fileName = file.Value;
                if (!GetUpload(file.Key, folder, out var contentStream))
                {
                    _logger.Error($"Cant find {file.Key} to zip");
                    return null;
                }
                var zipArchiveEntry = archive.CreateEntry(fileName, level);
                using (contentStream)
                {
                    var read = new byte[contentStream.Length];
                    await contentStream.ReadAsync(read, 0, (int)contentStream.Length);
                    using (var entryStream = zipArchiveEntry.Open())
                    {
                        entryStream.Write(read, 0, read.Length);
                    }
                }
            }
            stream.Position = 0;
            return stream;
        }
        public static bool GetUpload(string fileName, string folderPath, out FileStream fileStream)
        {
            fileStream = null;
            try
            {
                var fullPath = new FileInfo(Path.Combine(folderPath, fileName));
                fileStream = fullPath.OpenRead();
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
