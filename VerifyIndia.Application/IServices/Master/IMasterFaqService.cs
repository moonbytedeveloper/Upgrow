using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO.Api;
using VerifyIndia.Application.DTO.CustomerPanel;
using VerifyIndia.Application.DTO.Master;

namespace VerifyIndia.Application.IServices.Master
{
    public interface IMasterFaqService : IMasterService<MasterFaqDto, MasterFaqCommand>
    {
        Task<List<MasterFaqDto>> GetAllActiveAsync();
        //Task<List<MasterFaqApiDto>> GetByCategoryUuidAsync(string categoryUuid);
        //Task<List<FaqDetailDto>> GetDetailsByCategoryUuidAsync(string categoryUuid);
        Task<List<SubcategoryFaqsDto>> GetFaqsGroupedByCategoryUuidAsync(string categoryUuid);

    }
}
    
