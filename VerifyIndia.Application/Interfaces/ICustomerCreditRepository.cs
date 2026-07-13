using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;

namespace Upgrow.Application.Interfaces
{
    public interface ICustomerCreditRepository
    {
        Task<CustomerCreditMaster?>
            GetByCustomerUuidAsync(
                string customerUuid);

        Task AddCreditMasterAsync(
            CustomerCreditMaster entity);

        Task AddCreditLedgerAsync(
            CustomerCreditLedger entity);

        Task AddDebitLedgerAsync(
            CustomerDebitLedger entity);

        Task UpdateCreditMasterAsync(
            CustomerCreditMaster entity);

        Task SaveChangesAsync();

        Task<bool> HasSufficientCreditsAsync(
            string customerUuid,
            decimal requiredAmount);

        Task<decimal?> GetCurrentBalanceAsync(string customerUuid);
    }
}
