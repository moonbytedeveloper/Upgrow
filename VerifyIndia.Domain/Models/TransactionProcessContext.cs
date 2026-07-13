using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Domain.Models
{
    public class TransactionProcessContext
    {
        public Transaction Transaction { get; set; } = default!;

        public IList<TransactionDetail> Details { get; set; }
            = new List<TransactionDetail>();

        public CancellationToken CancellationToken { get; set; }
    }
}
