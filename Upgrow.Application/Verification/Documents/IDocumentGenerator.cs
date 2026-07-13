using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Models;

namespace Upgrow.Application.Verification.Documents
{
    public interface IDocumentGenerator
    {
        Task<string> GenerateBase64Async(
            DocumentGenerationContext context,
            CancellationToken cancellationToken);
    }
}
