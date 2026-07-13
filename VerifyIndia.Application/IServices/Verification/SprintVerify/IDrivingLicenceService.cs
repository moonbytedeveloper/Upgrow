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
    public interface IDrivingLicenceService
    {
        #region DrivingLicence V1
        Task<ApiResponse<JsonElement>> GetDrivinglicenceV1(DrivingLicenceV1Dto request);
        #endregion

        #region DrivingLicence V2
        Task<ApiResponse<JsonElement>> GetDrivinglicenceV2(DrivingLicenceV2Dto request);
        Task<ApiResponse<JsonElement>> VerifyAsync(DrivingLicenceV1Dto request);
        #endregion
    }
}
