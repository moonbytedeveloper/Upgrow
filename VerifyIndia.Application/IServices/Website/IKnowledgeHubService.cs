using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Website;
using VerifyIndia.Application.DTO.CustomerPanel;
using VerifyIndia.Application.DTO.Website;
using VerifyIndia.Application.DTOs;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Common;

namespace VerifyIndia.Application.IServices.Website
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
