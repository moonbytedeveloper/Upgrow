using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Api
{
    public class ApiProviderDto
    {
        public string UUID { get; set; } = null!;
        public string ProviderName { get; set; } = null!;
        public string Code { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
