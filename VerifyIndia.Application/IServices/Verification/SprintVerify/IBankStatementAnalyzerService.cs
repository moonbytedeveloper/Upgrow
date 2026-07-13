using Microsoft.AspNetCore.Http;
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
    public interface IBankStatementAnalyzerService
    {
        #region Upload Statement
        Task<ApiResponse<JsonElement>> UploadStatementAsync(string reqId, IFormFile file);
        #endregion
        #region Report Fetch
        Task<ApiResponse<JsonElement>> FetchReportAsync(ReportFetchRequestDto request);
        #endregion
    }
}
