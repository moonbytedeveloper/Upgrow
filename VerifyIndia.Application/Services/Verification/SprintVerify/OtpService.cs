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
    public class OtpService :IOtpService
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public OtpService(
            SprintVerifyClient client,
            SprintMapper sprintMapper
            )
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }

        public async Task<ApiResponse<JsonElement>> SendOtp(OTPSendDto request)
        {
            var response = await _client.PostAsync(
                ApiEndpoints.OTP_SEND,
                request);

            return _sprintMapper.Map(response);
        }

        public async Task<ApiResponse<JsonElement>> VerifyOtp(OTPVerifyDto request)
        {
            var response = await _client.PostAsync(
                ApiEndpoints.VERIFY_OTP,
                request);

            return _sprintMapper.Map(response);
        }
    }
}
