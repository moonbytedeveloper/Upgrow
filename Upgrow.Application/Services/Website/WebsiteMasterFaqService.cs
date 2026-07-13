using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.Website;
using Upgrow.Application.DTO.CustomerPanel;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTO.Website;
using Upgrow.Application.IServices.Master;
using Upgrow.Application.IServices.Website;
using Upgrow.Application.Services.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.Entities.Auth;
using Upgrow.Domain.Entities.Website;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Website
{
    public class WebsiteMasterFaqService : MasterServiceBase<Website_MasterFAQ, WebsiteMasterFaqDto, WebsiteMasterFaqCommand>, IWebsiteMasterFaqService
    {
      
        private readonly IMasterRepository<Website_FAQCategory> _categoryRepo;

        public WebsiteMasterFaqService(IMasterRepository<Website_MasterFAQ> repository,
            IMasterRepository<Website_FAQCategory> categoryRepo,
         IMapper mapper)
         : base(repository, mapper)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<List<WebsiteMasterFaqDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<WebsiteMasterFaqDto>>(entities);
        }

        protected override Expression<Func<Website_MasterFAQ, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm)) ||                
                 (x.FAQCategoryUUID != null && x.FAQCategoryUUID.ToLower().Contains(searchTerm));
        }

        protected override async Task<bool> IsDuplicateAsync(WebsiteMasterFaqCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
                x.FAQCategoryUUID!.ToLower().Trim() == command.FAQCategoryUUID.ToLower().Trim() &&
                x.UUID != command.UUID);
        }
        protected override Func<IQueryable<Website_MasterFAQ>, IOrderedQueryable<Website_MasterFAQ>>? BuildSortExpression(
           string? sortColumn,
           string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "title" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                "description" => q => isAsc ? q.OrderBy(x => x.Description) : q.OrderByDescending(x => x.Description),
                "category" => q => isAsc ? q.OrderBy(x => x.FAQCategoryUUID) : q.OrderByDescending(x => x.FAQCategoryUUID),

                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

    }
}
   
