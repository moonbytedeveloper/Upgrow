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
    public class VehicleChallanService : IVehicleChallanService
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public VehicleChallanService(
            SprintVerifyClient client,
            SprintMapper sprintMapper
            )
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }

        public async Task<ApiResponse<JsonElement>> GetVehicalChallanV1(VehicalChallanV1Dto request)
        {
            var response = await _client.PostAsync(
                ApiEndpoints.VEHICLE_CHALLAN_V1,
                request);

            return _sprintMapper.Map(response);
        }

        public async Task<ApiResponse<JsonElement>> GetVehicalChallanV2(VehicalChallanV2Dto request)
        {
            var response = await _client.PostAsync(
                ApiEndpoints.VEHICLE_CHALLAN_V1,
                request);

            return _sprintMapper.Map(response);
        }
    }
}
