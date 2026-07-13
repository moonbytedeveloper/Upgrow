using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Commands.Master
{
    public class MasterApiCategoryCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Category Name is required!")]
        [StringLength(150, ErrorMessage = "Category Name cannot exceed 150 characters")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Sequence No is required!")]
        public decimal? SequenceNo { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }
    }
}
