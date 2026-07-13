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
using Upgrow.Domain.Entities.Auth;
using Upgrow.Domain.Entities.Website;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Website
{
    public class WebsiteFaqCategoryService : MasterServiceBase<Website_FAQCategory, WebsiteFaqCategoryDto, WebsiteFaqCategoryCommand>, IWebsiteFaqCategoryService
    {
        public WebsiteFaqCategoryService(IMasterRepository<Website_FAQCategory> repository, IMapper mapper)
        : base(repository, mapper) { }

        
        protected override Expression<Func<Website_FAQCategory, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm) ||
                 (x.ShortDescription != null && x.ShortDescription.ToLower().Contains(searchTerm)));
        }

        protected override async Task<bool> IsDuplicateAsync(WebsiteFaqCategoryCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
                x.UUID != command.UUID);
        }
        protected override Func<IQueryable<Website_FAQCategory>, IOrderedQueryable<Website_FAQCategory>>? BuildSortExpression(
           string? sortColumn,
           string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "title" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                "shortdescription" => q => isAsc ? q.OrderBy(x => x.ShortDescription) : q.OrderByDescending(x => x.ShortDescription),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }


    }
}

   