using Upgrow.Shared.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Commands.Master
{
    public class MasterVerificationFeeCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        public string? VerificationType { get; set; }

        public List<SelectListItem> TypeList { get; set; } = VerificationFeeConstants.Types
            .Select(x => new SelectListItem
            {
                Value = x.Value,
                Text = x.Text
            })
            .ToList();

        [Range(0, 9999, ErrorMessage = "Amount cannot exceed 4 digits.")]
        public decimal? Amount { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool? IsActive { get; set; }
    }
}
