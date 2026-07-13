using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class Master_Offer : BaseEntity
    {
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
        public bool? IsFirstTransactionOnly { get; set; }
        public bool? IsApplicableForAllUsers { get; set; }
    }
}
