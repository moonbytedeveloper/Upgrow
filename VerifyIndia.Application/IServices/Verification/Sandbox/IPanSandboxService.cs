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
    public interface IPanSandboxService
    {
        Task<ApiResponse<JsonElement>> VerifyAsync(SB_PanVerifyDto request);
        Task<ApiResponse<JsonElement>> PanAadharLinkStatusAsync(SB_PanAadharLinkDto request);
        
    }
}
