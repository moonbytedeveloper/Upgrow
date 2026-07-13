using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Api
{
    public class ApiInfoSectionDto
    {
        public string UUID { get; set; } = null!;
        public string ApiUUID { get; set; } = null!;
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int Sequence { get; set; }
        public bool IsActive { get; set; }
 
    }
}
