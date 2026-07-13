using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Commands.Master
{
    public class MasterOfferCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        public string? Name { get; set; }
        public string? OfferCode { get; set; }
        public string? Description { get; set; }
        public string? OfferType { get; set; }
        public decimal? OfferValue { get; set; }
        public decimal? MinOrderAmount { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public int? MaxUsageCount { get; set; }
        public int? PerUserUsageCount { get; set; }
        public bool? IsFirstTransactionOnly { get; set; } = false;
        public bool? IsApplicableForAllUsers { get; set; } = false;
        public bool? IsActive { get; set; }

        public List<SelectListItem> OfferTypeList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "FixedDiscount", Text = "Fixed Discount" },
            new SelectListItem { Value = "PercentageDiscount", Text = "Percentage Discount" },
            new SelectListItem { Value = "Cashback", Text = "Cashback" },
            new SelectListItem { Value = "FreeVerification", Text = "Free Verification" },
            new SelectListItem { Value = "WalletRecharge", Text = "Wallet Recharge Offer" }
        };
    }
}
