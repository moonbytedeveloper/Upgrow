using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO.Customer;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTO.WL;
using Upgrow.Domain.Entities;

namespace Upgrow.Application.IServices.Master
{
    public interface IMasterCustomerService : IMasterService<MasterCustomerDto, MasterCustomerCommand>
    {
        Task<Master_Customer?> GetByMobileAndClientIdentifierAsync(string mobile, string clientId);
        Task<Master_Customer?> GetByMobileAsync(string mobile);
        Task<Master_Customer?> GetCustomerByUUID(string UUID);        
        Task<Master_Customer> UpdateCustomerAsync(Master_Customer customer, bool saveChanges = true);
        Task<List<MasterCustomerDto>> GetAllAsync(Expression<Func<Master_Customer, bool>> predicate);
        Task ConvertToAgentAsync(string uuid, string userId);

        Task ConvertToAgentHeadAsync(string uuid, string userId);

        Task<Master_Customer?> GetReferralAgentAsync(string mobile);

        Task<Master_Customer> CreateCustomerAsync(string mobile);

        Task<Master_Customer?> GetByAadhaarHashAsync(string aadhaarHash);
    }
}
