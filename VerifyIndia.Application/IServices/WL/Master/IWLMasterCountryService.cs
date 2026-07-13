using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO.Master;

namespace Upgrow.Application.IServices.Master
{
    public interface IWLMasterCountryService : IMasterService<WLMasterCountryDto, WLMasterCountryCommand>
    {
    }
}
