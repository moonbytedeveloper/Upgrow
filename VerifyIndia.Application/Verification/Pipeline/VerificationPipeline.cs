using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Helper;
using VerifyIndia.Application.Verification.Interfaces;
using VerifyIndia.Domain.Models;

namespace VerifyIndia.Application.Verification.Pipeline
{
    public sealed class VerificationPipeline : IVerificationPipeline
    {
        private readonly CentralApiClient _centralApiClient;
        private readonly IMapper _mapper;
        private readonly ILogger<VerificationPipeline> _logger;
        private readonly IConfiguration _configuration;

        public VerificationPipeline(
            CentralApiClient centralApiClient,
            IMapper mapper,
            ILogger<VerificationPipeline> logger,
            IConfiguration configuration)
        {
            _centralApiClient = centralApiClient;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<ProviderVerificationResult> ExecuteAsync<
            TRequest,
            TProviderRequest,
            TResponse,
            TProviderResponse>(
            TRequest request,
            VerificationDefinition<
                TRequest,
                TProviderRequest,
                TResponse,
                TProviderResponse> definition,
            CancellationToken cancellationToken)
            where TRequest : class
            where TProviderRequest : class
            where TResponse : class
            where TProviderResponse : class
        {
            // Map request

            var providerRequest =
                _mapper.Map<TProviderRequest>(request);

            // Log request

            _logger.LogDebug(
                definition.LogMessage,
                definition.LogValueSelector(providerRequest));

            ApiResponse<TProviderResponse> providerResponse;

            if (UseMockProvider())
            {
                providerResponse =
                    MockResponseProvider.Get<TProviderResponse>(
                        definition.Endpoint);
            }
            else
            {
                providerResponse =
                    await _centralApiClient.PostAsync<TProviderResponse>(
                        definition.Endpoint,
                        providerRequest,
                        cancellationToken);
            }

            if (!providerResponse.Success)
            {
                return Fail(
                    providerResponse.Message,
                    providerResponse.StatusCode);
            }

            if (providerResponse.Data is null)
            {
                return Fail(
                    "Provider response did not contain a data payload.",
                    providerResponse.StatusCode);
            }

            // Map provider response

            var response =
                _mapper.Map<TResponse>(
                    providerResponse.Data,
                    options =>
                    {
                        options.Items["Request"] = request;
                    });

            // Wrap into ApiResponse<TResponse>

            var apiResponse =
                ApiResponse<TResponse>.Ok(
                    response,
                    providerResponse.Message,
                    providerResponse.StatusCode);

            return CreateResult(
                apiResponse,
                definition.ResponseMapper);
        }

        private bool UseMockProvider()
        {
            return _configuration.GetValue<bool>(
                "SprintVerifyProvider:UseMock");
        }

        private static ProviderVerificationResult Fail(
            string message,
            HttpStatusCode statusCode)
        {
            return new ProviderVerificationResult
            {
                IsSuccess = false,
                Message = message,
                StatusCode = statusCode
            };
        }

        private static ProviderVerificationResult CreateResult<TResponse>(
            ApiResponse<TResponse> apiResponse,
            Action<TResponse, ProviderVerificationResult>? responseMapper)
            where TResponse : class
        {
            var result =
                new ProviderVerificationResult
                {
                    Response = apiResponse.Data,
                    StatusCode = apiResponse.StatusCode,
                    IsSuccess = apiResponse.Success,
                    Message = apiResponse.Message
                };

            if (apiResponse.Data != null)
            {
                responseMapper?.Invoke(
                    apiResponse.Data,
                    result);
            }

            return result;
        }
    }
}
