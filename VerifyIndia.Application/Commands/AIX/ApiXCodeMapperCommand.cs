using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;

namespace Upgrow.Application.Commands.AIX
{
    public class ApiXCodeMapperCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        [Required(ErrorMessage = "ApiXVersionUUID is required!")]
        public string? ApiXVersionUUID { get; set; }
       
        public string? ResponseSchemaUUID { get; set; }
       
        public string? StatusUUID { get; set; }
       
        public string? CodeExampleUUID { get; set; }
        public string? ResponseJson { get; set; }
        public string? ShortDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
