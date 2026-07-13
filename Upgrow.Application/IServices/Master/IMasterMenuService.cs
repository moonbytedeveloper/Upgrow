using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTOs.Master;

namespace Upgrow.Application.IServices.Master
{
    public interface IMasterMenuService : IMasterService<MasterMenuDto, MasterMenuCommand>
    {
        Task<List<MasterMenuDto>> GetMainParentsAsync();
        Task<List<MasterMenuDto>> GetSubParentsAsync(string mainParentUuid);
        Task<List<MasterMenuDto>> GetAllActiveAsync();
    }
}
