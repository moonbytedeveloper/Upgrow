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
    public class UanBasicV1Service : IUanBasicV1Service
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public UanBasicV1Service(
            SprintVerifyClient client,
            SprintMapper sprintMapper
            )
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }

        public async Task<ApiResponse<JsonElement>> GetUanBasicV1(UanBasicV1Dto request)
        {
            var response = await _client.PostAsync(
                ApiEndpoints.UAN_BASIC_V1_ENDPOINT,
                request);

            return _sprintMapper.Map(response);
        }
    }
}