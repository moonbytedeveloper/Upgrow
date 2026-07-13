using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.WL.Master
{
    public class WLWhatsappCredentialDto
    {
        public string UUID { get; set; } = null!;
        public string ApiKey { get; set; } = null!;
        public string SecretKey { get; set; } = null!;
        public string MobileNumber { get; set; } = null!;
        public string SenderName { get; set; } = null!;
        public bool IsActive { get; set; }

    }
}
