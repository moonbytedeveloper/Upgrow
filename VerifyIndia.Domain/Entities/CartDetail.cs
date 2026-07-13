using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class CartDetail : BaseEntity
    {
        public string ApiUUID { get; set; }

        public string CartUUID { get; set; }

        public string PricingUUID { get; set; }

        public decimal ApiCharge { get; set; }

        public string? ReqPayload { get; set; }

        public bool IsConsentBased { get; set; }

        public bool IsMultipleEndPoint { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public virtual Master_Cart Cart { get; set; }
    }
}
