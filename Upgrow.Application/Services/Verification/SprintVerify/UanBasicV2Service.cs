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
    public class UanBasicV2Service : IUanBasicV2Service
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public UanBasicV2Service(
            SprintVerifyClient client,
            SprintMapper sprintMapper
            )
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }

        public async Task<ApiResponse<JsonElement>> GetUanBasicV2(UanBasicV2Dto request)
        {
            var response = await _client.PostAsync(
                ApiEndpoints.UAN_BASIC_V2_ENDPOINT,
                request);

            return _sprintMapper.Map(response);
        }
    }
}