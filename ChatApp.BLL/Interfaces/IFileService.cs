using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.BLL.Interfaces
{
    public interface IFileService
    {
        Task<Uri> SaveFile(Stream stream, string fileExtension);
        Task RemoveFile(string fileUrl);
    }
}
