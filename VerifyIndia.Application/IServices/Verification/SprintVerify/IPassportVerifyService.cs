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
    public interface IPassportVerifyService
    {
        #region PassportVerify
        Task<ApiResponse<JsonElement>> GetPassportVerify(PassportVerifyDto request);
        #endregion
    }
}
