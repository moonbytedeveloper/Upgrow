using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.Commands.Master
{
    public class MasterSocialMediaCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Platform name is required")]
        public string PlatformName { get; set; } = null!;

        [Required(ErrorMessage = "Profile URL is required")]
        public string ProfileURL { get; set; } = null!;

        [Required(ErrorMessage = "Icon class is required (e.g., fab fa-instagram)")]
        public string IconURL { get; set; } = null!;

        [Required(ErrorMessage = "Display order is required")]
        [Range(0, 9999, ErrorMessage = "Display order must be between 0 and 9999")]
        public decimal? DisplayOrder { get; set; }

        public bool IsActive { get; set; }
    }
}
