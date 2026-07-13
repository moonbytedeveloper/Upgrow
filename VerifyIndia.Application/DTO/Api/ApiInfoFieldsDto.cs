using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Api
{
    public class ApiInfoFieldsDto
    {
        public string UUID { get; set; } = null!;
        public string InfoSectionUUID { get; set; } = null!;
        public string? Title { get; set; }
        public int Sequence { get; set; }
        public bool IsActive { get; set; }
 
    }
}
