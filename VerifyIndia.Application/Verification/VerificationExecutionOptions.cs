using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Models;

namespace VerifyIndia.Application.Verification
{
    public sealed class VerificationExecutionOptions<
    TProviderRequest,
    TResponse>
    {
        public required string VerificationCode { get; init; }

        public required string Endpoint { get; init; }

        public required string LogMessage { get; init; }

        public required Func<TProviderRequest, object?> LogValueSelector { get; init; }

        public Action<TResponse, ProviderVerificationResult>? ResultMapper { get; init; }
    }
}
