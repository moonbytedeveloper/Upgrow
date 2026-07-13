using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndia.Application.Commands.AIX
{
    public class ApiXLanguageContentCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "ApiXVersionUUID is required!")]
        public string? ApiXVersionUUID { get; set; }

        [Required(ErrorMessage = "Language is required!")]
        public string? LanguageUUID { get; set; }

        public string? LanguageContent { get; set; }

        

        public bool IsActive { get; set; }
    }
}
