using Microsoft.AspNetCore.Http;

namespace VerifyIndia.Application.DTO.Auth
{
    public class VerifyVideoKycRequestDto
    {
        public IFormFile? VideoFile { get; set; }
        public string? KycText { get; set; }
    }
}
