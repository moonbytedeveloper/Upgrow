using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Transaction
{
    public class GenerateConsentLinkRequest
    {
        public string TransactionUUID { get; set; }
            = string.Empty;
    }
}
