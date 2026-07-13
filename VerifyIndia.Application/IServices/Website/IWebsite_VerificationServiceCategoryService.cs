using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.Website;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTO.Website;
using Upgrow.Application.IServices.Master;

namespace Upgrow.Application.IServices.Website
{
    public interface IWebsite_VerificationServiceCategoryService : IMasterService<Website_VerificationServiceCategoryDto, Website_VerificationServiceCategoryCommand>
    {
    }
}
