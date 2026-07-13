using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.Website;
using VerifyIndia.Application.DTO.CustomerPanel;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTO.Website;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Application.IServices.Website;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities.Auth;
using VerifyIndia.Domain.Entities.Website;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Website
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

   