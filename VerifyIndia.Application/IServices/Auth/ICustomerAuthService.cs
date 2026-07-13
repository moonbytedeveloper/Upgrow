using Microsoft.AspNetCore.Http;
using Upgrow.Application.DTO.Auth;
using Upgrow.Domain.Entities;

namespace Upgrow.Application.IServices.Auth
{
    public interface ICustomerAuthService
    {
        Task<HmacValidationResultDto> ValidateHmacRequestAsync(ApiLoginHeaderDto headerDto, ApiLoginRequestDto requestDto);          
        Task<string> GenerateAndStoreOtpAsync(string mobileNo, string tenantIdentifier, CancellationToken cancellationToken = default);
        Task<bool> IsOtpRateLimitedAsync(string mobileNo, int tenantId, CancellationToken cancellationToken = default);        
        Task<Master_Customer> RegisterCustomer(string mobileNo, string tenantIdentifier, CancellationToken cancellationToken = default);      
        
        Task<(string, DateTime)> GenerateAndStoreKycChallengeAsync(string customerUuid, string customerName, CancellationToken cancellationToken = default);
        Task<(bool IsSuccess, string Message)> VerifyVideoKycAsync(string customerUuid, string kycText, string? videoFileUrl, CancellationToken cancellationToken = default);
    }
}
