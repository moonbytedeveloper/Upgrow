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
    public interface IWLMasterMenuService : IMasterService<WLMasterMenuDto, WLMasterMenuCommand>
    {
        Task<List<WLMasterMenuDto>> GetMainParentsAsync();
        Task<List<WLMasterMenuDto>> GetSubParentsAsync(string mainParentUuid);
        Task<List<WLMasterMenuDto>> GetAllActiveAsync();
    }
}
