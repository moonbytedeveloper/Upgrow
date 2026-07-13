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
    public class MasterBannerCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string MainTitle { get; set; } = null!;

        [Required(ErrorMessage = "Required!")]
        public string SubTitle { get; set; } = null!;
     
        public string? OptionalTitle { get; set; } 

        [Required(ErrorMessage = "Required!")]
        public string ButtonText { get; set; } = null!;
        [Required(ErrorMessage = "Required!")]
        public string ButtonURL { get; set; } = null!;
        public string? BannerImage { get; set; }
        [NotMapped]
        public string? ImageUrlwithdomain { get; set; }
        public decimal? SequenceNo { get; set; }
        public bool IsActive { get; set; }
        //Image Fields
        public IFormFile? Image { get; set; }
    }
}
