using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;

namespace Upgrow.Application.Commands.AIX
{
    public class RequestSchemaCommand : IMasterCommand
       
    {
        public string? UUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? ApiXVersionUUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? RequestType { get; set; }
        public string? RequestJson { get; set; }
        public bool IsActive { get; set; }

    }
}
