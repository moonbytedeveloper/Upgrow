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
    public class EpfoService: IEpfoService
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public EpfoService(SprintVerifyClient client, SprintMapper sprintMapper)
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }

        public async Task<ApiResponse<JsonElement>> EpfoWithoutOtp(EPFODto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.EPFO_WITHOUT_OTP, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<JsonElement>> GetKYCDetails(EPFO_PassbookDto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.KYC_DETAILS_GET, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<JsonElement>> PassportDownload(EPFO_PassbookDto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.PASSPORT_DOWNLOAD, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
    }
}
