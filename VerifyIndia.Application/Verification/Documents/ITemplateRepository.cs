using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Verification.Documents
{
    public interface ITemplateRepository
    {
        Task<string> GetTemplateAsync(
            string verificationCode,
            CancellationToken cancellationToken);
    }
}
