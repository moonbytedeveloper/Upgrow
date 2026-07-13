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
    #region Pan Advanced 
    //Develeoper Utsav 25-2-2026
    public interface IPanService
    {

        Task<ApiResponse<JsonElement>> VerifyPan(PanVerifyRequestDto request);

    }
    #endregion
}
