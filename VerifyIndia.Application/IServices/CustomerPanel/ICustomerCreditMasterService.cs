using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.CustomerPanel;

namespace VerifyIndia.Application.IServices.CustomerPanel
{
    public interface ICustomerCreditMasterService
    {
        Task<decimal?> GetCurrentBalanceAsync(string customerUuid);
    }
}
