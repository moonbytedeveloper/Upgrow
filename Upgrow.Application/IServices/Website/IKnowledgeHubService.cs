using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Website;
using Upgrow.Application.DTO.CustomerPanel;
using Upgrow.Application.DTO.Website;
using Upgrow.Application.DTOs;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Common;

namespace Upgrow.Application.IServices.Website
{
    public interface IKnowledgeHubService : IMasterService<KnowledgeHubDto, KnowledgeHubCommand>
    {
        Task<List<KnowledgeHubDto>> GetAllAsync();
        Task<List<KnowledgeHubApiDto>> GetByCategoryAsync(string categoryUuid);

        Task<PagedResult<KnowledgeHubApiDto>> GetByCategoryPagedAsync(
    string categoryUuid,
    DataTableRequest request);

    }
}
