using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities.WL
{
    public class WL_PasswordPolicy :TenantEntity
    {
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
        public bool? UpperCase { get; set; }
        public bool? LowerCase { get; set; }
        public bool? AllowDigit { get; set; }
        public bool? AllowSpecialChar { get; set; }
        public string? SpecialCharacters { get; set; }
    }
}
