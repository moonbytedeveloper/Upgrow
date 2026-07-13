using Microsoft.Extensions.Configuration;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Registration.VideoKyc;
using VerifyIndia.Application.IServices.Registration;

namespace VerifyIndia.Infrastructure.Services
{
    public sealed class FaceDetectionService
        : IFaceDetectionService
    {
        private readonly IConfiguration _configuration;

        public FaceDetectionService(
            IConfiguration configuration)
        {
            _configuration =
                configuration;
        }

        private string GetCascadePath()
        {
            var relativePath =
                _configuration[
                    "OpenCv:FaceCascadePath"];

            if (string.IsNullOrWhiteSpace(
                    relativePath))
            {
                throw new Exception(
                    "OpenCV cascade path not configured.");
            }

            var cascadePath =
                Path.Combine(
                    AppContext.BaseDirectory,
                    relativePath.Replace(
                        '/',
                        Path.DirectorySeparatorChar));

            if (!File.Exists(
                    cascadePath))
            {
                throw new FileNotFoundException(
                    $"Cascade file not found: {cascadePath}");
            }

            return cascadePath;
        }

        public async Task<FaceDetectionResultDto>
            DetectFaceAsync(
                string videoPath)
        {
            return await Task.Run(() =>
            {
                var cascadePath = GetCascadePath();

                var classifier =
                    new CascadeClassifier(
                        cascadePath);

                using var capture =
                    new VideoCapture(
                        videoPath);

                var frameCount =
                    (int)capture.FrameCount;

                var fps =
                    capture.Fps;

                var interval =
                    Math.Max(
                        1,
                        (int)fps);

                var totalFrames = 0;
                var faceFrames = 0;

                using var frame =
                    new Mat();

                for (int i = 0;
                    i < frameCount;
                    i += interval)
                {
                    capture.PosFrames = i;

                    if (!capture.Read(frame))
                    {
                        continue;
                    }

                    totalFrames++;

                    using var gray =
                        new Mat();

                    Cv2.CvtColor(
                        frame,
                        gray,
                        ColorConversionCodes.BGR2GRAY);

                    var faces =
                        classifier.DetectMultiScale(
                            gray,
                            1.1,
                            4);

                    if (faces.Length > 0)
                    {
                        faceFrames++;
                    }
                }

                decimal percentage =
                    totalFrames == 0
                        ? 0
                        : (decimal)faceFrames
                          / totalFrames
                          * 100;

                return new FaceDetectionResultDto
                {
                    FaceDetected =
                        percentage >= 70,

                    FacePercentage =
                        percentage,

                    TotalFrames =
                        totalFrames,

                    FaceFrames =
                        faceFrames
                };
            });
        }
    }
}
