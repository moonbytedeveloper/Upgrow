using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Verification.Verification;
using VerifyIndia.Domain.Models;

namespace VerifyIndia.Application.Verification
{
    public class VerificationEngine : IVerificationEngine
    {
        private readonly IEnumerable<IVerificationStep> _steps;
        private readonly ILogger<VerificationEngine> _logger;

        public VerificationEngine(
            IEnumerable<IVerificationStep> steps,
            ILogger<VerificationEngine> logger)
        {
            _steps = steps;
            _logger = logger;
        }

        public async Task<VerificationContext> ExecuteAsync(VerificationContext context, CancellationToken ct)
        {
            context.CancellationToken = ct;

            _logger.LogInformation(
                "Starting verification pipeline for {VerificationCode}",
                context.VerificationCode);

            var stepList = _steps.ToList();
            var index = 0;

            async Task ExecuteNext()
            {
                ct.ThrowIfCancellationRequested();

                if (index < stepList.Count)
                {
                    var step = stepList[index];
                    index++;
                    await step.ExecuteAsync(context, ExecuteNext);
                }
            }

            await ExecuteNext();

            _logger.LogInformation(
                "Completed verification pipeline for {VerificationCode}",
                context.VerificationCode);

            return context;
        }
    }
}
