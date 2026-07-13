using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Models
{
    public class VerificationContext
    {
        public string VerificationCode { get; set; } = string.Empty;
        public CancellationToken CancellationToken { get; set; }
        public object? Request { get; set; }
        public object? Response { get; set; }
        public HttpStatusCode ProviderStatusCode { get; set; }
        public bool IsProviderSuccess { get; set; } = true;
        public string ProviderMessage { get; set; } = string.Empty;
        public string? TenantId { get; set; }
        public string? ApiKey { get; set; }
        public string? Provider { get; set; }

        public string? FullName { get; set; }

        public string? PrimaryIdentifier { get; set; }

        public bool RequiresUserInput { get; set; }

        public string? NextVerificationCode { get; set; }

    }
}
