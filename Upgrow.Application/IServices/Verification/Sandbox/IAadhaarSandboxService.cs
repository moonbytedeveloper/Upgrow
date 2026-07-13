using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Verification.Sandbox;
using Upgrow.Application.Helper;

namespace Upgrow.Application.IServices.Verification.Sandbox
{
    public interface IAadhaarSandboxService
    {
        Task<ApiResponse<JsonElement>> SendOtpAsync(SB_AadhaarSendOtpDto request);
        Task<ApiResponse<JsonElement>> VerifyOtpAsync(SB_AadhaarVerifyOtpDto request);
    }
}
