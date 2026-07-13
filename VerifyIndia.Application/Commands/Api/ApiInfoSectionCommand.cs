using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndia.Application.Commands.Api
{
    public class ApiInfoSectionCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        [Required]
        public string? ApiUUID { get; set; }
        [Required(ErrorMessage = "Title is required!")]
        [StringLength(50, ErrorMessage = "Title cannot exceed 150 characters")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Description is required!")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        public bool IsActive { get; set; }
        [Required]
        public int Sequence { get; set; }
    }
}
