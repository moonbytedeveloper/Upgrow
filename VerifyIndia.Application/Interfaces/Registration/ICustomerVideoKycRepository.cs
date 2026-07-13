using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities.Registration;

namespace Upgrow.Application.Interfaces.Registration
{
    public interface ICustomerVideoKycRepository
    {
        Task AddAsync(
            CustomerVideoKyc entity);

        Task UpdateAsync(
            CustomerVideoKyc entity);

        Task<CustomerVideoKyc?> GetLatestByCustomerUUIDAsync(
                string customerUuid);

        Task<bool> IsVerifiedAsync(
                string customerUuid);

        Task<CustomerVideoKyc?> GetLatestChallengeAsync(
            string customerUuid);

        Task DeactivateActiveChallengesAsync(
            string customerUuid);

    }
}
