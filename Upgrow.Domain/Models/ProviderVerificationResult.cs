using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Models
{
    public class ProviderVerificationResult
    {
        public object? Response { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; } = string.Empty;

        public string? FullName { get; set; }

        public string? PrimaryIdentifier { get; set; }

        public bool RequiresUserInput { get; set; }

        public string? NextVerificationCode { get; set; }

    }
}
