using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Verification.Sandbox;
using VerifyIndia.Application.Helper;

namespace VerifyIndia.Application.IServices.Verification.Sandbox
{
    public interface IAadhaarSandboxService
    {
        Task<ApiResponse<JsonElement>> SendOtpAsync(SB_AadhaarSendOtpDto request);
        Task<ApiResponse<JsonElement>> VerifyOtpAsync(SB_AadhaarVerifyOtpDto request);
    }
}
