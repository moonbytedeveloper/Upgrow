using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        public class UploadRequest
        {
            public IFormFile? File { get; set; }
            public string? CompanyName { get; set; }
            public string? FolderName { get; set; }
        }

        private readonly IWebHostEnvironment _env;

        public FileController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("upload")]
        [RequestSizeLimit(50_000_000)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] UploadRequest request)
        {
            var file = request.File;
            var companyName = request.CompanyName;
            var folderName = request.FolderName;

            if (file is null || file.Length == 0)
            {
                return BadRequest("No file provided.");
            }

            if (string.IsNullOrWhiteSpace(companyName) || string.IsNullOrWhiteSpace(folderName))
            {
                return BadRequest("Company name and folder name are required.");
            }

            var webRoot = Path.Combine(_env.ContentRootPath, "wwwroot");

            var safeCompany = Path.GetFileName(companyName.Trim());
            var safeFolder = Path.GetFileName(folderName.Trim());

            var uploadsRoot = Path.Combine(webRoot, safeCompany, safeFolder);
            Directory.CreateDirectory(uploadsRoot);

            var extension = Path.GetExtension(file.FileName);
            var safeFileName = $"{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(uploadsRoot, safeFileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var relativeUrl = $"{safeCompany}/{safeFolder}/{safeFileName}";

            return Ok(new
            {
                FileName = safeFileName,
                RelativeUrl = relativeUrl
            });
        }
    }
}