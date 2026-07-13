using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;

namespace Upgrow.Application.Commands.WL.Master
{
    public class WLMasterNomenClatureCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Module Key is required")]
        [RegularExpression(@"^[A-Za-z0-9_]+$", ErrorMessage = "Module Key can only contain letters, numbers, and underscores")]
        public string? ModuleKey { get; set; }

        public bool? IsIncludeYear { get; set; } = false;

        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        public string? Prefix { get; set; }

        [Required(ErrorMessage = "Start Number is required")]
        [Range(1, 999999999999999999, ErrorMessage = "Start Number must be greater than 0")]
        public decimal? StartNo { get; set; }
        public string? FinancialYearUUID { get; set; }
        public List<SelectListItem> YearList { get; set; } = new();
        public List<SelectListItem> ModuleKeyList { get; set; } = new List<SelectListItem>
         {
          new SelectListItem { Value = "", Text = "" },
          new SelectListItem { Value = "Employee", Text = "Employee" },
          new SelectListItem { Value = "Agent", Text = "Agent" },
          new SelectListItem { Value = "Distributor", Text = "Distributor" },
          new SelectListItem { Value = "Invoice", Text = "Invoice" }
         };

        [Required(ErrorMessage = "Number of Digits is required")]
        [Range(1, 10, ErrorMessage = "Number of Digits must be between 1 and 10")]
        public int? NumberOfDigits { get; set; }
        public bool? IsIncremental { get; set; }
        public bool IsActive { get; set; }
    }
}
