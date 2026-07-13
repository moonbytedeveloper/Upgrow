using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Transaction
{
    public class ConsentPortalDto
    {
        /// <summary>
        /// Consent Token
        /// </summary>
        public string ConsentId { get; set; }
            = string.Empty;

        public string TransactionUUID { get; set; }
            = string.Empty;

        public string ConsentStatus { get; set; }
            = string.Empty;

        public DateTimeOffset ConsentDateTime { get; set; }

        public string VerifierName { get; set; }
            = string.Empty;

        public string ConsentNotice { get; set; }
            = string.Empty;

        public DateTimeOffset?
            ConsentExpiresAt
        { get; set; }

        public List<ConsentDocumentDto>
            Documents
        { get; set; }
            = new();
    }

    public class ConsentDocumentDto
    {

        public string DocumentName { get; set; }
            = string.Empty;

        public string DocumentNumber { get; set; }
            = string.Empty;

        public bool IsConsentBased { get; set; } = true;
    }
}