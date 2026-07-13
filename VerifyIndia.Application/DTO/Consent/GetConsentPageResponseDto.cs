using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Consent
{
    public class GetConsentPageResponseDto
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

        public DateOnly ConsentDate { get; set; }

        public string ConsentTime { get; set; }
            = string.Empty;

        public string VerifierName { get; set; }
            = string.Empty;

        public List<ConsentDocumentDto>
            Documents
        { get; set; }
            = new();

        public string ConsentNotice { get; set; }
            = string.Empty;

        public string ConfirmationText { get; set; }
            = string.Empty;

        public DateTimeOffset ConsentExpiresAt
        { get; set; }
    }

    public class ConsentDocumentDto
    {
        public string DocumentType { get; set; }
            = string.Empty;

        public string DocumentName { get; set; }
            = string.Empty;

        public string DocumentNumber { get; set; }
            = string.Empty;
    }
}
