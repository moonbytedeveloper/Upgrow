using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;

namespace Upgrow.Application.Commands.WL.Master
{
    public class WLMasterFinancialYearCommand:IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Required!")]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "Required!")]
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
