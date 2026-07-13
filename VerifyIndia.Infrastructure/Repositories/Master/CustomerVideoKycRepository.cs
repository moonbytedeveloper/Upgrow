using Microsoft.EntityFrameworkCore;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories.Master;

namespace Upgrow.Infrastructure.Repositories.Master
{
    public class CustomerVideoKycRepository : MasterRepositoryBase<CustomerVideoKYC>,ICustomerVideoKycRepository
    {
        public CustomerVideoKycRepository(AppDbContext context) :base(context)
        {
           
        }        

        public async Task<CustomerVideoKYC?> GetLatestByCustomerUuidAsync(string customerUuid)
        {
            return await _context.CustomerVideoKYC
                .OrderByDescending(x => x.TimeStamp)
                .ThenByDescending(x => x.Id)
                .FirstOrDefaultAsync(x => x.CustomerUUID == customerUuid);
        }
    }
}
