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
    public interface IWLMasterMenuService : IMasterService<WLMasterMenuDto, WLMasterMenuCommand>
    {
        Task<List<WLMasterMenuDto>> GetMainParentsAsync();
        Task<List<WLMasterMenuDto>> GetSubParentsAsync(string mainParentUuid);
        Task<List<WLMasterMenuDto>> GetAllActiveAsync();
    }
}
