using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities
{
    public class CustomerCreditMaster : BaseEntity
    {
        public string CustomerUUID { get; set; }
            = string.Empty;

        public decimal OpeningBalance { get; set; }

        public decimal CurrentBalance { get; set; }

        public decimal TotalCreditsPurchased
        { get; set; }

        public decimal TotalCreditsConsumed
        { get; set; }

        public DateTimeOffset?
            LastTransactionAt
        { get; set; }
    }
}
