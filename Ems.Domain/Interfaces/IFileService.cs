using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Domain.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFile(IFormFile file, CancellationToken cancellationToken);
        void DeleteFile(string filePath);
    }

    public class FileService : IFileService
    {
        public async Task<string> SaveFile(IFormFile file, CancellationToken cancellationToken)
        {
            var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadsDirectory))
                Directory.CreateDirectory(uploadsDirectory);

            var fileName = Path.GetRandomFileName();
            var filePath = Path.Combine(uploadsDirectory, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }

            return filePath;
        }

        public void DeleteFile(string filePath)
        {
            System.IO.File.Delete(filePath);
        }
    }

}
