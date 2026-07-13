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
    public interface IPanSandboxService
    {
        Task<ApiResponse<JsonElement>> VerifyAsync(SB_PanVerifyDto request);
        Task<ApiResponse<JsonElement>> PanAadharLinkStatusAsync(SB_PanAadharLinkDto request);
        
    }
}
