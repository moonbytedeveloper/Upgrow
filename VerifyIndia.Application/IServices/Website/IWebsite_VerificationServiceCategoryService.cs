using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.Website;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTO.Website;
using VerifyIndia.Application.IServices.Master;

namespace VerifyIndia.Application.IServices.Website
{
    public interface IWebsite_VerificationServiceCategoryService : IMasterService<Website_VerificationServiceCategoryDto, Website_VerificationServiceCategoryCommand>
    {
    }
}
