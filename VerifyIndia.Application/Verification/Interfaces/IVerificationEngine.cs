using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Models;

namespace VerifyIndia.Application.Verification.Verification
{
    public interface IVerificationEngine
    {
        Task<VerificationContext> ExecuteAsync(VerificationContext context, CancellationToken ct);
    }

}
