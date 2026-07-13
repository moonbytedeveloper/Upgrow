using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class TransactionBridgeLog : BaseEntity
    {
        public string TransactionUUID { get; set; }
            = string.Empty;

        public string TransactionDetailUUID { get; set; }
            = string.Empty;

        public string VerificationCode { get; set; }
            = string.Empty;

        public string ProviderCode { get; set; }
            = string.Empty;

        public int AttemptNo { get; set; }

        public string? RequestPayload { get; set; }

        public int ResponseStatusCode { get; set; }

        public string? ResponseMessage { get; set; }

        public string? PrimaryIdentifier { get; set; }

        public string? ProviderReferenceNo { get; set; }

        public DateTimeOffset RequestDateTime { get; set; }

        public DateTimeOffset ResponseDateTime { get; set; }

        public decimal LoadTime { get; set; }

        public bool IsSuccess { get; set; }

        public virtual Transaction Transaction
        { get; set; } = null!;

        public virtual TransactionDetail TransactionDetail
        { get; set; } = null!;
    }
}
