using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Upgrow.Application.DTO.Verification;
using Upgrow.Application.Helper;
using Upgrow.Application.IServices.Verification.SprintVerify;
using Upgrow.Application.Services.Mapper;

namespace Upgrow.Application.Services.Verification.SprintVerify
{
    #region
    //Develeoper utsav 26-2-2026
    public class MainBalanceService : IMainBalanceService
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public MainBalanceService(SprintVerifyClient client, SprintMapper sprintMapper)
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }

        public async Task<ApiResponse<JsonElement>> GetBalanceAsync()
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.GetBalance_ENDPOINT, null);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
    }
}
#endregion