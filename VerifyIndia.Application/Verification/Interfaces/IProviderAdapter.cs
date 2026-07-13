using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Models;

namespace Upgrow.Application.Verification.Interfaces
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
