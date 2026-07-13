using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities.Registration;

namespace Upgrow.Domain.IRepositories.Registration
{
    public interface ICustomerAadhaarSessionRepository
    {
        Task AddAsync(CustomerAadhaarSession entity, bool saveChanges = true);

        Task UpdateAsync(CustomerAadhaarSession entity, bool saveChanges = true);

        Task<CustomerAadhaarSession?>
            GetByUUIDAsync(
                string uuid);

        Task<CustomerAadhaarSession?>
    GetActiveByCustomerUUIDAsync(
        string customerUuid);

        Task DeactivateCustomerSessionsAsync(
            string customerUuid);

        Task<bool> HasCompletedPaymentAsync(
            string customerUuid);
    }
}
