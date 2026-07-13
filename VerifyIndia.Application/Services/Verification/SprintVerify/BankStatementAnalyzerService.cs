using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
    public class BankStatementAnalyzerService : IBankStatementAnalyzerService
    {
        
        private readonly IConfiguration _config;
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        // Use a single constructor and inject all dependencies
        public BankStatementAnalyzerService(IConfiguration config, SprintVerifyClient client, SprintMapper sprintMapper)
        {
            _client = client;
            _sprintMapper = sprintMapper;
            _config = config;
        }

        #region Upload Statement
        public async Task<ApiResponse<JsonElement>> UploadStatementAsync(string reqid, IFormFile file)
        {
            try
            {
                // Convert file to base64 string
                string fileBase64;
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    fileBase64 = Convert.ToBase64String(ms.ToArray());
                }

                var body = new
                {
                    reqid,
                    file = fileBase64,
                    fileName = file.FileName,
                    contentType = file.ContentType
                };

                var result = await _client.PostAsync(ApiEndpoints.UPLOAD_STATEMENT_ENDPOINT, body);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
        #endregion

        #region Report Fetch
        public async Task<ApiResponse<JsonElement>> FetchReportAsync(ReportFetchRequestDto request)
        {
            try
            {              

                var result = await _client.PostAsync(ApiEndpoints.REPORT_FETCH_ENDPOINT, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
        #endregion
    }
}