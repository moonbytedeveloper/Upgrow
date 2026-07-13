using AutoMapper;
using System.Linq.Expressions;
using Upgrow.Application.Commands.Website;
using Upgrow.Application.DTO.DropDown;
using Upgrow.Application.DTO.Website;
using Upgrow.Application.DTOs;
using Upgrow.Application.IServices.Master;
using Upgrow.Application.IServices.Website;
using Upgrow.Application.Services.Master;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Website
{
    public class WebsiteIndustryPointsService : MasterServiceBase<Website_IndustryPoints, Website_IndustryPointsDto, Website_IndustryPointsCommand>, IWebsiteIndustryPointsService 
    {
        public WebsiteIndustryPointsService(IMasterRepository<Website_IndustryPoints> repository, IMapper mapper)
         : base(repository, mapper) { }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(Website_IndustryPointsCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<Website_IndustryPoints, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<Website_IndustryPoints>, IOrderedQueryable<Website_IndustryPoints>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                 
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

       
    }
}



