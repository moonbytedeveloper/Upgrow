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
    //Devloped By Dixita 27-02-2026
    public interface IValidateService
    {
        #region ValidateFetch
        Task<ApiResponse<JsonElement>> VoterIdValidation(VoterValidateDto request);
        #endregion
    }
}
