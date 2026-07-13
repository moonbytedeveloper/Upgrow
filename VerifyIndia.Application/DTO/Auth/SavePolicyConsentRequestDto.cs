using Microsoft.AspNetCore.Http;

namespace Upgrow.Application.DTO.Auth
{
    public class SavePolicyConsentRequestDto
    {
        public List<PolicyConsentItemDto> Policies { get; set; } = new();
        public bool IsConsentGiven { get; set; }        
        public string Platform { get; set; } = string.Empty;       
        public string LatLong { get; set; } = string.Empty;
        public string SignId { get; set; } = string.Empty;

    }

    public class PolicyConsentItemDto
    {
        public string PolicyUuid { get; set; } = string.Empty;
        public string PolicyContent { get; set; } = string.Empty;
        public string PolicyVersion { get; set; } = string.Empty;     
        
    }
}
