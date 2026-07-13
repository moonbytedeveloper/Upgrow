using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Verification.SprintVerify;
using Upgrow.Application.Helper;

namespace Upgrow.Application.IServices.Verification.SprintVerify
{
    //Devloped By Dixita 27-02-2026
    public interface IValidateService
    {
        #region ValidateFetch
        Task<ApiResponse<JsonElement>> VoterIdValidation(VoterValidateDto request);
        #endregion
    }
}
