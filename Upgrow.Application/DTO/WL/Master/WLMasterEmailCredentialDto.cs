using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Master
{
    public class WLMasterEmailCredentialDto
    {
        public string UUID { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string HostServiceProvider { get; set; } = null!;
        public string SMTP { get; set; } = null!;
        public string Port { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
