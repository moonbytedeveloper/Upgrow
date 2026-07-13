using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;

namespace Upgrow.Domain.Models
{
    public class TransactionProcessContext
    {
        public Transaction Transaction { get; set; } = default!;

        public IList<TransactionDetail> Details { get; set; }
            = new List<TransactionDetail>();

        public CancellationToken CancellationToken { get; set; }
    }
}
