using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.AIX;
using Upgrow.Application.DTO.AIX;
using Upgrow.Application.IServices.Master;

namespace Upgrow.Application.IServices.AIX
{
    public interface IApixHeaderService : IMasterService<ApixHeaderDto, ApixHeaderCommand>
    {
        Task<List<ApixHeaderDto>> GetAllActiveAsync();
        Task<List<ApixHeaderDto>> GetAllAsync();
        Task<ApixHeaderDto> SaveAndReturnAsync(ApixHeaderCommand command, string userUuid, string ip);
        Task<bool> CheckDuplicateAsync(ApixHeaderCommand command);
    }
}

    
