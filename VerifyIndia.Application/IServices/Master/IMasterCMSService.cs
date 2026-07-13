using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTOs.Master;

namespace VerifyIndia.Application.IServices.Master
{
    public interface IMasterCMSService : IMasterService<MasterCMSDto, MasterCMSCommand>
    {
        Task<List<MasterCMSDto>> GetAllActiveAsync();
        Task<MasterCMSDto?> GetByCodeAsync(string code);

    }
}
