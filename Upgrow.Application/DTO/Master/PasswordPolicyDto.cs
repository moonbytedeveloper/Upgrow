using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Master
{
    public class PasswordPolicyDto
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
