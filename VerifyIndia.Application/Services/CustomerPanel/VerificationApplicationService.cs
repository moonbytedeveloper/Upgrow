using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.IServices.CustomerPanel;
using VerifyIndia.Application.Verification.Interfaces;
using VerifyIndia.Application.Verification.Verification;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Models;

namespace VerifyIndia.Application.Services.CustomerPanel
{
    public class VerificationApplicationService : IVerificationApplicationService
    {
        private readonly IVerificationEngine
    _verificationEngine;

        private readonly IVerificationResultProcessor
            _verificationResultProcessor;

        public VerificationApplicationService(
    IVerificationEngine verificationEngine,
    IVerificationResultProcessor verificationResultProcessor)
        {
            _verificationEngine =
                verificationEngine;

            _verificationResultProcessor =
                verificationResultProcessor;
        }

        public async Task<VerificationResult> ExecuteAsync(
    VerificationContext context,
    Transaction transaction,
    TransactionDetail detail,
    CancellationToken cancellationToken)
        {
            context =
                await _verificationEngine
                    .ExecuteAsync(
                        context,
                        cancellationToken);

            return await _verificationResultProcessor
                .ProcessAsync(
                    context,
                    transaction,
                    detail,
                    cancellationToken);
        }
    }
}
