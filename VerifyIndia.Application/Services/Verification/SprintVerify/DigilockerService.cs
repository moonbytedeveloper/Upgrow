using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
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
    //Developed By Utsav 27-02-2026
    public class DigilockerService : IDigilockerService
    {
        private readonly SprintVerifyClient _client;
        private readonly SprintMapper _sprintMapper;

        public DigilockerService(SprintVerifyClient client, SprintMapper sprintMapper)
        {
            _client = client;
            _sprintMapper = sprintMapper;
        }

        #region "Initiate Session"
        public async Task<ApiResponse<JsonElement>> InitiateSessionAsync(DigilockerInitiateSessionRequestDto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.Inititate_Session_ENDPOINT, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
        #endregion

        #region "Access Token Generate"
        public async Task<ApiResponse<JsonElement>> GenerateAccessTokenAsync(DigilockerAccessTokenRequestDto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.Access_Token_Generation_ENDPOINT, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
        #endregion

        #region Get Issued Files
        public async Task<ApiResponse<JsonElement>> GetIssuedFilesAsync(DigilockerIssuedFilesRequestDto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.Issued_Files_Endpoint, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
        #endregion

        #region Download Document PDF
        public async Task<ApiResponse<JsonElement>> DownloadPdfAsync(DigilockerDownloadPdfRequestDto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.Download_Pdf_Endpoint, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
        #endregion

        #region Download_Document_Xml
        public async Task<ApiResponse<JsonElement>> DownloadXmlAsync(DigilockerDownloadXmlRequestDto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.Download_Xml_Endpoint, request);

                return _sprintMapper.Map(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<JsonElement>.InternalServerError(ex.Message);
            }
        }
        #endregion

        #region Get Eaadhaar Document In XML
        public async Task<ApiResponse<JsonElement>> GetEaadhaarXmlAsync(DigilockerEaadhaarRequestDto request)
        {
            try
            {
                var result = await _client.PostAsync(ApiEndpoints.Eaadhaar_Endpoint, request);

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
