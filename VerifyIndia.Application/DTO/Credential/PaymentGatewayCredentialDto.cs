using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Credential
{
    public class PaymentGatewayCredentialDto
    {
        public string?UUID { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? MerchantId { get; set; }
        public string? ApiKey { get; set; }
        public string? EncryptedKeySecret { get; set; }
        public bool IsActive { get; set; }
    }
}
