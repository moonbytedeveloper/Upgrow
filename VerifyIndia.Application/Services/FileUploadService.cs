using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Upgrow.Application.IServices;
using Upgrow.Application.Options;

namespace Upgrow.Application.Services
{
    public class FileUploadService : IFileUploadService
    {
        private const long DefaultMaxFileSizeBytes = 10 * 1024 * 1024;

        private static readonly string[] DefaultAllowedExtensions =
        {
            ".jpg", ".jpeg", ".png", ".webp",
            ".pdf", ".doc", ".docx", ".xls", ".xlsx",
            ".webm", ".mp4"
        };

        private static readonly Regex SafeSegmentRegex =
            new("^[a-zA-Z0-9_-]{1,100}$", RegexOptions.Compiled);

        private static readonly Dictionary<string, string[]> AllowedMimeTypesByExtension =
            new(StringComparer.OrdinalIgnoreCase)
            {
                [".jpg"] = new[] { "image/jpeg" },
                [".jpeg"] = new[] { "image/jpeg" },
                [".png"] = new[] { "image/png" },
                [".webp"] = new[] { "image/webp" },
                [".pdf"] = new[] { "application/pdf" },
                [".doc"] = new[] { "application/msword" },
                [".docx"] = new[]
                {
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "application/zip"
                },
                [".xls"] = new[] { "application/vnd.ms-excel" },
                [".xlsx"] = new[]
                {
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "application/zip"
                },
                [".webm"] = new[] { "video/webm" },
                [".mp4"] = new[] { "video/mp4" }
            };

