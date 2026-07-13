using AuthenticateIndia.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Registration.Business
{
    public sealed class BusinessMetadataResponseDto
    {
        public decimal BusinessVerificationFee { get; set; }
        public List<BusinessTypeDto> BusinessTypes { get; set; } = [];
        public List<VerificationDocumentDto> VerificationDocuments { get; set; } = [
            new()
            {
                UUID = Guid.NewGuid().ToString(),
                Name = "Goods and Services Tax (GST)",
                DisplayOrder = 1
            },

            new()
            {
                UUID = Guid.NewGuid().ToString(),
                Name = "Shop & Establishment License",
                DisplayOrder = 2
            },

            new()
            {
                UUID = Guid.NewGuid().ToString(),
                Name = "Udyam Registration / MSME",
                DisplayOrder = 3
            },

            new()
            {
                UUID = Guid.NewGuid().ToString(),
                Name = "Bank Account",
                DisplayOrder = 4
            }
        ];
        
    }

    public sealed class BusinessTypeDto
    {
        public string UUID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal DisplayOrder { get; set; }
    }

    public sealed class VerificationDocumentDto
    {
        public string UUID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal DisplayOrder { get; set; }
    }
}
