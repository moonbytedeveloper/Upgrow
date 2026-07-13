using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Models;

namespace Upgrow.Application.Verification.Interfaces
{
    public interface IVerificationPipeline
    {
        Task<ProviderVerificationResult> ExecuteAsync<
            TRequest,
            TProviderRequest,
            TResponse,
            TProviderResponse>(
            TRequest request,
            VerificationDefinition<
                TRequest,
                TProviderRequest,
                TResponse,
                TProviderResponse> definition,
            CancellationToken cancellationToken)
            where TRequest : class
            where TProviderRequest : class
            where TResponse : class
            where TProviderResponse : class;
    }
}
