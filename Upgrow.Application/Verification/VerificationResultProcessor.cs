using AuthenticateIndia.Shared.Constants;
using AuthenticateIndia.Shared.Constants.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Common;
using Upgrow.Application.Constant;
using Upgrow.Application.IServices.Master;
using Upgrow.Application.Verification.Documents;
using Upgrow.Application.Verification.Interfaces;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories.Master;
using Upgrow.Domain.Models;

namespace Upgrow.Application.Verification
{
    public class VerificationResultProcessor
        : IVerificationResultProcessor
    {
        private readonly AuditContext _auditContext;
        private readonly IMasterCMSService _cmsService;
        private readonly IDocumentGenerator _documentGenerator;

        private readonly IMasterCustomerRepository _customerRepository;

        public VerificationResultProcessor(
            AuditContext auditContext,
            IMasterCMSService masterCMSService,
            IDocumentGenerator documentGenerator,
            IMasterCustomerRepository customerRepository)
        {
            _auditContext = auditContext;
            _cmsService = masterCMSService;
            _documentGenerator = documentGenerator;
            _customerRepository = customerRepository;
        }

        public async Task<VerificationResult> ProcessAsync(
            VerificationContext context,
            Transaction transaction,
            TransactionDetail transactionDetail,
            CancellationToken cancellationToken)
        {
            if (!context.IsProviderSuccess)
            {
                return new VerificationResult
                {
                    IsSuccess = false
                };
            }

            if (context.RequiresUserInput)
            {
                return new VerificationResult
                {
                    IsSuccess = true,
                    RequiresUserInput = true,
                    NextVerificationCode = context.NextVerificationCode
                };
            }

            await ValidateSelfVerificationAsync(
                context,
                transaction,
                cancellationToken);

            var documentBase64 =
                await GenerateDocumentAsync(
                    context,
                    transaction,
                    transactionDetail,
                    cancellationToken);

            return new VerificationResult
            {
                IsSuccess = true,
                DocumentBase64 = documentBase64
            };
        }

        private async Task ValidateSelfVerificationAsync(
            VerificationContext context,
            Transaction transaction,
            CancellationToken cancellationToken)
        {
            if (!string.Equals(
                    transaction.AuthFor,
                    ConsentFor.SELF,
                    StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var customer =
                await _customerRepository
                    .GetByUuidAsync(
                        transaction.VerifierUUID);

            if (customer == null)
            {
                throw new Exception(
                    "Customer not found.");
            }

            var customerFullName = $"{customer.FName} {customer.LName}".Trim();

            /*if (!NameMatcher.IsMatch(
                    customerFullName,
                    context.FullName))
            {
                throw new Exception(
                    "The verified document does not belong to the logged-in customer.");
            }*/
        }

        private async Task<string?> GenerateDocumentAsync(
    VerificationContext context,
    Transaction transaction,
    TransactionDetail transactionDetail,
    CancellationToken cancellationToken)
        {
            if (context.Response == null)
            {
                return null;
            }

            var notesDto = await _cmsService.GetByCodeAsync(CmsConstants.VERIFICATION_DOCUMENT_NOTES);

            var documentContext =
                new DocumentGenerationContext
                {
                    VerificationCode =
                        context.VerificationCode,

                    ProviderResponse =
                        context.Response,

                    AdditionalVariables =
                        new Dictionary<string, object>(
                            StringComparer.OrdinalIgnoreCase)
                        {
                            ["verification_id"] =
                                transactionDetail.UUID,

                            ["generated_date"] = 
                                Utils.GetCurrentIndianDate(),

                            ["generated_time"] = 
                                Utils.GetCurrentIndianTimeString(),

                            ["user_id"] =
                                transaction.VerifierUUID,

                            ["latitude"] =
                                _auditContext.GetLatitude() ?? string.Empty,

                            ["longitude"] =
                                _auditContext.GetLongitude() ?? string.Empty,

                            ["notes"] = notesDto?.Description ?? string.Empty
                        }
                };

            return await _documentGenerator
                .GenerateBase64Async(
                    documentContext,
                    cancellationToken);
        }
    }
}
