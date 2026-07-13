using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.WL.Master;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTO.WL.Master;
using VerifyIndia.Application.IServices.Master;

namespace VerifyIndia.Application.IServices.WL.Master
{
    public interface IWLMasterDepartmentService : IMasterService<WLMasterDepartmentDto, WLMasterDepartmentCommand>
    {
    }
}
