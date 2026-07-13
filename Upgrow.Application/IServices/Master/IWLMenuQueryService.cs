using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTOs.Master;

namespace Upgrow.Application.IServices.Master
{
    public interface IWLMenuQueryService
    {
        Task<List<WLMasterMenuDto>> GetMenusForRoleAsync(string? roleUuid);
        Task<List<WLMasterMenuDto>> GetMenusForRolesAsync(IEnumerable<string?>? roleUuids);
        void InvalidateCache();
    }
}
