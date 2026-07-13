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
using static VerifyIndia.Application.DTO.Verification.SprintVerify.GstValidationDto;

namespace VerifyIndia.Application.Services.Verification.SprintVerify
{
    public class GstAdvanceV2Service : IGstAdvanceV2Service
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public GstAdvanceV2Service(SprintVerifyClient client, SprintMapper sprintMapper)
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }

        public async Task<ApiResponse<JsonElement>> GetGstAdvanceV2(GstAdvanceV2Dto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.GST_ADVANCE_V2, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
    }
}
