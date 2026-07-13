using System.Text.Json;
using VerifyIndia.Application.DTO.Verification.SprintVerify;
using VerifyIndia.Application.Helper;
using VerifyIndia.Application.IServices.Verification.SprintVerify;
using VerifyIndia.Application.Services.Mapper;

namespace VerifyIndia.Application.Services.Verification.SprintVerify
{
    public class MobileOperatorCheckService : IMobileOperatorCheckService
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public MobileOperatorCheckService(SprintVerifyClient client,
            SprintMapper sprintMapper)
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }

        public async Task<ApiResponse<JsonElement>> GetMobileOperatorCheck(MobileOperatorDto request)
        {
            var response = await _client.PostAsync(
                ApiEndpoints.MOBILE_OPERATOR_CHECK,
                request);

            return _sprintMapper.Map(response);
        }
    }
}
