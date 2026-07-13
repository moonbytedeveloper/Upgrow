using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;

namespace Upgrow.Application.Commands.Support
{
    public class SupportTicketCategoryCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Required!")]
        public string? DesignationUUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string? UserType { get; set; }

        public bool IsActive { get; set; }
    }
}
