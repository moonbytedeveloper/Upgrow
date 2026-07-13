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
    public class ITRCompliancecheckService : IITRCompliancecheckService
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public ITRCompliancecheckService(SprintVerifyClient client, SprintMapper sprintMapper)
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }

        public async Task<ApiResponse<JsonElement>> GetITRCompliancecheck(ITRCompliancecheckDto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.ITR_COMPLIANCE_CHECK, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
    }
}
