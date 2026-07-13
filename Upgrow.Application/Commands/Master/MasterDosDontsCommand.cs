using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Commands.Master
{
    public class MasterDosDontsCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(500, ErrorMessage = "Message cannot exceed 500 characters")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Please select DOS/DONTS")]
        public bool IsDos { get; set; }

        [Range(0, 9999, ErrorMessage = "Sequence number must be between 0 and 9999")]
        public decimal SequenceNo { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
