using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.CustomerPanel;

namespace Upgrow.Application.IServices.CustomerPanel
{
    public interface ICustomerCreditMasterService
    {
        Task<decimal?> GetCurrentBalanceAsync(string customerUuid);
    }
}
