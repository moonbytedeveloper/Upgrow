using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Models;

namespace Upgrow.Application.Verification.Verification
{
    public interface IVerificationEngine
    {
        Task<VerificationContext> ExecuteAsync(VerificationContext context, CancellationToken ct);
    }

}
