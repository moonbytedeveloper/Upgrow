using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities.Registration;

namespace Upgrow.Application.Interfaces.Registration
{
    public interface ICustomerBusinessRepository
    {
        Task<CustomerOrganization?>
            GetByUUIDAsync(
                string uuid);

        Task<CustomerOrganization?>
            GetByCustomerUUIDAsync(
                string customerUuid);

        Task<bool>
            ExistsAsync(
                string customerUuid);

        Task AddAsync(
            CustomerOrganization entity);

        Task UpdateAsync(
            CustomerOrganization entity);

        Task<CustomerOrganization?> GetRequiredByCustomerUUIDAsync(
            string customerUuid);
    }
}
