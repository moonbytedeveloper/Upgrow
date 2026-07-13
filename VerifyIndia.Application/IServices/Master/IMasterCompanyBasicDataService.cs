using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.IServices.Master
{
    public interface IMasterCompanyBasicDataService : IMasterService<MasterCompanyBasicDataDto, MasterCompanyBasicDataCommand>
    {
        Task<MasterCompanyBasicDataDto?> GetFirstAsync();
    }
}
