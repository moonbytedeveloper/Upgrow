using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Models
{
    public sealed class DocumentGenerationContext
    {
        public required string VerificationCode { get; init; }

        public required object ProviderResponse { get; init; }

        public IDictionary<string, object> AdditionalVariables { get; init; }
            = new Dictionary<string, object>(
                StringComparer.OrdinalIgnoreCase);
    }
}
