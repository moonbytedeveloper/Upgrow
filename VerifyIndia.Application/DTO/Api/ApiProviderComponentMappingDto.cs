using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Api
{
    public class ApiProviderComponentMappingDto
    {
        public string? UUID { get; set; }
        public string? ApiUUID { get; set; }
        public string? ComponentUUID { get; set; }
        public bool IsActive { get; set; }
        public int Sequence { get; set; }
    }
}
