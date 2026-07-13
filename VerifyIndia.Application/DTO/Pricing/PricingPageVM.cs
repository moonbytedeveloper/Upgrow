using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Pricing
{
    public class PricingPageVM
    {
        // =====================================
        // FILTERS
        // =====================================

        [Required]
        public string PriceType { get; set; }

        [Required]
        public string ProviderUUID { get; set; }

        [Required]
        public DateOnly? EffectiveDate { get; set; }

        public string? SearchApi { get; set; }

        public string? CategoryUUID { get; set; }
        public string? PlatformOwnerUUID { get; set; }

        public bool SelectAll { get; set; }

        public decimal? BulkPrice { get; set; }



        // =====================================
        // DROPDOWNS
        // =====================================
        public List<SelectListItem> PriceTypeList { get; set; }
            = new();

        public List<SelectListItem> ProviderList { get; set; }
            = new();

        public List<SelectListItem> CategoryList { get; set; }
            = new();

        // =====================================
        // GRID DATA
        // =====================================

        public List<PricingGroupDto> PricingGroups { get; set; }
            = new();
    }
}
