using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.WL;
using VerifyIndia.Application.DTO.DropDown;
using VerifyIndia.Application.DTO.WL;
using VerifyIndia.Application.DTOs;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Application.IServices.WL
{
    public interface IWLMasterCompanyBasicDataService
       : IMasterService<WLMasterCompanyBasicDataDto, WLMasterCompanyBasicDataCommand>, 
         IWLBaseService<WL_MasterCompanyBasicData, WLMasterCompanyBasicDataDto, WLMasterCompanyBasicDataCommand>
    {

        /// <summary>
        /// Get tenant dropdown for filtering
        /// </summary>
        Task<List<MasterDropDownDto>> GetTenantDropdownAsync();

        /// <summary>
        /// Get paged company basic data by tenant
        /// </summary>
        Task<PagedResult<WLMasterCompanyBasicDataDto>> GetPagedByTenantAsync(DataTableRequest request, string? tenantUuid);
        /// <summary>
        /// Get the first company basic data record
        /// </summary>
        Task<WLMasterCompanyBasicDataDto?> GetFirstAsync();
    }
}
