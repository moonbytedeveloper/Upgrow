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
    public class TanVerifyService : ITanVerifyService
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public TanVerifyService(
            SprintVerifyClient client,
            SprintMapper sprintMapper
            )
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }
        public async Task<ApiResponse<JsonElement>> GetTANVerify(TANDto request)
        {
            var response = await _client.PostAsync(
                ApiEndpoints.TAN_VALIDATE,
                request);

            return _sprintMapper.Map(response);
        }
        public async Task<ApiResponse<JsonElement>> GetTANAdvance(TANAdvanceDto request)
        {
            var response = await _client.PostAsync(
                ApiEndpoints.TAN_ADVANCE,
                request);

            return _sprintMapper.Map(response);
        }

       
    }
}
