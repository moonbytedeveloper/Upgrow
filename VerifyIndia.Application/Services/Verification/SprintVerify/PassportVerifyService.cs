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
    public class PassportVerifyService : IPassportVerifyService
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public PassportVerifyService(
            SprintVerifyClient client,
            SprintMapper sprintMapper
            )
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }
        public async Task<ApiResponse<JsonElement>> GetPassportVerify(PassportVerifyDto request)
        {
            var response = await _client.PostAsync(
                ApiEndpoints.PASSPORT_VERIFY,
                request);

            return _sprintMapper.Map(response);
        }
       
    }
}
