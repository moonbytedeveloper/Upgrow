using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTOs.Master;

namespace VerifyIndia.Application.IServices.Master
{
    public interface IMasterMenuService : IMasterService<MasterMenuDto, MasterMenuCommand>
    {
        Task<List<MasterMenuDto>> GetMainParentsAsync();
        Task<List<MasterMenuDto>> GetSubParentsAsync(string mainParentUuid);
        Task<List<MasterMenuDto>> GetAllActiveAsync();
    }
}
