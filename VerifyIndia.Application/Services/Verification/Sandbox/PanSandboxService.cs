using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Verification.Sandbox;
using Upgrow.Application.Helper;
using Upgrow.Application.IServices.Verification.Sandbox;
using Upgrow.Application.Services.Mapper;

namespace Upgrow.Application.Services.Verification.Sandbox
{
    public class PanSandboxService : IPanSandboxService
    {
        private readonly SandboxClient _sandboxClient;
        private readonly SandboxMapper _mapper;

        public PanSandboxService(SandboxClient sandboxClient, SandboxMapper sandboxMapper)
        {
            _sandboxClient = sandboxClient;
            _mapper = sandboxMapper;
        }

        public async Task<ApiResponse<JsonElement>> VerifyAsync(SB_PanVerifyDto request)
        {
            try
            {
                var response = await _sandboxClient.PostAsync(
                ApiEndpoints.SB_PAN_VERIFY,
                request,
                "in.co.sandbox.kyc.pan_verification.request",
                HttpMethod.Post);

                return _mapper.Map(response);

            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
        public async Task<ApiResponse<JsonElement>> PanAadharLinkStatusAsync(SB_PanAadharLinkDto request)
        {
            try
            {
                var response = await _sandboxClient.PostAsync(
                ApiEndpoints.SB_PAN_AADHAR_LINK,
                request,
                "in.co.sandbox.kyc.pan_aadhaar.status",
                HttpMethod.Post);

                return _mapper.Map(response);

            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
    }
}
