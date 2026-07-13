using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Commands.Master
{
    public class MasterCMSCommand : IMasterCommand
    {
        public string? UUID { get; set; } = null!;

        [Required(ErrorMessage = "Required!")]        
        public string PageTitle { get; set; } = null!;

        //[Required(ErrorMessage = "Required!")]
        public string? UploadImage { get; set; } = null!;

        [Required(ErrorMessage = "Required!")]
        public string Description { get; set; }

        // Image Upload File
        public IFormFile? ImageFile { get; set; }
        // For Dropdown Options
        public List<(string UUID, string PageTitle)> CmsOptions { get; set; } = new();
        public bool IsActive { get; set; }
        [NotMapped]
        public string? ImageUrlwithdomain { get; set; }
    }
}
