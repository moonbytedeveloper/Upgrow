using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Verification.Common.Request;
using VerifyIndia.Application.DTO.Verification.Common.Response;
using VerifyIndia.Application.DTO.Verification.Sandbox;
using VerifyIndia.Application.DTO.Verification.Sandbox.Response;
using VerifyIndia.Application.Helper;
using VerifyIndia.Application.Verification.Interfaces;
using VerifyIndia.Application.Verification.Verification;
using VerifyIndia.Domain.Models;
using static VerifyIndia.Application.Constants;

namespace VerifyIndia.Infrastructure.Verification.Providers.Sandbox;

public sealed class SandboxProvider : IProviderAdapter
{
    public string ProviderCode => "SANDBOX";

    private sealed record VerificationHandler(
        string Endpoint,
        Func<object?, CancellationToken, Task<ProviderVerificationResult>> Execute);

    private readonly CentralApiClient _centralApiClient;
    private readonly IMapper _mapper;
    private readonly ILogger<SandboxProvider> _logger;
    private readonly IReadOnlyDictionary<string, VerificationHandler> _handlers;

    public SandboxProvider(
        CentralApiClient centralApiClient,
        IMapper mapper,
        ILogger<SandboxProvider> logger)
    {
        _centralApiClient = centralApiClient;
        _mapper = mapper;
        _logger = logger;

        _handlers = new Dictionary<string, VerificationHandler>(StringComparer.OrdinalIgnoreCase)
        {
            [VerificationCodes.Pan] = CreateHandler<PanVerifyRequest, SB_PanVerifyDto, PanVerifyResponse, SB_PanVerifyResponse>(
                ApiEndpoints.CENTRAL_PAN_VERIFY,
                "[SandboxProvider] PAN verify request mapped: {pan}",
                request => request.pan),

            [VerificationCodes.IfscLookup] = CreateHandler<IfscVerifyRequest, SB_IfscVerifyDto, IfscVerifyResponse, SB_IfscVerifyResponse>(
                ApiEndpoints.CENTRAL_IFSC_VERIFY,
                "[SandboxProvider] IFSC verify request mapped: {ifsc}",
                request => request.ifsc),

            [VerificationCodes.PanAdharLink] = CreateHandler<PanAadharLinkRequest, SB_PanAadharLinkDto, PanAadharLinkResponse, SB_PanAadharLinkResponse>(
                ApiEndpoints.CENTRAL_PAN_AADHAR_LINK,
                "[SandboxProvider] PAN Aadhaar link request mapped: {pan}",
                request => request.pan),

            [VerificationCodes.SearchGst] = CreateHandler<SearchGstRequest, SB_SearchGstDto, SearchGstResponse, SB_SearchGstResponse>(
                ApiEndpoints.CENTRAL_SEARCH_GSTIN,
                "[SandboxProvider] GSTIN search request mapped: {Pan}",
                request => request.pan),


            [VerificationCodes.BavPennydropV2] = CreateHandler<PennyDropVerifyRequest, SB_PennyDropVerifyDto, PennyDropVerifyResponse, SB_PennyDropVerifyResponse>(
                ApiEndpoints.CENTRAL_PENNY_DROP_VERIFY,
                "[SandboxProvider] Penny drop request mapped: {ifsc}",
                request => request.ifsc),

            [VerificationCodes.BavPennylessV3] = CreateHandler<PennyLessVerifyRequest, SB_PennyLessVerifyDto, PennyLessVerifyResponse, SB_PennyLessVerifyResponse>(
                ApiEndpoints.CENTRAL_PENNY_LESS_VERIFY,
                "[SandboxProvider] Penny less request mapped: {ifsc}",
                request => request.ifsc),

            [VerificationCodes.AadhaarSendOTP] = CreateHandler<AadhaarSendOtpRequest, SB_AadhaarSendOtpDto, AadharSendOtpResponse, SB_AadharSendOtpResponse>(
                ApiEndpoints.CENTRAL_AADHAR_SENDOTP,
                "[SandboxProvider] Penny less request mapped: {AadharNumber}",
                request => request.aadhaar_number),

            [VerificationCodes.AadhaarVerifyOtp] = CreateHandler<AadhaarVerifyOtpRequest, SB_AadhaarVerifyOtpDto, AadhaarVerifyOtpResponse, SB_AadhaarVerifyOtpResponse>(
                ApiEndpoints.CENTRAL_AADHAR_VERIFYOTP,
                "[SandboxProvider] Penny less request mapped: {RefrenceId}",
                request => request.reference_id),
        };
    }

