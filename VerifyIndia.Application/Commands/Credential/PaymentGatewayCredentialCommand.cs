using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;

namespace Upgrow.Application.Commands.Credential
{
    public class PaymentGatewayCredentialCommand : IMasterCommand, IEncryptable
    {
        public string? UUID { get; set; }
        [Required(ErrorMessage = "Name is required!")]
        public string? Name { get; set; }
        public string? Code { get; set; }

        [Required(ErrorMessage = "MerchantId is required!")]
        public string? MerchantId { get; set; }

        [Required(ErrorMessage = "ApiKey is required!")]
        public string? ApiKey { get; set; }

        [Required(ErrorMessage = "EncryptedKeySecret is required!")]
        public string? EncryptedKeySecret { get; set; }
        public bool? IsActive { get; set; }

        public string? SensitiveValue
        {
            get => EncryptedKeySecret;
            set => EncryptedKeySecret = value;
        }
    }
}
