using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities.Registration;

namespace VerifyIndia.Domain.IRepositories.Registration
{
    public interface ICustomerRegDocumentRepository
    {
        Task AddAsync(
            CustomerRegDocument entity, bool saveChanges = true);

        Task<CustomerRegDocument?> GetByUUIDAsync(
                string uuid);

        Task<CustomerRegDocument?>
            GetLatestByCustomerAsync(
                string customerUuid,
                string recordCategory);

        Task UpdateAsync(
            CustomerRegDocument entity);

        Task<CustomerRegDocument?>
            GetAadhaarDocumentAsync(
                string customerUuid);
    }
}
