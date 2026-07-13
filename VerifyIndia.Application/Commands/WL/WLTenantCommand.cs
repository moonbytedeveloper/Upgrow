using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndia.Application.Commands.WL
{
    public class WLTenantCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Identifier  is required")]
        [RegularExpression(@"^[a-z]+$", ErrorMessage = "Identifier must contain only lowercase letters (a-z). No spaces, numbers, uppercase letters or special characters allowed.")]
        public string Identifier { get; set; } = null!;

        [Required(ErrorMessage = "TenantName is required")]
        public string TenantName { get; set; } = null!;

        public bool IsActive { get; set; }

        public bool IsPlatformOwner { get; set; }
    }
}
