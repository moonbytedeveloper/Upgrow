using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities.Registration;

namespace Upgrow.Domain.IRepositories.Registration
{
    public interface ICustomerBusinessSessionRepository
    {
        Task AddAsync(
            CustomerBusinessSession session);

        Task UpdateAsync(
            CustomerBusinessSession session);

        Task<CustomerBusinessSession?>
            GetByUUIDAsync(
                string uuid);

        Task<CustomerBusinessSession?>
            GetByOrderIdAsync(
                string orderId);

        Task<CustomerBusinessSession?>
            GetActiveByCustomerUUIDAsync(
                string customerUuid);

        Task DeactivateCustomerSessionsAsync(
            string customerUuid);
    }
}
