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
    public interface IUdyamAadhaarService
    {
        #region UdyamAadhaarV1
        Task<ApiResponse<JsonElement>> GetUdyamAadhaarV1(UdyamAadhaarV1Dto request);
        #endregion
        #region UdyamAadhaarV2
        Task<ApiResponse<JsonElement>> GetUdyamAadhaarV2(UdyamAadhaarV2Dto request);
        #endregion
    }
}
