using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;

namespace Upgrow.Application.Commands.Api
{
    public class ApiInfoFieldsCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        [Required]
        public string? InfoSectionUUID { get; set; }
       
        public string? ApiUUID { get; set; }
        [Required(ErrorMessage = "Title is required!")]
        [StringLength(50, ErrorMessage = "Title cannot exceed 50 characters")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Sequence is required!")]
        public int Sequence { get; set; }
        public bool IsActive { get; set; }
    }
}
