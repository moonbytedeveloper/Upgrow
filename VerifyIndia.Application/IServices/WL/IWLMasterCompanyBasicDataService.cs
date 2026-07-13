using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.WL;
using Upgrow.Application.DTO.DropDown;
using Upgrow.Application.DTO.WL;
using Upgrow.Application.DTOs;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;

namespace Upgrow.Application.IServices.WL
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
