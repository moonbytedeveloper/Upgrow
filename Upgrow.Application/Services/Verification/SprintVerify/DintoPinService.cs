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
    public class DintoPinService : IDintoPinService
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public DintoPinService(SprintVerifyClient client, SprintMapper sprintMapper)
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }

        public async Task<ApiResponse<JsonElement>> ProcessPaisaDropAsync(PaisaDropRequestDto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.PAISA_DROP_Endpoint, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<JsonElement>> LookupIFSCAsync(IFSCLookupRequestDto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.IFSC_LOOKUP_Endpoint, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<JsonElement>> MobileIntelligenceAsync(MobileIntelligenceRequestDto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.MOBILE_INTELLIGENCE_ENDPOINT, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<JsonElement>> FetchPincodeInfoAsync(PincodeInfoRequestDto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.PINCODE_INFO_ENDPOINT, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<JsonElement>> CheckVpnProxyAsync(VpnProxyCheckRequestDto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.VPN_PROXY_CHECK_ENDPOINT, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<JsonElement>> VerifyDinAsync(DinVerificationRequestDto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.DIN_VERIFICATION_ENDPOINT, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<JsonElement>> VerifyDinToMobileNumberAsync(DinToMobileNumberRequestDto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.DIN_TO_MOBILE_NUMBER_ENDPOINT, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
    }
}
