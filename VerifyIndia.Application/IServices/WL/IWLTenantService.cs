using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.WL;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTO.WL;
using Upgrow.Application.IServices.Master;

namespace Upgrow.Application.IServices.WL
{
    public interface IWLTenantService : IMasterService<WLTenantDto, WLTenantCommand>
    {
        Task<List<WLTenantDto>> GetAllActiveAsync();
        Task<List<WLTenantDto>> GetAllAsync();
    }
}
