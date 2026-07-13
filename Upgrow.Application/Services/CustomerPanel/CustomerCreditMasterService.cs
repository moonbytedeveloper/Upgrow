using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.CustomerPanel;
using Upgrow.Application.Interfaces;
using Upgrow.Application.IServices.CustomerPanel;

namespace Upgrow.Application.Services.CustomerPanel
{
    public class CustomerCreditMasterService :ICustomerCreditMasterService
    {
        private readonly ICustomerCreditRepository _customerCreditRepository;

        public CustomerCreditMasterService(
            ICustomerCreditRepository customerCreditRepository)
        {
            _customerCreditRepository = customerCreditRepository;
        }

        public async Task<decimal?> GetCurrentBalanceAsync(string customerUuid)
        {
            return await _customerCreditRepository
                .GetCurrentBalanceAsync(customerUuid);
        }
    }
}
