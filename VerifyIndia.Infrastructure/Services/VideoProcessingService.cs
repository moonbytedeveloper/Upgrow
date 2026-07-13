using FFMpegCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.IServices;
using VerifyIndia.Application.IServices.Common;

namespace VerifyIndia.Infrastructure.Services
{
    public class VideoProcessingService
        : IVideoProcessingService
    {
        private readonly IConfiguration _configuration;

        private readonly IFileUploadService
            _fileUploadService;

        public VideoProcessingService(
            IConfiguration configuration,
            IFileUploadService fileUploadService)
        {
            _configuration =
                configuration;

            _fileUploadService =
                fileUploadService;
        }

        public string GetPhysicalPath(
            string relativePath)
        {
            if (string.IsNullOrWhiteSpace(
                    relativePath))
            {
                throw new ArgumentException(
                    "Video path is required.",
                    nameof(relativePath));
            }

            var storageRoot =
                _configuration[
                    "VideoKyc:StorageRoot"];

            if (string.IsNullOrWhiteSpace(
                    storageRoot))
            {
                throw new Exception(
                    "Video storage root is not configured.");
            }

            return Path.Combine(
                storageRoot,
                relativePath
                    .TrimStart('/')
                    .Replace(
                        '/',
                        Path.DirectorySeparatorChar));
        }

        public async Task<string> CompressAsync(
            string inputFilePath)
        {
            if (string.IsNullOrWhiteSpace(
                    inputFilePath))
            {
                throw new ArgumentException(
                    nameof(inputFilePath));
            }

            if (!File.Exists(
                    inputFilePath))
            {
                throw new FileNotFoundException(
                    "Video file not found.",
                    inputFilePath);
            }

            var outputFilePath =
                Path.Combine(
                    Path.GetTempPath(),
                    $"{Guid.NewGuid()}.mp4");

            try
            {
                await FFMpegArguments
                    .FromFileInput(
                        inputFilePath)
                    .OutputToFile(
                        outputFilePath,
                        true,
                        options =>
                            options
                                .WithVideoCodec(
                                    "libx264")
                                .WithAudioCodec(
                                    "aac")
                                .WithConstantRateFactor(
                                    28)
                                .WithCustomArgument(
                                    "-preset medium")
                                .WithCustomArgument(
                                    "-b:a 96k")
                                .WithCustomArgument(
                                    "-movflags +faststart"))
                    .ProcessAsynchronously();

                var info =
                    new FileInfo(
                        outputFilePath);

                if (!info.Exists ||
                    info.Length == 0)
                {
                    throw new Exception(
                        "Video compression failed.");
                }

                return outputFilePath;
            }
            catch
            {
                if (File.Exists(
                        outputFilePath))
                {
                    File.Delete(
                        outputFilePath);
                }

                throw;
            }
        }

        public async Task<string> ConvertToWavAsync(
            string videoPath)
        {
            if (!File.Exists(
                    videoPath))
            {
                throw new FileNotFoundException(
                    "Video file not found.",
                    videoPath);
            }

            var wavPath =
                Path.Combine(
                    Path.GetTempPath(),
                    $"{Guid.NewGuid()}.wav");

            try
            {
                await FFMpegArguments
                    .FromFileInput(
                        videoPath)
                    .OutputToFile(
                        wavPath,
                        true,
                        options =>
                            options
                                .WithAudioCodec(
                                    "pcm_s16le")
                                .WithAudioSamplingRate(
                                    16000)
                                .ForceFormat(
                                    "wav"))
                    .ProcessAsynchronously();

                var info =
                    new FileInfo(
                        wavPath);

                if (!info.Exists ||
                    info.Length == 0)
                {
                    throw new Exception(
                        "Failed to convert video.");
                }

                return wavPath;
            }
            catch
            {
                if (File.Exists(
                        wavPath))
                {
                    File.Delete(
                        wavPath);
                }

                throw;
            }
        }

        public async Task<string> UploadCompressedAsync(
            IFormFile video)
        {
            if (video == null)
            {
                throw new ArgumentNullException(
                    nameof(video));
            }

            var tempFolder =
                Path.Combine(
                    Path.GetTempPath(),
                    "AuthenticateIndia",
                    "VideoKyc");

            Directory.CreateDirectory(
                tempFolder);

            var extension =
                Path.GetExtension(
                    video.FileName);

            var originalFilePath =
                Path.Combine(
                    tempFolder,
                    $"{Guid.NewGuid()}{extension}");

            string? compressedFilePath =
                null;

            try
            {
                await using (
                    var stream =
                        new FileStream(
                            originalFilePath,
                            FileMode.Create))
                {
                    await video.CopyToAsync(
                        stream);
                }

                compressedFilePath =
                    await CompressAsync(
                        originalFilePath);

                await using var compressedStream =
                    new FileStream(
                        compressedFilePath,
                        FileMode.Open,
                        FileAccess.Read);

                IFormFile compressedFile =
                    new FormFile(
                        compressedStream,
                        0,
                        compressedStream.Length,
                        "File",
                        Path.GetFileName(
                            compressedFilePath))
                    {
                        Headers =
                            new HeaderDictionary(),

                        ContentType =
                            "video/mp4"
                    };

                var relativeUrl =
                    await _fileUploadService
                        .SaveFileAsync(
                            compressedFile,
                            _configuration[
                                "FileUploadService:CompanyName"]!,
                            "VideoKyc",
                            new[]
                            {
                                ".mp4"
                            });

                if (string.IsNullOrWhiteSpace(
                        relativeUrl))
                {
                    throw new Exception(
                        "Video upload failed.");
                }

                return relativeUrl;
            }
            finally
            {
                if (File.Exists(
                        originalFilePath))
                {
                    File.Delete(
                        originalFilePath);
                }

                if (!string.IsNullOrWhiteSpace(
                        compressedFilePath)
                    &&
                    File.Exists(
                        compressedFilePath))
                {
                    File.Delete(
                        compressedFilePath);
                }
            }
        }
    }
}
