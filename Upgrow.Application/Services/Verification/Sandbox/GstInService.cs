using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Verification.Sandbox;
using Upgrow.Application.Helper;
using Upgrow.Application.IServices.Verification.Sandbox;
using Upgrow.Application.Services.Mapper;

namespace Upgrow.Application.Services.Verification.Sandbox
{
    public class GstInService : IGstInService
    {
        private readonly SandboxClient _sandboxClient;
        private readonly SandboxMapper _mapper;
        public GstInService(SandboxClient sandboxClient, SandboxMapper mapper)
        {
            _sandboxClient = sandboxClient;
            _mapper = mapper;
        }
        public async Task<ApiResponse<JsonElement>> VerifyAsync(SB_SearchGstDto request)
        {
            try
            {
                var response = await _sandboxClient.PostAsync(
                ApiEndpoints.SB_SEARCH_GSTIN,
                request,
                string.Empty,
                HttpMethod.Post);

                return _mapper.Map(response);

            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
    }
}
