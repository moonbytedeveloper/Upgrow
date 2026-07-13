using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.WL;
using VerifyIndia.Application.DTO.DropDown;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTO.WL;
using VerifyIndia.Application.DTOs;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Common;

namespace VerifyIndia.Application.IServices.WL
{
    public interface IWLSocialMediaService : IMasterService<WLSocialMediaDto, WLSocialMediaCommand>
    {
        Task<List<MasterDropDownDto>> GetTenantDropdownAsync();
        Task<PagedResult<WLSocialMediaDto>> GetPagedByTenantAsync(DataTableRequest request, string? tenantUuid);

    }
}
