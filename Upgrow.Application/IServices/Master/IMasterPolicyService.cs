using Upgrow.Application.DTO.Auth;
using Upgrow.Application.DTO.Customer;
using Upgrow.Application.DTO.Master;

namespace Upgrow.Application.IServices.Master
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
