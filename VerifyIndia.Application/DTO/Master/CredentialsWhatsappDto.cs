using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Master
{
    public class CredentialsWhatsappDto
    {
        public string? UUID { get; set; }
        public string APIKey { get; set; }
        public string AccessToken { get; set; }
        public string MobileNo { get; set; }
        public string SenderName { get; set; }
        public bool IsActive { get; set; }
    }
}
