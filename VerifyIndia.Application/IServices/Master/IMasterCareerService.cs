using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.Website;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTO.Website;

namespace VerifyIndia.Application.IServices.Master
{
    public interface IMasterCareerService : IMasterService<MasterCareerDto, MasterCareerCommand>
    {
    }
}
