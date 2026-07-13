using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Transaction
{
    public class InitiateTransactionRequest
    {
        public string PaymentMode { get; set; }
            = string.Empty;
    }
}
