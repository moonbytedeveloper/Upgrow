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
    public class UdyamAadhaarService : IUdyamAadhaarService
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public UdyamAadhaarService(
            SprintVerifyClient client,
            SprintMapper sprintMapper
            )
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }
        public async Task<ApiResponse<JsonElement>> GetUdyamAadhaarV1(UdyamAadhaarV1Dto request)
        {
            var response = await _client.PostAsync(
                ApiEndpoints.UDYAM_AADHAAR_V1,
                request);

            return _sprintMapper.Map(response);

        }

        public async Task<ApiResponse<JsonElement>> GetUdyamAadhaarV2(UdyamAadhaarV2Dto request)
        {

            var response = await _client.PostAsync(
                ApiEndpoints.UDYAM_AADHAAR_V2,
                request);

            return _sprintMapper.Map(response);
        }
    }
}
