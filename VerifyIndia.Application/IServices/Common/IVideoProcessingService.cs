using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.IServices.Common
{
    public interface IVideoProcessingService
    {
        /// <summary>
        /// Compresses a video and returns the compressed
        /// temporary file path.
        /// Caller is responsible for deleting the returned file.
        /// </summary>
        Task<string> CompressAsync(
            string inputFilePath);

        /// <summary>
        /// Converts a video into a temporary WAV file.
        /// Caller is responsible for deleting the returned file.
        /// </summary>
        Task<string> ConvertToWavAsync(
            string videoPath);

        /// <summary>
        /// Converts a relative VideoUrl stored in database
        /// into its physical path on disk.
        /// </summary>
        string GetPhysicalPath(
            string relativePath);

        /// <summary>
        /// Compresses the uploaded video,
        /// uploads it to File API and
        /// returns the relative url.
        /// </summary>
        Task<string> UploadCompressedAsync(
            IFormFile video);
    }
}
