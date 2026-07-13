using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Models;

namespace VerifyIndia.Application.Verification.Interfaces
{
    public interface IVerificationDefinition
    {
        string VerificationCode { get; }

        Task<ProviderVerificationResult> ExecuteAsync(
            IVerificationPipeline pipeline,
            object? request,
            CancellationToken cancellationToken);
    }
}
