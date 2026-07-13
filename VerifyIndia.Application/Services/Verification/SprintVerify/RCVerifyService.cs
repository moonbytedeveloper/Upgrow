using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Verification.SprintVerify;
using VerifyIndia.Application.Helper;
using VerifyIndia.Application.IServices.Verification.SprintVerify;
using VerifyIndia.Application.Services.Mapper;

namespace VerifyIndia.Application.Services.Verification.SprintVerify
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
