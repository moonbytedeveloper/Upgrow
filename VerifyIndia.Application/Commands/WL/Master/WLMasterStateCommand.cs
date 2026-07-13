using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Common;

namespace VerifyIndia.Application.Commands.Master
{
    public class WLMasterStateCommand : IMasterCommand 
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]

        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Required!")]
        public string? CountryUUID { get; set; }
        public string? ShortTitle { get; set; }

        public bool IsActive { get; set; }
       
    }
}
