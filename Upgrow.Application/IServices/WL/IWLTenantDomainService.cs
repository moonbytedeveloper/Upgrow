using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.WL;
using Upgrow.Application.DTO.WL;
using Upgrow.Application.IServices.Master;

namespace Upgrow.Application.IServices.WL
{
    public interface IWLTenantDomainService : IMasterService<WLTenantDomainDto, WLTenantDomainCommand>
    
    {

    }
}
