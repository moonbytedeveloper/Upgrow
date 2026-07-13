using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Domain.IRepositories.Master
{
    public interface IMasterPolicyRepository
    {
        Task<List<Master_Policy>> GetPolicyListAsync(List<string> codes);
        Task<Master_Policy?> GetByUuidAsync(string uuid);
        Task<Master_Policy?> GetByCodeAndVersionAsync(string code, string version);
        Task<List<Master_Policy>> GetActivePrivacyAndTermsAsync();
        Task AddAsync(Master_Policy entity);
        Task UpdateAsync(Master_Policy entity);
        Task<Master_Policy?> GetByCodeLatestAsync(string code);
    }
}
