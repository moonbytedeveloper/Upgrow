using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Api;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO.Api;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.IServices.Master;

namespace VerifyIndia.Application.IServices.Api
{
    public interface IApiInfoSectionService : IMasterService<ApiInfoSectionDto, ApiInfoSectionCommand>
    {
    }
}
