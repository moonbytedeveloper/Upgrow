using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Website;
using Upgrow.Application.DTO.Website;
using Upgrow.Application.IServices.Master;

namespace Upgrow.Application.IServices.Website
{
    public interface IWebsiteIndustryService : IMasterService<Website_MasterIndustryDto, Website_MasterIndustryCommand>
    {
    }
}