        private static readonly Dictionary<string, byte[][]> FileSignatures =
            new(StringComparer.OrdinalIgnoreCase)
            {
                [".jpg"] = new[] { new byte[] { 0xFF, 0xD8, 0xFF } },
                [".jpeg"] = new[] { new byte[] { 0xFF, 0xD8, 0xFF } },
                [".png"] = new[] { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } },
                [".pdf"] = new[] { new byte[] { 0x25, 0x50, 0x44, 0x46 } },
                [".doc"] = new[] { new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 } },
                [".xls"] = new[] { new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 } },
                [".docx"] = new[]
                {
                    new byte[] { 0x50, 0x4B, 0x03, 0x04 },
                    new byte[] { 0x50, 0x4B, 0x05, 0x06 },
                    new byte[] { 0x50, 0x4B, 0x07, 0x08 }
                },
                [".xlsx"] = new[]
                {
                    new byte[] { 0x50, 0x4B, 0x03, 0x04 },
                    new byte[] { 0x50, 0x4B, 0x05, 0x06 },
                    new byte[] { 0x50, 0x4B, 0x07, 0x08 }
                },
                [".webm"] = new[] { new byte[] { 0x1A, 0x45, 0xDF, 0xA3 } },
                [".mp4"] = new[] { new byte[] { 0x00, 0x00, 0x00, 0x18, 0x66, 0x74, 0x79, 0x70 } }
            };

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly FileUploadOptions _options;
        private readonly IFileMalwareScanner _malwareScanner;

        public FileUploadService(
            IHttpClientFactory httpClientFactory,
            IOptions<FileUploadOptions> options,
            IFileMalwareScanner malwareScanner)
        {
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
            _malwareScanner = malwareScanner;
        }

        public async Task<string?> SaveFileAsync(IFormFile file, string companyName, string folderName, string[]? allowedExtensions = null)
        {
            if (file == null || file.Length == 0)
                return null;

            var safeFolderName = SanitizePathSegment(folderName);
            var effectiveCompany = string.IsNullOrWhiteSpace(companyName) ? "Unknown" : companyName;
            var safeCompanyName = SanitizePathSegment(effectiveCompany);

            await ValidateFileForUploadAsync(file, allowedExtensions, DefaultMaxFileSizeBytes);

            var httpClient = _httpClientFactory.CreateClient("FileApiUpload");
            using var form = new MultipartFormDataContent();

            await using var fileStream = file.OpenReadStream();
            using var fileContent = new StreamContent(fileStream);

            fileContent.Headers.ContentType = new MediaTypeHeaderValue(
                string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType);

            form.Add(fileContent, "File", file.FileName);
            form.Add(new StringContent(safeCompanyName), "CompanyName");
            form.Add(new StringContent(safeFolderName), "FolderName");

            using var response = await httpClient.PostAsync("/api/file/upload", form);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"File upload failed: {(int)response.StatusCode} - {responseBody}");

            using var json = JsonDocument.Parse(responseBody);

            if (json.RootElement.TryGetProperty("RelativeUrl", out var relativeUrlElement))
                return relativeUrlElement.GetString();

            if (json.RootElement.TryGetProperty("relativeUrl", out relativeUrlElement))
                return relativeUrlElement.GetString();

            throw new InvalidOperationException("Upload API response did not include RelativeUrl.");
        }

        public bool ValidateFile(IFormFile file, string[] allowedExtensions, long maxSizeInBytes)
        {
            if (file == null || file.Length == 0)
                return false;

            if (file.Length > maxSizeInBytes)
                return false;

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var extensionAllowed = allowedExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase);

            if (!extensionAllowed)
                return false;

            if (!string.IsNullOrWhiteSpace(file.ContentType) &&
                AllowedMimeTypesByExtension.TryGetValue(fileExtension, out var allowedMimes) &&
                !allowedMimes.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        private async Task ValidateFileForUploadAsync(IFormFile file, string[]? allowedExtensions, long maxSizeInBytes)
        {
            var effectiveExtensions = (allowedExtensions == null || allowedExtensions.Length == 0)
                ? DefaultAllowedExtensions
                : allowedExtensions;

            if (!ValidateFile(file, effectiveExtensions, maxSizeInBytes))
                throw new InvalidOperationException("Invalid file type or size.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            //var signatureValid = await ValidateSignatureAsync(file, extension);

            //if (!signatureValid)
            //    throw new InvalidOperationException("Invalid file signature.");

            await using var scanStream = file.OpenReadStream();
            var isClean = await _malwareScanner.IsCleanAsync(scanStream);
            if (!isClean)
                throw new InvalidOperationException("Malware detected in uploaded file.");
        }

        private static string SanitizePathSegment(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Path segment is required.", nameof(value));

            var trimmed = value.Trim();

            if (trimmed.Contains("..", StringComparison.Ordinal) ||
                trimmed.Contains(Path.DirectorySeparatorChar) ||
                trimmed.Contains(Path.AltDirectorySeparatorChar))
            {
                throw new ArgumentException("Invalid path segment.", nameof(value));
            }

            if (!SafeSegmentRegex.IsMatch(trimmed))
                throw new ArgumentException("Path segment contains unsafe characters.", nameof(value));

            return trimmed;
        }

        private static async Task<bool> ValidateSignatureAsync(IFormFile file, string extension)
        {
            var header = await ReadHeaderAsync(file, 16);

            if (extension.Equals(".webp", StringComparison.OrdinalIgnoreCase))
            {
                return header.Length >= 12 &&
                       header[0] == 0x52 && header[1] == 0x49 && header[2] == 0x46 && header[3] == 0x46 &&
                       header[8] == 0x57 && header[9] == 0x45 && header[10] == 0x42 && header[11] == 0x50;
            }
            if (extension.Equals(".mp4", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (extension.Equals(".webm", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (!FileSignatures.TryGetValue(extension, out var signatures))
                return true;

            return signatures.Any(signature =>
                header.Length >= signature.Length &&
                header.AsSpan(0, signature.Length).SequenceEqual(signature));
        }

        private static async Task<byte[]> ReadHeaderAsync(IFormFile file, int maxLength)
        {
            var buffer = new byte[maxLength];
            await using var stream = file.OpenReadStream();
            var totalRead = 0;

            while (totalRead < maxLength)
            {
                var read = await stream.ReadAsync(buffer.AsMemory(totalRead, maxLength - totalRead));
                if (read == 0)
                    break;

                totalRead += read;
            }

            return totalRead == maxLength ? buffer : buffer[..totalRead];
        }
    }
}