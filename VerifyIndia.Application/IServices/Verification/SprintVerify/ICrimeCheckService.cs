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
    public interface ICrimeCheckService
    {
        #region Crime Check Individual
        Task<ApiResponse<JsonElement>> CrimeCheckIndividualAsync(CrimeCheckIndividualRequestDto request);
        #endregion

        #region Crime Check Company
        Task<ApiResponse<JsonElement>> CrimeCheckCompanyAsync(CrimeCheckCompanyRequestDto request);
        #endregion

        #region Download PDF Report
        Task<ApiResponse<JsonElement>> DownloadPdfReportAsync(CrimeCheckDownloadPdfReportRequestDto request);
        #endregion

        #region Download JSON Report
        Task<ApiResponse<JsonElement>> DownloadJsonReportAsync(CrimeCheckDownloadJsonReportRequestDto request);
        #endregion
    }
}
