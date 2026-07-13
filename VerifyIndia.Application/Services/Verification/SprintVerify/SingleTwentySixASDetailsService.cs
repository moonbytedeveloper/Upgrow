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
    public class SingleTwentySixASDetailsService : ISingleTwentySixASDetailsService
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public SingleTwentySixASDetailsService(
            SprintVerifyClient client,
            SprintMapper sprintMapper
            )
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }

        public async Task<ApiResponse<JsonElement>> GetSingleTwentySixASDetails(SingleTwentySixASDetailsDto request)
        {
            var response = await _client.PostAsync(
                ApiEndpoints.SINGLE_TWENTY_SIX_AS_DETAILS_ENDPOINT,
                request);

            return _sprintMapper.Map(response);
        }
    }
}
