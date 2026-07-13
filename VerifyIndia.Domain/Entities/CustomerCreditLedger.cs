using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities
{
    public class CustomerCreditLedger : BaseEntity
    {
        public string CustomerUUID { get; set; }
            = string.Empty;

        public string? TransactionUUID { get; set; }

        public string CreditType { get; set; }
            = string.Empty;

        public decimal CreditsAdded { get; set; }

        public decimal BalanceBefore { get; set; }

        public decimal BalanceAfter { get; set; }

        public string? Remarks { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
