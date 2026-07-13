using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Models;

namespace VerifyIndia.Application.Verification.Interfaces
{
    public interface IProviderAdapter
    {
        string ProviderCode { get; }

        Task<ProviderVerificationResult> VerifyAsync(
            string verificationCode,
            object? request,
            CancellationToken cancellationToken = default);
    }
}
