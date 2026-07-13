using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO
{
    public class PinnedServiceDto
    {
        public string UUID { get; set; } = null!;
        public string ApiUUID { get; set; } = null!;
        public string CustomerUUID { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
