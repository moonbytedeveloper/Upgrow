using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class CustomerDebitLedger : BaseEntity
    {
        public string CustomerUUID { get; set; }
            = string.Empty;

        public string TransactionUUID { get; set; }
            = string.Empty;

        public string DebitType { get; set; }
            = string.Empty;

        public decimal CreditsDebited { get; set; }

        public decimal BalanceBefore { get; set; }

        public decimal BalanceAfter { get; set; }

        public string? Remarks { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
