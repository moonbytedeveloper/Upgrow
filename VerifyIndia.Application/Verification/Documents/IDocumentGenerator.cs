using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Models;

namespace VerifyIndia.Application.Verification.Documents
{
    public interface IDocumentGenerator
    {
        Task<string> GenerateBase64Async(
            DocumentGenerationContext context,
            CancellationToken cancellationToken);
    }
}
