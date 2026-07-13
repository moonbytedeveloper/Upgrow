using System.Net;
using System.Text.Json;
using VerifyIndia.Application.DTO.Verification.Sandbox;
using VerifyIndia.Application.Helper;
using VerifyIndia.Application.IServices.Verification.Sandbox;
using VerifyIndia.Application.Services.Mapper;

namespace VerifyIndia.Application.Services.Verification.Sandbox
{
    public class BankSandboxService : IBankSandboxService
    {
        private readonly SandboxClient _sandboxClient;
        private readonly SandboxMapper _mapper;

        public BankSandboxService(SandboxClient sandboxClient, SandboxMapper mapper)
        {
            _sandboxClient = sandboxClient;
            _mapper = mapper;
        }

        public async Task<ApiResponse<JsonElement>> IfscVerifyAsync(SB_IfscVerifyDto request)
        {
            try
            {
                var endpoint = ApiEndpoints.SB_IFSC_VERIFY
                    .Replace("{ifsc}", request.ifsc);

                var response = await _sandboxClient.GetAsync(endpoint);

                var statusCode = 200;
                var success = true;
                var message = "Success";

                if (response.TryGetProperty("code", out var codeEl) && codeEl.ValueKind == JsonValueKind.Number)
                    statusCode = codeEl.GetInt32();

                if (response.TryGetProperty("status", out var statusEl) &&
                    (statusEl.ValueKind == JsonValueKind.True || statusEl.ValueKind == JsonValueKind.False))
                    success = statusEl.GetBoolean();
                else if (response.TryGetProperty("success", out var successEl) &&
                         (successEl.ValueKind == JsonValueKind.True || successEl.ValueKind == JsonValueKind.False))
                    success = successEl.GetBoolean();
                else
                    success = statusCode is >= 200 and < 300;

                if (response.TryGetProperty("message", out var msgEl) && msgEl.ValueKind == JsonValueKind.String)
                    message = msgEl.GetString() ?? message;
                else if (!success)
                    message = "Provider returned an unsuccessful response.";

                return new ApiResponse<JsonElement>
                {
                    StatusCode = (HttpStatusCode)statusCode,
                    Success = success,
                    Message = message,
                    Data = response.Clone()
                };
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<JsonElement>> PennyDropVerifyAsync(SB_PennyDropVerifyDto request)
        {
            try
            {
                var endpoint = ApiEndpoints.SB_BANK_ACCOUNT_VERIFY
                    .Replace("{ifsc}", request.ifsc)
                    .Replace("{account_number}", request.account_number);

                var response = await _sandboxClient.GetAsync(endpoint);
                return _mapper.Map(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<JsonElement>> PennyLessVerifyAsync(SB_PennyLessVerifyDto request)
        {
            try
            {
                var endpoint = ApiEndpoints.SB_BANK_ACCOUNT_PENNYLESS_VERIFY
                    .Replace("{ifsc}", request.ifsc)
                    .Replace("{account_number}", request.account_number);

                var response = await _sandboxClient.GetAsync(endpoint);
                return _mapper.Map(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
    }
}