using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Verification.SprintVerify;
using VerifyIndia.Application.Helper;

namespace VerifyIndia.Application.IServices.Verification.SprintVerify
{
    public interface IDintoPinService
    {
        Task<ApiResponse<JsonElement>> LookupIFSCAsync(IFSCLookupRequestDto request);
        Task<ApiResponse<JsonElement>> ProcessPaisaDropAsync(PaisaDropRequestDto request);
        Task<ApiResponse<JsonElement>> MobileIntelligenceAsync(MobileIntelligenceRequestDto request);
        Task<ApiResponse<JsonElement>> FetchPincodeInfoAsync(PincodeInfoRequestDto request);
        Task<ApiResponse<JsonElement>> CheckVpnProxyAsync(VpnProxyCheckRequestDto request);
        Task<ApiResponse<JsonElement>> VerifyDinAsync(DinVerificationRequestDto request);
        Task<ApiResponse<JsonElement>> VerifyDinToMobileNumberAsync(DinToMobileNumberRequestDto request);
    }
}
