using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndia.Application.Commands.Api
{
    public class ApiProviderComponentMappingCommand :IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string? ApiUUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? ComponentUUID { get; set; }
        public bool IsActive { get; set; }
        public int Sequence { get; set; }
    }
}
