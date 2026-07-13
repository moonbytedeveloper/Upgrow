using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndia.Application.Commands.Support
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
