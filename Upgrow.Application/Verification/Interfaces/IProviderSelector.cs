using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Verification.Interfaces;

namespace Upgrow.Application.Verification.Verification
{
    public interface IProviderSelector
    {
        Task<IReadOnlyList<IProviderAdapter>> SelectAsync(string verificationCode, CancellationToken ct);
    }
}
