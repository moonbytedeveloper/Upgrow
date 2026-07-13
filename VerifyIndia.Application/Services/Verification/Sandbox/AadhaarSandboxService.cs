using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Verification.Sandbox;
using VerifyIndia.Application.Helper;
using VerifyIndia.Application.IServices.Verification.Sandbox;
using VerifyIndia.Application.Services.Mapper;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Verification.Sandbox
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
