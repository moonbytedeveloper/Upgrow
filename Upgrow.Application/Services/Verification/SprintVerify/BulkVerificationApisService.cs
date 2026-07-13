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
    public class BulkVerificationApisService : IBulkVerificationApisService
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public BulkVerificationApisService(SprintVerifyClient client, SprintMapper sprintMapper)
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }

        #region GST Verify
        public async Task<ApiResponse<JsonElement>> BulkUploadGstAsync(BulkUploadGstItemDto request)
        {
            try
            {
               
                var result = await _client.PostAsync(ApiEndpoints.BULK_UPLOAD_GST_ENDPOINT, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
        #endregion

        #region Rc/Vehicle Verify
        public async Task<ApiResponse<JsonElement>> BulkUploadRcAsync(BulkUploadRcItemDto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.BULK_UPLOAD_RC_ENDPOINT, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
        #endregion

        #region Bank Account Verify
        public async Task<ApiResponse<JsonElement>> BankAccountVerifyAsync()
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.BANK_ACCOUNT_VERIFY_ENDPOINT, null);

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