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
    public interface IVehicleChallanService
    {
        Task<ApiResponse<JsonElement>> GetVehicalChallanV1(VehicalChallanV1Dto request);
        Task<ApiResponse<JsonElement>> GetVehicalChallanV2(VehicalChallanV2Dto request);
    }
}
