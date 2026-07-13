using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTOs.Master;

namespace VerifyIndia.Application.IServices.Master
{
    public interface IWLMenuQueryService
    {
        Task<List<WLMasterMenuDto>> GetMenusForRoleAsync(string? roleUuid);
        Task<List<WLMasterMenuDto>> GetMenusForRolesAsync(IEnumerable<string?>? roleUuids);
        void InvalidateCache();
    }
}
