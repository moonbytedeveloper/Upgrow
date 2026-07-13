using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Registration
{
    public class AcceptTermsResponseDto
    {
        public string SignId { get; set; } = string.Empty;
        public DateTimeOffset SignedAt { get; set; }

        public string Name { get; set; } = "User";
    }
}
