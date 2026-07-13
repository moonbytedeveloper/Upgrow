using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;

namespace Upgrow.Application.Commands.AIX
{
    public class ApixVersionCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? ApiCategoryUUID { get; set; }
        [Required(ErrorMessage = "Required!")]
        public string? ApiXCategoryUUID { get; set; }
        public string? VersionNo { get; set; }
        public string? ApiMethod { get; set; }
        public string? ApiPath { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsRequestBodyRequired { get; set; }
        public int SequenceNo { get; set; }
        public bool IsActive { get; set; }
    }
}
