using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Commands.Master
{
    public class ServiceCategoryCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Category Name is required!")]
        [StringLength(150, ErrorMessage = "Category Name cannot exceed 150 characters")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Icon class is required!")]
        [StringLength(200, ErrorMessage = "Icon class cannot exceed 200 characters")]
        [RegularExpression(@"^(fas|fab|far|fal)\s+fa-[a-z0-9-]+(\s+(fas|fab|far|fal)\s+fa-[a-z0-9-]+)*$",
            ErrorMessage = "Please use valid Font Awesome icon class (e.g., fab fa-instagram)")]
        public string IconImage { get; set; }

        [Required(ErrorMessage = "Description is required!")]
        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters")]
        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
