using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Verification;
using VerifyIndia.Application.Verification.Interfaces;
using VerifyIndia.Domain.Models;

namespace VerifyIndia.Infrastructure.Verification.Providers.SprintVerify
{
    public sealed class SprintProvider : IProviderAdapter
    {
        public string ProviderCode => "SPRINT_VERIFY";

        private readonly IVerificationPipeline _pipeline;

        private readonly IReadOnlyDictionary<string, IVerificationDefinition> _definitions;

        public SprintProvider(
            IVerificationPipeline pipeline)
        {
            _pipeline = pipeline;

            _definitions = SprintDefinitions.All;
        }

        public async Task<ProviderVerificationResult> VerifyAsync(
            string verificationCode,
            object? request,
            CancellationToken cancellationToken = default)
        {
            if (!_definitions.TryGetValue(
                verificationCode,
                out var definition))
            {
                throw new NotSupportedException(
                    $"Sprint does not support verification '{verificationCode}'.");
            }

            return await definition.ExecuteAsync(
                _pipeline,
                request,
                cancellationToken);
        }
    }
}
