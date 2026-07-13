using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.Website;
using Upgrow.Application.DTO;
using Upgrow.Application.DTO.CustomerPanel;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTOs;
using Upgrow.Domain.Common;

namespace Upgrow.Application.IServices.Master
{
    public interface INewsService : IMasterService<NewsDto, NewsCommand>
    {
        Task<List<NewsApiDto>> GetByCategoryAsync(string? categoryUuid);
 
        Task SaveAsync(string newsUuid, string customerUuid);
        Task<List<NewsDetailDto>> GetSavedNewsAsync(string customerUuid, string? categoryUuid = null);
        Task<PagedResult<NewsDto>> GetByCategoryPagedAsync(string? categoryUuid, DataTableRequest request);

        Task<PagedResult<NewsDto>> GetByTopStoriesPagedAsync(string? categoryUuid, DataTableRequest request);
        Task<PagedResult<NewsDto>> GetSavedNewsPagedAsync(string customerUuid, string? categoryUuid, DataTableRequest request);


    }
}

