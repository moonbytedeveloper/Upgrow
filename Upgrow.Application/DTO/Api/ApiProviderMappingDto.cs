using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Api
{
    public class ApiProviderMappingDto
    {
        public string UUID { get; set; } = null!;
        public string ProviderUUID { get; set; } = null!;
        public string ApiUUID { get; set; } = null!;
        public decimal? Priority { get; set; }
        public bool IsActive { get; set; }
       
    }
}
