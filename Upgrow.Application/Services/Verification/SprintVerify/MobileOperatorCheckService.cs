using System.Text.Json;
using Upgrow.Application.DTO.Verification.SprintVerify;
using Upgrow.Application.Helper;
using Upgrow.Application.IServices.Verification.SprintVerify;
using Upgrow.Application.Services.Mapper;

namespace Upgrow.Application.Services.Verification.SprintVerify
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
