using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTOs.Master;

namespace Upgrow.Application.IServices.Master
{
    public interface IMenuQueryService
    {
        Task<List<MasterMenuDto>> GetMenusForRoleAsync(string? roleUuid);
        Task<List<MasterMenuDto>> GetMenusForRolesAsync(IEnumerable<string?>? roleUuids);
        void InvalidateCache();
    }
}
