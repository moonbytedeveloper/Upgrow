using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;

namespace Upgrow.Application.Commands.AIX
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
