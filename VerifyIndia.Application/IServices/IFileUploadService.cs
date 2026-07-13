using Microsoft.AspNetCore.Http;

namespace VerifyIndia.Application.IServices
{
    public interface IFileUploadService
    {    
        Task<string?> SaveFileAsync(IFormFile file, string companyName,string folderName, string[]? allowedExtensions = null);       

        /// <summary>
        /// Validates file type and size
        /// </summary>
        bool ValidateFile(IFormFile file, string[] allowedExtensions, long maxSizeInBytes);
    }
}