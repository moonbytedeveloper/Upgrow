using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities.Registration
{
    public class Master_VerificationFee : BaseEntity
    {

        public string VerificationType { get; set; }
            = string.Empty;

        public decimal Amount { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

    }
}
