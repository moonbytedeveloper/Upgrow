using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Transaction
{
    public class GetConsentStatusResponseDto
    {
        public string TransactionUUID { get; set; }
            = string.Empty;

        public bool ConsentRequired { get; set; }

        public string ConsentStatus { get; set; }
            = string.Empty;

        public bool IsCompleted { get; set; }

        public string? Message { get; set; }

    }
}
