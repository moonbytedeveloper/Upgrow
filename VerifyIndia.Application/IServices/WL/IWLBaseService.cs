using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.DropDown;
using Upgrow.Application.DTOs;
using Upgrow.Domain.Common;

namespace Upgrow.Application.IServices.WL
{
    public interface IWLBaseService<TEntity, TDto, TCommand>
    {
        Task<PagedResult<TDto>> GetPagedByTenantAsync(DataTableRequest request, string? tenantUuid);
        Task<List<MasterDropDownDto>> GetTenantDropdownAsync();

    }
}
