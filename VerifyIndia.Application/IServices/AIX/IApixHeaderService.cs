using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.AIX;
using VerifyIndia.Application.DTO.AIX;
using VerifyIndia.Application.IServices.Master;

namespace VerifyIndia.Application.IServices.AIX
{
    public interface IApixHeaderService : IMasterService<ApixHeaderDto, ApixHeaderCommand>
    {
        Task<List<ApixHeaderDto>> GetAllActiveAsync();
        Task<List<ApixHeaderDto>> GetAllAsync();
        Task<ApixHeaderDto> SaveAndReturnAsync(ApixHeaderCommand command, string userUuid, string ip);
        Task<bool> CheckDuplicateAsync(ApixHeaderCommand command);
    }
}

    
