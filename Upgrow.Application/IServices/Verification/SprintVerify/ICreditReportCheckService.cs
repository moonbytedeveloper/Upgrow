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
    //Developed By Utsav 26-02-2026
    public interface ICreditReportCheckService
    {
        #region State
        Task<ApiResponse<JsonElement>> StateList();
        #endregion

        #region CheckCreditReportEx
        Task<ApiResponse<JsonElement>> CheckCreditReportExAsync(CreditReportCheckExRequestDto request);
        #endregion

        #region CreditReportFetchEQ
        Task<ApiResponse<JsonElement>> CreditReportFetchEQAsync(CreditReportFetchEQRequestDto request);
        #endregion
    }
}
