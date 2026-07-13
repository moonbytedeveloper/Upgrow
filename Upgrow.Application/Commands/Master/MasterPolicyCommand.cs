using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Commands.Master
{
    public class MasterPolicyCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Policy Content is required")]
        public string PolicyContent { get; set; } = null!;

        [Required(ErrorMessage = "Version is required (e.g., fab fa-instagram)")]
        public string Version { get; set; } = null!;

        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Sequence No is required")]
        public int SequenceNo { get; set; }

        [Required(ErrorMessage = "Version Type  is required")]
        public string? VersionType { get; set; }

        [Required(ErrorMessage = "Required!")]
        public DateTime? FromDate { get; set; }
        
        public DateTime? ToDate { get; set; }
        public bool IsActive { get; set; }
    }
}
