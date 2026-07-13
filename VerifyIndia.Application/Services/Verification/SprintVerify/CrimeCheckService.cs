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
    public class CrimeCheckService : ICrimeCheckService
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public CrimeCheckService(SprintVerifyClient client, SprintMapper sprintMapper)
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }

        #region Crime Check Individual
        public async Task<ApiResponse<JsonElement>> CrimeCheckIndividualAsync(CrimeCheckIndividualRequestDto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.CRIME_CHECK_INDIVIDUAL_ENDPOINT, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
        #endregion

        #region Crime Check Company
        public async Task<ApiResponse<JsonElement>> CrimeCheckCompanyAsync(CrimeCheckCompanyRequestDto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.CRIME_CHECK_COMPANY_ENDPOINT, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
        #endregion

        #region Download PDF Report
        public async Task<ApiResponse<JsonElement>> DownloadPdfReportAsync(CrimeCheckDownloadPdfReportRequestDto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.CRIME_CHECK_DOWNLOAD_PDF_REPORT_ENDPOINT, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
        #endregion

        #region Download JSON Report
        public async Task<ApiResponse<JsonElement>> DownloadJsonReportAsync(CrimeCheckDownloadJsonReportRequestDto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.CRIME_CHECK_DOWNLOAD_JSON_REPORT_ENDPOINT, request);

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