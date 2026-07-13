using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.WL.Master;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTO.WL.Master;

namespace Upgrow.Application.IServices.Master
{
    public interface IWLMasterRoleService : IMasterService<WLMasterRolesDto, WLMasterRolesCommand>
    {
        Task<List<WLMasterRolesDto>> GetAllActiveAsync();
    }
}
