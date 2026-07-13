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
    public class SingleItrDetailsService : ISingleItrDetailsService
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public SingleItrDetailsService(
            SprintVerifyClient client,
            SprintMapper sprintMapper
            )
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }

        public async Task<ApiResponse<JsonElement>> GetSingleItrDetails(SingleItrDetailsDto request)
        {
   
            var response = await _client.PostAsync(
                ApiEndpoints.SINGLE_ITR_DETAILS_ENDPOINT,
                request);

            return _sprintMapper.Map(response);
        }
    }
}
