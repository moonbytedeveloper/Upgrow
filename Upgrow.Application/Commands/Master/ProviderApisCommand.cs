using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Commands.Master
{
    public class ProviderApisCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "API UUID is required!")]
        [StringLength(50, ErrorMessage = "API UUID cannot exceed 50 characters")]
        public string ApiUUID { get; set; } = null!;

        [Required(ErrorMessage = "Provider UUID is required!")]
        [StringLength(50, ErrorMessage = "Provider UUID cannot exceed 50 characters")]
        public string ProviderUUID { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}
