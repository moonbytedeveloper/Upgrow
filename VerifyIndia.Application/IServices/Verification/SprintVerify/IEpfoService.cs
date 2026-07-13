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
    public interface IEpfoService
    {
        Task<ApiResponse<JsonElement>> GetKYCDetails(EPFO_PassbookDto request);
        Task<ApiResponse<JsonElement>> EpfoWithoutOtp(EPFODto request);
        Task<ApiResponse<JsonElement>> PassportDownload(EPFO_PassbookDto request);
    }
}
 