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
    public class RCVerifyService : IRCVerifyService
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public RCVerifyService(
            SprintVerifyClient client,
            SprintMapper sprintMapper
            )
           {
            _client = client;
            _sprintMapper = sprintMapper;
           }
        public async Task<ApiResponse<JsonElement>> GetRCVerify(RCVerifyDto request)
        {
            var response = await _client.PostAsync(
                ApiEndpoints.RC_VERIFICATION,
                request);

            return _sprintMapper.Map(response);

        }
        public async Task<ApiResponse<JsonElement>> GetRCAdvanceVerify(RCAdvanceVerifyDto request)
        {
          
             var response = await _client.PostAsync(
                ApiEndpoints.RC_ADVANCE_VERIFICATION,
                request);

             return _sprintMapper.Map(response);

        }
        public async Task<ApiResponse<JsonElement>> GetRCReverseVerify(RCReverseDto request)
        {
                var response = await _client.PostAsync(
                    ApiEndpoints.REVERSE_RC_VERIFICATION,
                    request);

                return _sprintMapper.Map(response);


        }
    }
}
