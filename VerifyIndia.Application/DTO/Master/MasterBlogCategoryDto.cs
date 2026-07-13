using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTOs.Master
{
    public class MasterBlogCategoryDto
    {
        public string UUID { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
