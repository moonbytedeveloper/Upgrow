using Microsoft.AspNetCore.Http;

namespace Upgrow.Application.DTO.Auth
{
    public class VerifyVideoKycRequestDto
    {
        public IFormFile? VideoFile { get; set; }
        public string? KycText { get; set; }
    }
}
