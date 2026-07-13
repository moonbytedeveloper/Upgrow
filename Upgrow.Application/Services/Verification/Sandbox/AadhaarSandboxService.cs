using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Verification.Sandbox;
using Upgrow.Application.Helper;
using Upgrow.Application.IServices.Verification.Sandbox;
using Upgrow.Application.Services.Mapper;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Verification.Sandbox
{
    public class AadhaarSandboxService : IAadhaarSandboxService
    {
        private readonly SandboxClient _sandboxClient;
        private readonly SandboxMapper _mapper;

        public AadhaarSandboxService(SandboxClient sandboxClient,SandboxMapper sandboxMapper)
        {
            _sandboxClient = sandboxClient;
            _mapper = sandboxMapper;
        }

        public async Task<ApiResponse<JsonElement>> SendOtpAsync(SB_AadhaarSendOtpDto request)
        {
            try
            {
                var response = await _sandboxClient.PostAsync(
                ApiEndpoints.SB_AADHAR_SENDOTP,
                request,
                "in.co.sandbox.kyc.aadhaar.okyc.otp.request",
                HttpMethod.Post);

                return _mapper.Map(response);

            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
        public async Task<ApiResponse<JsonElement>> VerifyOtpAsync(SB_AadhaarVerifyOtpDto request)
        {
            try
            {
                var response = await _sandboxClient.PostAsync(
                ApiEndpoints.SB_AADHAR_VERIFYOTP,
                request,
                "in.co.sandbox.kyc.aadhaar.okyc.request",
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
