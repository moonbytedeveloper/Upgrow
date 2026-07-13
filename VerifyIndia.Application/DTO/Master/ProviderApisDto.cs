using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Master
{
    public class ProviderApisDto
    {
        public string UUID { get; set; } = null!;
        public string ApiUUID { get; set; } = null!;
        public string ProviderUUID { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
