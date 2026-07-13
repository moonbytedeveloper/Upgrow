using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO.Master;

namespace VerifyIndia.Application.IServices.Master
{
    public interface IMasterDepartmentService : IMasterService<MasterDepartmentDto, MasterDepartmentCommand>
    {

    }
}
