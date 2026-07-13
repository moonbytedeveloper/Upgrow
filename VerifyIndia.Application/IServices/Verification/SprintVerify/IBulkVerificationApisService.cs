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
    public interface IBulkVerificationApisService
    {
        #region GST Verify 
        Task<ApiResponse<JsonElement>> BulkUploadGstAsync(BulkUploadGstItemDto request);
        #endregion
        #region Rc/Vehicle Verify
        Task<ApiResponse<JsonElement>> BulkUploadRcAsync(BulkUploadRcItemDto request);
        #endregion
        #region Bank Account
        Task<ApiResponse<JsonElement>> BankAccountVerifyAsync();
        #endregion

    }
}
