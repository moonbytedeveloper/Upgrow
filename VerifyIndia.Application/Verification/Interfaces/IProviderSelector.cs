using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Verification.Interfaces;

namespace VerifyIndia.Application.Verification.Verification
{
    public interface IProviderSelector
    {
        Task<IReadOnlyList<IProviderAdapter>> SelectAsync(string verificationCode, CancellationToken ct);
    }
}
