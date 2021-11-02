using ChatApp.BLL.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.BLL.Service
{
    public class FileService : IFileService
    {
        private readonly string _folderPath;

        public FileService(IHostingEnvironment env)
        {
            _folderPath = GetDirectory(env.WebRootPath);
        }

        public async Task<Uri> SaveFile(Stream stream, string fileExtension)
        {
            var uniqueName = Guid.NewGuid().ToString();
            var fileName = $"{uniqueName}{fileExtension}";
            var fullPath = FormatFullPath(fileName);

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await stream.CopyToAsync(fileStream);
            }

            return new Uri($"/fileuploads/{fileName}", UriKind.Relative);
        }

        public Task RemoveFile(string fileUrl)
        {
            var fileName = Path.GetFileName(fileUrl);

            var fullPath = FormatFullPath(fileName);

            if (!File.Exists(fullPath))
            {
                throw new Exception("File doestn exist");
            }

            File.Delete(fullPath);
            return Task.CompletedTask;
        }

        private string GetDirectory(string baseFolder)
        {
            if (string.IsNullOrWhiteSpace(baseFolder))
            {
                baseFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }
            var folder = Path.Combine(baseFolder, "fileuploads");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return folder;
        }

        private string FormatFullPath(string fileName)
        {
            return Path.Combine(_folderPath, fileName);
        }
    }
}
