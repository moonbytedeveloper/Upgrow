using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.Common.Request
{
    public class FaceMatchRequest
    {
        public string? thereshould { get; set; }
        public IFormFile? image1 { get; set; }
        public IFormFile? image2 { get; set; }
        public string? image1_Url { get; set; }
        public string? image2_Url { get; set; }
        public string? accept { get; set; }
    }
}
