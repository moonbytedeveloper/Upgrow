using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.Api;


namespace Upgrow.Application.Commands.Api
{
    public class ApiProviderCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed")]
        public string ProviderName { get; set; } = null!;

        public string? Code { get; set; }
        public bool IsActive { get; set; }
    }
}
