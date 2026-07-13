using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class TransactionConsent : BaseEntity
    {
        public string TransactionUUID { get; set; }
            = string.Empty;

        public string ConsentDocNo { get; set; }
            = string.Empty;

        public string ConsentMobileNo { get; set; }
            = string.Empty;

        public string? ConsentToken { get; set; }

        public DateTimeOffset?
            ConsentLinkSentAt
        { get; set; }

        public DateTimeOffset?
            ConsentExpiresAt
        { get; set; }

        public bool IsAadharOTPSent { get; set; }

        public string? AadharReqId { get; set; }

        public bool IsConsentSentOnMobile { get; set; }

        public string ConsentStatus { get; set; }
            = string.Empty;

        public string? ConsentSubmitterName { get; set; }

        public string? ConsentSubmitterLat { get; set; }

        public string? ConsentSubmitterLong { get; set; }

        public string? ConsentSubmitterCurrentCity
        { get; set; }

        public string? ConsentSubmitterDevice
        { get; set; }

        public string? ConsentSubmitterIp
        { get; set; }

        public DateTimeOffset?
            ConsentSubmittedTimestamp
        { get; set; }

        public bool IsExpired { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public virtual Transaction
            Transaction
        { get; set; }
            = null!;
    }
}
