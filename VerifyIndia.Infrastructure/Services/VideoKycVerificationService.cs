using AuthenticateIndia.Shared.Constants;
using FFMpegCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Registration.VideoKyc;
using VerifyIndia.Application.IServices;
using VerifyIndia.Application.IServices.Common;
using VerifyIndia.Application.IServices.Registration;
using VerifyIndia.Domain.Entities.Registration;
using Whisper.net;

namespace VerifyIndia.Infrastructure.Services
{
    public class VideoKycVerificationService
        : IVideoKycVerificationService
    {
        private readonly IVideoProcessingService _videoProcessingService;
        private readonly IFaceDetectionService _faceDetectionService;
        private readonly IAppSettingService _appSettingService;
        private readonly IConfiguration _configuration;

        public VideoKycVerificationService(
            IVideoProcessingService videoProcessingService,
            IFaceDetectionService faceDetectionService,
            IConfiguration configuration,
            IAppSettingService appSettingService)
        {
            _videoProcessingService = 
                videoProcessingService;

            _faceDetectionService =
                faceDetectionService;

            _configuration =
                configuration;

            _appSettingService =
                appSettingService;
        }

        private int CalculateLevenshteinDistance(
            string source,
            string target)
        {
            if (string.IsNullOrEmpty(source))
            {
                return target.Length;
            }

            if (string.IsNullOrEmpty(target))
            {
                return source.Length;
            }

            var matrix =
                new int[
                    source.Length + 1,
                    target.Length + 1];

            for (var i = 0; i <= source.Length; i++)
            {
                matrix[i, 0] = i;
            }

            for (var j = 0; j <= target.Length; j++)
            {
                matrix[0, j] = j;
            }

            for (var i = 1; i <= source.Length; i++)
            {
                for (var j = 1; j <= target.Length; j++)
                {
                    var cost =
                        source[i - 1] == target[j - 1]
                            ? 0
                            : 1;

                    matrix[i, j] =
                        Math.Min(
                            Math.Min(
                                matrix[i - 1, j] + 1,
                                matrix[i, j - 1] + 1),
                            matrix[i - 1, j - 1] + cost);
                }
            }

            return matrix[
                source.Length,
                target.Length];
        }

        private decimal CalculateSimilarity(
    string source,
    string target)
        {
            source =
                Normalize(source);

            target =
                Normalize(target);

            if (string.IsNullOrWhiteSpace(source)
                ||
                string.IsNullOrWhiteSpace(target))
            {
                return 0;
            }

            var distance =
                CalculateLevenshteinDistance(
                    source,
                    target);

            var maxLength =
                Math.Max(
                    source.Length,
                    target.Length);

            return
                (1m -
                 ((decimal)distance /
                  maxLength))
                * 100m;
        }

        private string GetModelPath()
        {
            var relativePath =
                _configuration[
                    "Whisper:ModelPath"];

            if (string.IsNullOrWhiteSpace(
                    relativePath))
            {
                throw new Exception(
                    "Whisper model path not configured.");
            }

            return Path.Combine(
                AppContext.BaseDirectory,
                relativePath.Replace(
                    '/',
                    Path.DirectorySeparatorChar));
        }

        private string Normalize(
            string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            value =
                value.ToLowerInvariant();

            value =
                Regex.Replace(
                    value,
                    @"[^\w\s]",
                    " ");

            value =
                Regex.Replace(
                    value,
                    @"\s+",
                    " ");

            return value.Trim();
        }

        private async Task<string> TranscribeAsync(
            string wavPath)
        {
            var modelPath = GetModelPath();

            if (!File.Exists(modelPath))
            {
                throw new FileNotFoundException(
                    $"Whisper model not found: {modelPath}");
            }

            using var whisperFactory =
                WhisperFactory.FromPath(
                    modelPath);

            using var processor =
                whisperFactory
                    .CreateBuilder()
                    .WithLanguage("en")
                    .Build();

            using var audioStream =
                File.OpenRead(
                    wavPath);

            var transcript =
                new StringBuilder();

            await foreach (
                var segment
                in processor.ProcessAsync(
                    audioStream))
            {
                transcript.Append(' ');
                transcript.Append(
                    segment.Text);
            }

            return transcript
                .ToString()
                .Trim();
        }

        private string GetVideoPath(string relativePath)
        {
            var storageRoot =
                _configuration[
                    "VideoKyc:StorageRoot"];

            return Path.Combine(
                storageRoot!,
                relativePath
                    .TrimStart('/')
                    .Replace(
                        '/',
                        Path.DirectorySeparatorChar));
        }

        private async Task<string> ConvertToWavAsync(
            string videoPath)
        {
            var wavPath =
                Path.Combine(
                    Path.GetTempPath(),
                    $"{Guid.NewGuid()}.wav");

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

            return wavPath;
        }

        public async Task<VideoKycVerificationResultDto> VerifyAsync(
    CustomerVideoKyc challenge,
    string videoUrl)
        {
            string? wavPath = null;

            try
            {
                if (challenge == null)
                {
                    throw new ArgumentNullException(
                        nameof(challenge));
                }

                if (string.IsNullOrWhiteSpace(
                        videoUrl))
                {
                    throw new ArgumentException(
                        "Video URL is required.",
                        nameof(videoUrl));
                }

                var videoPath =
                    _videoProcessingService
                        .GetPhysicalPath(
                            videoUrl);

                if (!File.Exists(
                        videoPath))
                {
                    throw new FileNotFoundException(
                        "Video file not found.",
                        videoPath);
                }

                wavPath =
                    await _videoProcessingService
                        .ConvertToWavAsync(
                            videoPath);

                var spokenText =
                    await TranscribeAsync(
                        wavPath);

                if (string.IsNullOrWhiteSpace(
                        spokenText))
                {
                    throw new Exception(
                        "Unable to transcribe video.");
                }

                var expectedText =
                    challenge.ChallengeText
                    ?? string.Empty;

                var speechSimilarity =
                    Math.Round(
                        CalculateSimilarity(
                            expectedText,
                            spokenText),
                        2);

                var minimumPercentage =
                    await _appSettingService
                        .GetIntValueAsync(
                            AppSettingKeys
                                .VIDEO_KYC_MIN_MATCH_PERCENTAGE,
                            85);

                var faceResult =
                    await _faceDetectionService
                        .DetectFaceAsync(
                            videoPath);

                var isMatched =
                    faceResult.FaceDetected
                    &&
                    speechSimilarity >=
                    minimumPercentage;

                string? failureReason =
                    null;

                if (!faceResult.FaceDetected)
                {
                    failureReason =
                        $"No face detected. Face Detection Score: {faceResult.FacePercentage:N2}%";
                }
                else if (speechSimilarity <
                         minimumPercentage)
                {
                    failureReason =
                        $"Speech similarity ({speechSimilarity:N2}%) is below required ({minimumPercentage}%).";
                }

                return new VideoKycVerificationResultDto
                {
                    IsMatched =
                        isMatched,

                    SpokenText =
                        spokenText,

                    SpeechMatchPercentage =
                        speechSimilarity,

                    FaceDetected =
                        faceResult.FaceDetected,

                    FaceDetectionPercentage =
                        Math.Round(
                            faceResult.FacePercentage,
                            2),

                    FailureReason =
                        failureReason
                };
            }
            catch (Exception ex)
            {
                return new VideoKycVerificationResultDto
                {
                    IsMatched = false,

                    SpokenText = string.Empty,

                    SpeechMatchPercentage = 0,

                    FaceDetected = false,

                    FaceDetectionPercentage = 0,

                    FailureReason = ex.Message
                };
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(
                        wavPath)
                    &&
                    File.Exists(
                        wavPath))
                {
                    File.Delete(
                        wavPath);
                }
            }
        }
    }
}
