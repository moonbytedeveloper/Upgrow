namespace Upgrow.Domain.IRepositories
{
    public interface IConfigurationRepository
    {
        Task<string?> GetFileDomainUrlAsync();
        Task<string?> GetSandboxAccessTokenAsync();
    }
}