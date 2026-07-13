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
    public class BankVerifyService : IBankVerifyService
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;
        
        public BankVerifyService(SprintVerifyClient client, SprintMapper sprintMapper)
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }

        public async Task<ApiResponse<JsonElement>> GetBavHybridV1(BankAccountVerification request)
        {       
            var response = await _client.PostAsync(
                ApiEndpoints.BAV_HYBRID_V1,
                request);
            return _sprintMapper.Map(response);
        }

        public async Task<ApiResponse<JsonElement>> GetBavHybridV2(BankAccountVerification request)
        {
            var response = await _client.PostAsync(
                ApiEndpoints.BAV_HYBRID_V2,
                request);
            return _sprintMapper.Map(response);
        }

        public async Task<ApiResponse<JsonElement>> GetBavHybridV3(BAVHybridV3 request)
        {
            
            var response = await _client.PostAsync(
                ApiEndpoints.BAV_HYBRID_V3,
                request);
            return _sprintMapper.Map(response);

        }

        public async Task<ApiResponse<JsonElement>> GetBavPennyDropV1(BAVPennydropV1 request)
        {
            
            var response = await _client.PostAsync(
                ApiEndpoints.BAV_PENNYDROP_V1,
                request);
            return _sprintMapper.Map(response);
        }

        public async Task<ApiResponse<JsonElement>> GetBavPennyDropV2(BankAccountVerification request)
        {
            
            var response = await _client.PostAsync(
                ApiEndpoints.BAV_PENNYDROP_V2,
                request);
            return _sprintMapper.Map(response);
        }

        public async Task<ApiResponse<JsonElement>> GetBavPennylessV1(BankAccountVerification request)
        {
            
            var response = await _client.PostAsync(
                ApiEndpoints.BAV_PENNYLESS_V1,
                request);
            return _sprintMapper.Map(response);
        }

        public async Task<ApiResponse<JsonElement>> GetBavPennylessV2(BankAccountVerification request)
        {
            
            var response = await _client.PostAsync(
                ApiEndpoints.BAV_PENNYLESS_V2,
                request);                
            return _sprintMapper.Map(response);
        }

        public async Task<ApiResponse<JsonElement>> GetBavPennylessV3(BAVPennylessV3 request)
        {
            
            var response = await _client.PostAsync(
                ApiEndpoints.BAV_PENNYLESS_V3,
                request);
            return _sprintMapper.Map(response);
        }
    }
}
