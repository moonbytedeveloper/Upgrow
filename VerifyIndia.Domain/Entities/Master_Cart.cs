using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities
{
    public class Master_Cart : BaseEntity
    {
        public string CartNo { get; set; }

        public string AuthFor { get; set; }

        public decimal BaseCreditsTotal { get; set; }

        public decimal ConsentCreditsTotal { get; set; }

        public decimal PayableBaseAmount { get; set; }

        public string? ConsentDocNo { get; set; }

        public string? ConsentMobileNo { get; set; }

        public string VerifierUUID { get; set; }

        public int TotalApis { get; set; }

        public string Status { get; set; }

        public string SellerTenantUUID { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public virtual ICollection<CartDetail>
            CartDetails
        { get; set; }
            = new List<CartDetail>();
    }
}
