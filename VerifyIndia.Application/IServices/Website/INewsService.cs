using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.Website;
using VerifyIndia.Application.DTO;
using VerifyIndia.Application.DTO.CustomerPanel;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTOs;
using VerifyIndia.Domain.Common;

namespace VerifyIndia.Application.IServices.Master
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

