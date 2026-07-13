using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO.Api;
using Upgrow.Application.DTO.CustomerPanel;
using Upgrow.Application.DTO.Master;

namespace Upgrow.Application.IServices.Master
{
    public interface IMasterFaqService : IMasterService<MasterFaqDto, MasterFaqCommand>
    {
        Task<List<MasterFaqDto>> GetAllActiveAsync();
        //Task<List<MasterFaqApiDto>> GetByCategoryUuidAsync(string categoryUuid);
        //Task<List<FaqDetailDto>> GetDetailsByCategoryUuidAsync(string categoryUuid);
        Task<List<SubcategoryFaqsDto>> GetFaqsGroupedByCategoryUuidAsync(string categoryUuid);

    }
}
    
