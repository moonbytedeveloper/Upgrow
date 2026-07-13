using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.Commands.Master
{
    public class MasterCareerCommand :IMasterCommand
    {
        public string? UUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? DepartmentUUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? CountryUUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? StateUUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? CityUUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z\s\.\+\&\/\-\(\)]+$", ErrorMessage = "Only letters are allowed")]
        public string? Name { get; set; }
        public IFormFile? Image { get; set; }
        public string? IconImage { get; set; }

        [NotMapped]
        public string? ImageUrlwithdomain { get; set; }
        public string? Experience { get; set; }
        public int? NumberOfPosition { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string? ShortDescription { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string? LongDescription { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string? SkillsUUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string? JobTypeUUID { get; set; }
        public bool IsActive { get; set; }

    }
}
