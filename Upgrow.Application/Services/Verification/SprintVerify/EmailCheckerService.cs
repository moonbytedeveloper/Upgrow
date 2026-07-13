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
    public class EmailCheckerService : IEmailCheckerService
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public EmailCheckerService(SprintVerifyClient client, SprintMapper sprintMapper)
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }

        public async Task<ApiResponse<JsonElement>> GetEmailChecker(EmailCheckerDto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.EMAIL_CHECKER, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<JsonElement>> GetEmailCheckerV2(EmailCheckerV2Dto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.EMAIL_CHECKERV2, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
    }
}
