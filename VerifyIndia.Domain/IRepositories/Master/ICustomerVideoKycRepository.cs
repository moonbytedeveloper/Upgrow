using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Domain.IRepositories.Master
{
    public interface ICustomerVideoKycRepository : IMasterRepository<CustomerVideoKYC>    
    {
        Task<CustomerVideoKYC?> GetLatestByCustomerUuidAsync(string customerUuid);
    }
}
