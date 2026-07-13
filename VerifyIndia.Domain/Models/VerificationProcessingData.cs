using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Models
{
    public sealed class VerificationProcessingData
    {
        public string? FullName { get; set; }

        public string? PrimaryIdentifier { get; set; }

        public string? ProviderReferenceNo { get; set; }

        public string? DocumentBase64 { get; set; }
    }
}
