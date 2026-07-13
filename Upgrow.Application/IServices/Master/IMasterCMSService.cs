using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTOs.Master;

namespace Upgrow.Application.IServices.Master
{
    public interface IMasterCMSService : IMasterService<MasterCMSDto, MasterCMSCommand>
    {
        Task<List<MasterCMSDto>> GetAllActiveAsync();
        Task<MasterCMSDto?> GetByCodeAsync(string code);

    }
}
