using Upgrow.Domain.Entities;

namespace Upgrow.Domain.IRepositories.Master
{
    public interface ICustomerVideoKycRepository : IMasterRepository<CustomerVideoKYC>    
    {
        Task<CustomerVideoKYC?> GetLatestByCustomerUuidAsync(string customerUuid);
    }
}
