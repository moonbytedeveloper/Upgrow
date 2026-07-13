using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Verification.Interfaces;
using VerifyIndia.Domain.Models;

namespace VerifyIndia.Application.Verification
{
    public sealed class VerificationDefinition<
    TRequest,
    TProviderRequest,
    TResponse,
    TProviderResponse> : IVerificationDefinition
    where TRequest : class
    where TProviderRequest : class
    where TResponse : class
    where TProviderResponse : class
    {
        public required string VerificationCode { get; init; }

        public required string Endpoint { get; init; }

        public required string LogMessage { get; init; }

        public required Func<TProviderRequest, object?> LogValueSelector { get; init; }

        public Action<TResponse, ProviderVerificationResult>? ResponseMapper { get; init; }

        public Task<ProviderVerificationResult> ExecuteAsync(
            IVerificationPipeline pipeline,
            object? request,
            CancellationToken cancellationToken)
        {
            if (request is not TRequest typedRequest)
            {
                throw new ArgumentException(
                    $"Expected request type '{typeof(TRequest).Name}', received '{request?.GetType().Name ?? "null"}'.");
            }

            return pipeline.ExecuteAsync<
                TRequest,
                TProviderRequest,
                TResponse,
                TProviderResponse>(
                typedRequest,
                this,
                cancellationToken);
        }
    }
}
