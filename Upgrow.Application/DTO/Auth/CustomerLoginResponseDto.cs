using Upgrow.Application.DTO.Customer;
using Upgrow.Application.DTO.DropDown;
using Upgrow.Application.DTO.Master;

namespace Upgrow.Application.DTO.Auth
{
    public class RegistrationModel
    {
        public TokenResponseDto? Token { get; set; }
        public  ChallengeResponseDto Challenge { get; set; } = new();
        public MobileotpResponseDto MobileOtp { get; set; } = new();

        public CustomerDtoForApi? Customer { get; set; }
        public List<MasterDropDownDto>? States { get; set; } = new();
        public List<MasterDropDownDto>? Cities { get; set; } = new();
        public List<MasterDropDownDto>? BusinessCategories { get; set; } = new();
        public List<GetDosDontsDto> Dos { get; set; } = new();
        public List<GetDosDontsDto> Donts { get; set; } = new();
        public List<PolicyListDto> Policies { get; set; } = new();
    }
    public class ChallengeResponseDto
    {
        public string? ChallengeText { get; set; }
        public DateTime? ChallengeExpiresAtUtc { get; set; }
    }
    public class MobileotpResponseDto
    {
        public string? Otp { get; set; }
        public int? ExpiresInMinutes { get; set; }
    }
    public class TokenResponseDto
    {
        public string? AccessToken { get; set; }
        public DateTime? AccessTokenExpiresAtUtc { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiresAtUtc { get; set; }
    }
}
