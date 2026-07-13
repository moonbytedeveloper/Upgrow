using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.WL.Master
{
    public class WLSMSCredentialDto
    {
        public string UUID { get; set; } = null!;
        public string ApiKey { get; set; } = null!;
        public string SenderId { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
