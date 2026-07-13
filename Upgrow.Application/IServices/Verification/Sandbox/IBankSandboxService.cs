using System.Text.Json;
using Upgrow.Application.DTO.Verification.Sandbox;
using Upgrow.Application.Helper;

namespace Upgrow.Application.IServices.Verification.Sandbox
{
    public interface IBankSandboxService
    {
        Task<ApiResponse<JsonElement>> IfscVerifyAsync(SB_IfscVerifyDto request);
        Task<ApiResponse<JsonElement>> PennyDropVerifyAsync(SB_PennyDropVerifyDto request);
        Task<ApiResponse<JsonElement>> PennyLessVerifyAsync(SB_PennyLessVerifyDto request);
    }
}