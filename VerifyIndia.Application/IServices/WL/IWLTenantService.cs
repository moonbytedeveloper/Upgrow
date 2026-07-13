using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.WL;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTO.WL;
using VerifyIndia.Application.IServices.Master;

namespace VerifyIndia.Application.IServices.WL
{
    public interface IWLTenantService : IMasterService<WLTenantDto, WLTenantCommand>
    {
        Task<List<WLTenantDto>> GetAllActiveAsync();
        Task<List<WLTenantDto>> GetAllAsync();
    }
}
