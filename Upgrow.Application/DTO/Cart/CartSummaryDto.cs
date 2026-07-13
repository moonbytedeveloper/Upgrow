using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Cart
{
    public class CartSummaryDto
    {
        public string CartUUID { get; set; }
            = string.Empty;

        public string CartNo { get; set; }
            = string.Empty;

        public string AuthFor { get; set; } = string.Empty;
        public int TotalApis { get; set; }

        public decimal BaseCreditsTotal { get; set; }

        public decimal ConsentCreditsTotal { get; set; }

        public decimal PayableBaseAmount { get; set; }
        public bool IsConsentRequired { get; set; }

        public bool IsConsentProvided { get; set; }
        public string? ConsentDocNo { get; set; } = string.Empty;

        public string? ConsentMobileNo { get; set; } = string.Empty;
        public List<CartSummaryItemDto> Items { get; set; }
            = new();

        
    }

    public class CartSummaryItemDto
    {
        public string ApiUUID { get; set; }
            = string.Empty;

        public string ApiName { get; set; }
            = string.Empty;

        public decimal ApiCharge { get; set; }

        public bool IsConsentBased { get; set; }
    }
}
