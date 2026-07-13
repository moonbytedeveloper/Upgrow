using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO.Api;
using Upgrow.Application.DTO.Master;

namespace Upgrow.Application.IServices.Master
{
    public interface IApiXCategoryService : IMasterService<ApiXCategoryDto, ApiXCategoryCommand>
    {
        Task<List<ApiXCategoryDto>> GetAllActiveAsync();
        Task<List<ApiXCategoryDto>> GetByCategoryUUIDAsync(string categoryUUID);
    }
}
