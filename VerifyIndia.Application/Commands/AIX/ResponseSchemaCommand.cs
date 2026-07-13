using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndia.Application.Commands.AIX
{
    public class ResponseSchemaCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? ApiXVersionUUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? ShortDescription { get; set; }
        public string? SchemaType { get; set; }
        public bool IsSchemaForSuccess { get; set; }
        public bool IsActive { get; set; }
    }
}
