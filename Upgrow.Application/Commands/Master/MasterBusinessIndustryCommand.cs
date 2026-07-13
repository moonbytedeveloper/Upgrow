using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.DropDown;

namespace Upgrow.Application.Commands.Master
{
    public class MasterBusinessIndustryCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        [StringLength(50, ErrorMessage = "Title cannot exceed 50 characters")]
        public string? Title { get; set; }
        public bool IsActive { get; set; }

    }
}
