using Microsoft.Extensions.Configuration;
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
    #region Pan Advanced 
    //Develeoper Utsav 25-2-2026 changes by utsav 26-2-2026
    public class PanService : IPanService
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public PanService(
            SprintVerifyClient client,
            SprintMapper sprintMapper
            )
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }
        public async Task<ApiResponse<JsonElement>> VerifyPan(PanVerifyRequestDto request)
        {
            var response = await _client.PostAsync(
                ApiEndpoints.PAN_VERIFY_ENDPOINT,
                request);

            return _sprintMapper.Map(response);
        }
    }

}
    #endregion