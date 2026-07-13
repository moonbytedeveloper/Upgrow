using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities
{
    public class Master_Pricing : BaseEntity
    {
        public string APIUUID { get; set; }

        public string? SellerTenantId { get; set; }

        public string? ProviderUUID { get; set; }

        public string BuyerType { get; set; }

        public string? BuyerUUID { get; set; }

        public decimal BaseAmount { get; set; }

        // MB (optional) : Required only while adding price of provider -> MB
        public decimal? BaseAmountMB { get; set; }

        public DateOnly EffectiveFrom { get; set; }
        public DateOnly? EffectiveTo { get; set; }

    }
}
