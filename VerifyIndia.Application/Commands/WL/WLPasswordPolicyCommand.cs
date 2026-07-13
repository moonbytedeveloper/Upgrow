using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndia.Application.Commands.WL
{
    public class WLPasswordPolicyCommand :IMasterCommand
    {
        public string? UUID { get; set; }
        public bool IsActive { get; set; }
        public int? MinLength { get; set; }
        public bool? UpperCase { get; set; }
        public bool? LowerCase { get; set; }
        public bool? AllowDigit { get; set; }
        public bool? AllowSpecialChar { get; set; }
        public string? SpecialCharacters { get; set; }
    }
}
