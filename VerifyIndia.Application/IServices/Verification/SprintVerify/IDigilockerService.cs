using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Verification.SprintVerify;
using VerifyIndia.Application.Helper;

namespace VerifyIndia.Application.IServices.Verification.SprintVerify
{
    //Developed By Utsav 27-02-2026
    public interface IDigilockerService
    {
        #region Intiate Session
        Task<ApiResponse<JsonElement>> InitiateSessionAsync(DigilockerInitiateSessionRequestDto request);
        #endregion

        #region Access Token Generation
        Task<ApiResponse<JsonElement>> GenerateAccessTokenAsync(DigilockerAccessTokenRequestDto request);
        #endregion

        #region Get Issued Files
        Task<ApiResponse<JsonElement>> GetIssuedFilesAsync(DigilockerIssuedFilesRequestDto request);
        #endregion

        #region Download Document PDF
        Task<ApiResponse<JsonElement>> DownloadPdfAsync(DigilockerDownloadPdfRequestDto request);
        #endregion

        #region Download Documet in XML
        Task<ApiResponse<JsonElement>> DownloadXmlAsync(DigilockerDownloadXmlRequestDto request);
        #endregion

        #region Get Eaadhaar Document In XML
        Task<ApiResponse<JsonElement>> GetEaadhaarXmlAsync(DigilockerEaadhaarRequestDto request);
        #endregion
    }
}
