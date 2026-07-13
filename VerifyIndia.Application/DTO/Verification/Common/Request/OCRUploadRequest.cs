using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.Common.Request
{
    public class OCRUploadRequest
    {
        public string? type { get; set; }
        public IFormFile? file { get; set; }
        public string? link { get; set; }
        public string? back { get; set; }
    }
}
