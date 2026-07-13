using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.IServices.Master
{
    public interface IMasterCompanyBasicDataService : IMasterService<MasterCompanyBasicDataDto, MasterCompanyBasicDataCommand>
    {
        Task<MasterCompanyBasicDataDto?> GetFirstAsync();
    }
}
