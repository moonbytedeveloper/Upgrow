using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Verification.SprintVerify;
using Upgrow.Application.Helper;
using Upgrow.Application.IServices.Verification.SprintVerify;
using Upgrow.Application.Services.Mapper;

namespace Upgrow.Application.Services.Verification.SprintVerify
{
    public class AadhaarQRCheckService : IAadhharQRCheckService
    {
        
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;
        public AadhaarQRCheckService(SprintVerifyClient client,SprintMapper sprintMapper)
        {           
            _client = client;
            _sprintMapper = sprintMapper;
        }

        public async Task<ApiResponse<JsonElement>> CheckAadhharQR(AdhaarQRCheckDto request)
        {
            // Changed from .Result (blocking) to await (async)
            var result = await _client.PostAsync(ApiEndpoints.AADHAAR_QR_CHECK,request);          
            return  _sprintMapper.Map(result);
            
        }

        public async Task<ApiResponse<JsonElement>> CheckAadhharSendOTP(AadharSendOtpDto request)
        {
            var result = await _client.PostAsync(ApiEndpoints.AADHAAR_SENDOTP_ENDPOINT, request);
            return _sprintMapper.Map(result);
        }

        public async Task<ApiResponse<JsonElement>> CheckAadhharVerifyOTP(AadharVerifyOtpDto request)
        {
            var result = await _client.PostAsync(ApiEndpoints.AADHAR_VERIFY_ENDPOINT, request);
            return _sprintMapper.Map(result);
        }
    }
}
