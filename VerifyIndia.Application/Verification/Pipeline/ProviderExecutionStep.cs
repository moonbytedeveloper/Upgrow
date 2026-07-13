using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Verification.Verification;
using VerifyIndia.Domain.Models;

namespace VerifyIndia.Application.Verification.Pipeline;

public class ProviderExecutionStep : IVerificationStep
{
    private readonly IProviderSelector _providerSelector;
    private readonly ILogger<ProviderExecutionStep> _logger;

    public ProviderExecutionStep(
        IProviderSelector providerSelector,
        ILogger<ProviderExecutionStep> logger)
    {
        _providerSelector = providerSelector;
        _logger = logger;
    }

    public async Task ExecuteAsync(VerificationContext context, Func<Task> next)
    {
        var providers = await _providerSelector.SelectAsync(
            context.VerificationCode,
            context.CancellationToken);

        ProviderVerificationResult? result = null;

        foreach (var provider in providers)
        {
            context.Provider = provider.ProviderCode;

            _logger.LogInformation(
                "[Pipeline] ProviderExecutionStep: Executing {ProviderCode} for {VerificationCode}",
                provider.ProviderCode,
                context.VerificationCode);

            result = await provider.VerifyAsync(
                context.VerificationCode,
                context.Request,
                context.CancellationToken);

            context.Response =
                result.Response;

            context.ProviderStatusCode =
                result.StatusCode;

            context.IsProviderSuccess =
                result.IsSuccess;

            context.ProviderMessage =
                result.Message;

            context.Provider =
                provider.ProviderCode;

            context.FullName =
                result.FullName;

            context.PrimaryIdentifier =
                result.PrimaryIdentifier;

            context.RequiresUserInput =
                result.RequiresUserInput;

            context.NextVerificationCode =
                result.NextVerificationCode;

            if (result.IsSuccess)
            {
                break;
            }

            _logger.LogWarning(
                "[Pipeline] ProviderExecutionStep: {ProviderCode} failed for {VerificationCode} with status {StatusCode}",
                provider.ProviderCode,
                context.VerificationCode,
                result.StatusCode);
        }

        await next();
    }
}
