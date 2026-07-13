using System.Text.Json;
using VerifyIndia.Application.DTO.Verification.Sandbox;
using VerifyIndia.Application.Helper;

namespace VerifyIndia.Application.IServices.Verification.Sandbox
{
    public interface IBankSandboxService
    {
        Task<ApiResponse<JsonElement>> IfscVerifyAsync(SB_IfscVerifyDto request);
        Task<ApiResponse<JsonElement>> PennyDropVerifyAsync(SB_PennyDropVerifyDto request);
        Task<ApiResponse<JsonElement>> PennyLessVerifyAsync(SB_PennyLessVerifyDto request);
    }
}