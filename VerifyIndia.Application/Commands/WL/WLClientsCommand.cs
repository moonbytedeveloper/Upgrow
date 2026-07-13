using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;

namespace Upgrow.Application.Commands.WL
{
    public class WLClientsCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        public string? Name { get; set; }
        public string? IconImage { get; set; }

        [NotMapped]
        public string? ImageUrlwithdomain { get; set; }

        public IFormFile? Image { get; set; }
        public int? TenantId { get; set; }
        public string? TenantName { get; set; }
        public bool IsActive { get; set; }
        public List<SelectListItem> Tenants { get; set; } = new List<SelectListItem>();
    }
}
