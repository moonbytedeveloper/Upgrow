using Microsoft.Extensions.Configuration;
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
    public class CommonStatusCheckService : ICommonStatusCheckService
    {

        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public CommonStatusCheckService(SprintVerifyClient client, SprintMapper sprintMapper)
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }

        #region STATUS CHECK API
        public async Task<ApiResponse<JsonElement>> CheckStatusAsync(CommonStatusCheckRequestDto request)
        {
            try
            {
                

                var result = await _client.PostAsync(ApiEndpoints.CHECK_STATUS, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
        #endregion
    }
}