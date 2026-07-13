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
    public class TelecomService : ITelecomService
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public TelecomService(
            SprintVerifyClient client,
            SprintMapper sprintMapper
            )
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }

        public async Task<ApiResponse<JsonElement>> TelecomValidation(TelecomValidateDto request)
        {
            var response = await _client.PostAsync(
                ApiEndpoints.TELECOM_VERIFY,
                request);

            return _sprintMapper.Map(response);
        }
        public async Task<ApiResponse<JsonElement>> GetTelecomDetails(TelecomDetailDto request)
        {

            var response = await _client.PostAsync(
                ApiEndpoints.TELECOM_GET_DETAILS,
                request);

            return _sprintMapper.Map(response);
        }
    }
}
