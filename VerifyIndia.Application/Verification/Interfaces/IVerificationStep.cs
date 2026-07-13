using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Models;

namespace Upgrow.Application.Verification.Verification
{
    public interface IVerificationStep
    {
        Task ExecuteAsync(VerificationContext context, Func<Task> next);
    }
}
