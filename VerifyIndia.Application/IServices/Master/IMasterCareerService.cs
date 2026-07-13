using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.Website;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTO.Website;

namespace Upgrow.Application.IServices.Master
{
    public interface IMasterCareerService : IMasterService<MasterCareerDto, MasterCareerCommand>
    {
    }
}
