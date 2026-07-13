using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Commands.Master
{
    public class WLMasterMenuCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        [StringLength(50, ErrorMessage = "Name cannot be greater than 50 characters please validate the name")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        public string? MenuName { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9\s\-\ _\.]+$", ErrorMessage = "Invalid icon format")]
        public string? MenuIcon { get; set; }

        [Required(ErrorMessage = "Required!")]
        public decimal? MenuLevel { get; set; }
        public string? MainParentUUID { get; set; }
        public string? SubParentUUID { get; set; }
        public string? PermissionUUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? Url { get; set; }
        public bool? IsParent { get; set; }
        public bool? IsActive { get; set; }
        [Required(ErrorMessage = "Required!")]
        public decimal? Sequence { get; set; }
    }
}
