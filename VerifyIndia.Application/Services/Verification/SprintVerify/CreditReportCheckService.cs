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
    public class CreditReportCheckService : ICreditReportCheckService
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public CreditReportCheckService(SprintVerifyClient client, SprintMapper sprintMapper)
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }

        #region State
        public async Task<ApiResponse<JsonElement>> StateList()
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.STATE_LIST_GET, new { });

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
        #endregion

        #region CheckCreditReportEx
        public async Task<ApiResponse<JsonElement>> CheckCreditReportExAsync(CreditReportCheckExRequestDto request)
        {
            try
            {
                
                var result = await _client.PostAsync(ApiEndpoints.CREDIT_REPORT_CheckerEX_ENDPOINT, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
        #endregion

        #region CreditReportFetchEQ
        public async Task<ApiResponse<JsonElement>> CreditReportFetchEQAsync(CreditReportFetchEQRequestDto request)
        {
            try
            {
                

                var result = await _client.PostAsync(ApiEndpoints.CreditReportFetchEQ_ENDPOINT, request);

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
