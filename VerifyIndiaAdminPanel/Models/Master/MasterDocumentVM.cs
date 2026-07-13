using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndiaAdminPanel.Models.Master
{
    public class MasterDocumentVM
    {
        public MasterDocumentCommand Command { get; set; } = new();
        //public string? UUID { get; set; }

        //[Required(ErrorMessage = "Required!")]
        //[RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        //public string Title { get; set; } = null!;

        //[Required(ErrorMessage = "Required!")]
        //public string? FileType { get; set; }

        //public string? Path { get; set; }

        //public bool IsActive { get; set; } = true;

        //// Image Field
        //public IFormFile? Image { get; set; }
        [NotMapped]
        public string? ImageUrlwithdomain { get; set; }

        // Dropdown list for FileType
        public List<SelectListItem> FileTypeList { get; set; } = new List<SelectListItem>();
    }
}
