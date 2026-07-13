using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Domain.IRepositories.Master
{
    public interface ICustomerConsentRepository
    {
        Task<CustomerConsent?> GetByCustomerAndPolicyAsync(string customerUuid, string policyUuid, string policyVersion);
        Task<CustomerConsent?> GetLatestByCustomerUUIDAsync(
            string customerUuid);
        Task AddAsync(CustomerConsent entity);
        Task UpdateAsync(CustomerConsent entity);
    }
}
