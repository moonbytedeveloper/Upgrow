using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;

namespace Upgrow.Application.Commands.WL.Master
{
    public class WLMasterDocumentCommand : IMasterCommand
    {
        public string? UUID { get; set; }


        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        public string Title { get; set; } = null!;

        public string? FileType { get; set; }

        [NotMapped]
        public string? ImageUrlwithdomain { get; set; }
        public string? Path { get; set; }

        public bool IsActive { get; set; }

        public IFormFile? Image { get; set; }
    }
}
