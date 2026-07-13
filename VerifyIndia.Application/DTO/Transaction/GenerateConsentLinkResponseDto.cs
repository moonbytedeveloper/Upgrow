using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Transaction
{
    public class GenerateConsentLinkResponseDto
    {
        public string TransactionUUID { get; set; }
            = string.Empty;

        public string ConsentToken { get; set; }
            = string.Empty;

        public string ConsentUrl { get; set; }
            = string.Empty;

        public DateTimeOffset ConsentExpiresAt
        { get; set; }

        public int ConsentExpiresInSeconds
        { get; set; }
    }
}
