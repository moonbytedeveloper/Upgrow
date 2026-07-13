using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Api;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO.Api;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.IServices.Master;

namespace Upgrow.Application.IServices.Api
{
    public interface IApiInfoSectionService : IMasterService<ApiInfoSectionDto, ApiInfoSectionCommand>
    {
    }
}
