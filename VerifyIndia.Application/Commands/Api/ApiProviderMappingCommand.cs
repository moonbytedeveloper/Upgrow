using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndia.Application.Commands.Api
{
    public class ApiProviderMappingCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        [Required(ErrorMessage = "Required!")]

        public string? ProviderUUID { get; set; } = null!;
        public decimal? Priority { get; set; }
        public string? ApiUUID { get; set; }

        public bool IsActive { get; set; }
    }
}
