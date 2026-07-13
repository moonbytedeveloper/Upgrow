using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities.Registration;

namespace VerifyIndia.Domain.IRepositories.Registration
{
    public interface ICustomerEmailVerificationRepository
    {
        Task<CustomerEmailVerification?>
        GetLatestAsync(
            string customerUuid);

        Task AddAsync(
            CustomerEmailVerification entity);

        Task UpdateAsync(
            CustomerEmailVerification entity);

        Task DeactivateActiveAsync(
            string customerUuid);
    }
}
