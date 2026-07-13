using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class DrivingLicenceService : IDrivingLicenceService
    {
        
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public DrivingLicenceService(
            
            SprintVerifyClient client,
            SprintMapper sprintMapper
            )
        {
            
            _client = client;
            _sprintMapper = sprintMapper;
        }

        public async Task<ApiResponse<JsonElement>> GetDrivinglicenceV1(DrivingLicenceV1Dto request)
        {            

            var result = await _client.PostAsync(ApiEndpoints.DRIVING_LICENCE_V1,request);

            return _sprintMapper.Map(result);

        }

        public async Task<ApiResponse<JsonElement>> GetDrivinglicenceV2(DrivingLicenceV2Dto request)
        {
            
            var result = await _client.PostAsync(ApiEndpoints.DRIVING_LICENCE_V2,request);

            return _sprintMapper.Map(result);
            
        
        }

        public async Task<ApiResponse<JsonElement>> VerifyAsync(DrivingLicenceV1Dto request)
        {

            var response = await _client.PostAsync(ApiEndpoints.DRIVING_LICENCE_V1, request);

            return _sprintMapper.Map(response);
        }
    }
}

