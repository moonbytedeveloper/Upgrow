using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Verification.SprintVerify;
using Upgrow.Application.Helper;

namespace Upgrow.Application.IServices.Verification.SprintVerify
{
    public interface IBankVerifyService
    {
        Task<ApiResponse<JsonElement>> GetBavHybridV1(BankAccountVerification request);
        Task<ApiResponse<JsonElement>> GetBavHybridV2(BankAccountVerification request);
        Task<ApiResponse<JsonElement>> GetBavHybridV3(BAVHybridV3 request);
        Task<ApiResponse<JsonElement>> GetBavPennylessV1(BankAccountVerification request);
        Task<ApiResponse<JsonElement>> GetBavPennylessV2(BankAccountVerification request);
        Task<ApiResponse<JsonElement>> GetBavPennylessV3(BAVPennylessV3 request);
        Task<ApiResponse<JsonElement>> GetBavPennyDropV1(BAVPennydropV1 request);
        Task<ApiResponse<JsonElement>> GetBavPennyDropV2(BankAccountVerification request);

    }
}
