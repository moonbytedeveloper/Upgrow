using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Master
{
    public class TrustedPartnersDto
    {
        public string? UUID { get; set; }

        public string? Name { get; set; }

        public string? IconImage { get; set; }

        public bool IsActive { get; set; }
    }
}
