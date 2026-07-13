using VerifyIndia.Application.DTO.Auth;
using VerifyIndia.Application.DTO.Customer;
using VerifyIndia.Application.DTO.Master;

namespace VerifyIndia.Application.IServices.Master
{
    public interface IMasterPolicyService
    {
        Task<List<PolicyListDto>> GetPolicyListForRegistrationAsync();

        Task SaveConsentAsync(CustomerConsentDto consentDto);
           
        Task<string> VerifyPolicyContent(PolicyConsentItemDto content);
       
        Task<List<PolicyListDto>> GetActivePrivacyAndTermsAsync();
        Task<MasterPolicyDto?> GetByUuidAsync(string uuid);
        Task UpdatePolicyVersionAsync(string selectedPolicyUuid, string bumpType, DateTime effectiveFromDate, string? newPolicyContent = null, int? newSequenceNo = null);

        Task<PolicyListDto?> GetByCodeAsync(string code);
    }

}
