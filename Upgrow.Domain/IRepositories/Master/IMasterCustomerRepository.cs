using Upgrow.Domain.Entities;

namespace Upgrow.Domain.IRepositories.Master
{
    public interface IMasterCustomerRepository : IMasterRepository<Master_Customer>
    {
       Task<Master_Customer?> GetByMobileAndClientIdAsync(string mobile, decimal clientId);
       Task<Master_Customer?> GetByMobileAsync(string mobile);
        Task<bool> ExistsByUuidAndClientIdAsync(string customerUuid, string clientId);

        Task<Master_Customer?> GetReferralAgentAsync(string mobile);

        Task<Master_Customer?> GetByAadhaarHashAsync(string aadhaarHash);
    }
}
