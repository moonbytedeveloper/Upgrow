using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndia.Application.Commands.Api
{
    public class ApiEndpointCommand : IMasterCommand
    {
        public string? UUID { get; set; }

         
        public string? ApiUUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^\/?[A-Za-z0-9\/\-\._\{\}]+$", ErrorMessage = "Endpoint URL must not contain spaces and may optionally start with '/'")]
        public string? EndpointUrl { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string? HttpMethod { get; set; }
        public bool IsActive { get; set; }

        public int Sequence { get; set; }
    }
}
