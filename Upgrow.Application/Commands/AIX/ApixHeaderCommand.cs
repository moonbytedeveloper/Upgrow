using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;

namespace Upgrow.Application.Commands.AIX
{
    public class ApixHeaderCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        [Required(ErrorMessage = "ApiXVersionUUID is required!")]
        public string? ApiXVersionUUID { get; set; }
        [Required(ErrorMessage = "Field Name is required!")]
        public string? FieldName { get; set; }
        [Required(ErrorMessage = "Data Type is required!")]
        public string? DataType { get; set; }
        [Required(ErrorMessage = "Field Details is required!")]
        public string? FieldDetails { get; set; }
        public bool IsRequired { get; set; }
        public bool IsActive { get; set; }
    }
}