    private VerificationHandler CreateHandler<TRequest, TProviderRequest, TResponse, TProviderResponse>(
        string endpoint,
        string logMessage,
        Func<TProviderRequest, object?> logValueSelector)
        where TRequest : class
        where TProviderRequest : class
        where TProviderResponse : class
        where TResponse : class
    {
        return new VerificationHandler(
            endpoint,
            async (request, ct) =>
            {
                if (request is not TRequest typedRequest)
                {
                    throw new ArgumentException(
                        $"Request type mismatch for '{endpoint}'. Expected {typeof(TRequest).Name}.");
                }

                var response = await VerifyAsync<TRequest, TProviderRequest, TResponse, TProviderResponse>(
                    typedRequest,
                    endpoint,
                    logMessage,
                    logValueSelector,
                    ct);

                return CreateVerificationResult(response);
            });
    }

    private VerificationHandler CreateHandler<TProviderResponse, TResponse>(
        string endpoint,
        string logMessage)
        where TProviderResponse : class
        where TResponse : class
    {
        return new VerificationHandler(
            endpoint,
            async (_, ct) =>
            {
                var response = await VerifyAsync<TProviderResponse, TResponse>(
                    endpoint,
                    logMessage,
                    ct);

                return CreateVerificationResult(response);
            });
    }

    private ProviderVerificationResult CreateVerificationResult<TResponse>(
        ApiResponse<TResponse> apiResponse)
        where TResponse : class
    {
        return new ProviderVerificationResult
        {
            Response = apiResponse.Data,
            StatusCode = apiResponse.StatusCode,
            IsSuccess = apiResponse.Success,
            Message = apiResponse.Message
        };
    }

    private async Task<ApiResponse<TResponse>> VerifyAsync<TRequest, TProviderRequest, TResponse, TProviderResponse>(
        TRequest request,
        string endpoint,
        string logMessage,
        Func<TProviderRequest, object?> logValueSelector,
        CancellationToken ct)
        where TProviderRequest : class
        where TProviderResponse : class
        where TResponse : class
    {
        var providerRequest = _mapper.Map<TProviderRequest>(request);

        _logger.LogDebug(logMessage, logValueSelector(providerRequest));

        var providerResponse = await _centralApiClient.PostAsync<TProviderResponse>(
            endpoint,
            providerRequest,
            ct);

        if (!providerResponse.Success)
        {
            return ApiResponse<TResponse>.Fail(providerResponse.Message, providerResponse.StatusCode);
        }

        if (providerResponse.Data is null)
        {
            return ApiResponse<TResponse>.Fail(
                "Central API response did not contain a data payload.",
                providerResponse.StatusCode);
        }

        var response = _mapper.Map<TResponse>(providerResponse.Data);
        return ApiResponse<TResponse>.Ok(response, providerResponse.Message, providerResponse.StatusCode);
    }

    private async Task<ApiResponse<TResponse>> VerifyAsync<TProviderResponse, TResponse>(
        string endpoint,
        string logMessage,
        CancellationToken ct)
        where TProviderResponse : class
        where TResponse : class
    {
        _logger.LogDebug(logMessage);

        var providerResponse = await _centralApiClient.PostAsync<TProviderResponse>(
            endpoint,
            null,
            ct);

        if (!providerResponse.Success)
        {
            return ApiResponse<TResponse>.Fail(providerResponse.Message, providerResponse.StatusCode);
        }

        if (providerResponse.Data is null)
        {
            return ApiResponse<TResponse>.Fail(
                "Central API response did not contain a data payload.",
                providerResponse.StatusCode);
        }

        var response = _mapper.Map<TResponse>(providerResponse.Data);
        return ApiResponse<TResponse>.Ok(response, providerResponse.Message, providerResponse.StatusCode);
    }

    public async Task<ProviderVerificationResult> VerifyAsync(
        string verificationCode,
        object? request,
        CancellationToken ct)
    {
        if (!_handlers.TryGetValue(verificationCode, out var handler))
        {
            throw new NotSupportedException($"Sandbox does not support '{verificationCode}'.");
        }

        _logger.LogInformation(
            "[SandboxProvider] Executing {VerificationCode} on {Endpoint}",
            verificationCode.ToUpperInvariant(),
            handler.Endpoint);

        return await handler.Execute(request, ct);
    }
}
