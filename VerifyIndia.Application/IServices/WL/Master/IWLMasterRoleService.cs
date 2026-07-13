using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.WL.Master;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTO.WL.Master;

namespace VerifyIndia.Application.IServices.Master
{
    public interface IWLMasterRoleService : IMasterService<WLMasterRolesDto, WLMasterRolesCommand>
    {
        Task<List<WLMasterRolesDto>> GetAllActiveAsync();
    }
}
