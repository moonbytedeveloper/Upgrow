using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Website;
using Upgrow.Application.DTO.Website;
using Upgrow.Application.IServices.Master;
using Upgrow.Application.IServices.Website;
using Upgrow.Application.Services.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Website
{
    public class WebsiteIndustryService : MasterServiceBase<Website_MasterIndustry, Website_MasterIndustryDto, Website_MasterIndustryCommand>, IWebsiteIndustryService
    {
        public WebsiteIndustryService(IMasterRepository<Website_MasterIndustry> repository, IMapper mapper)
         : base(repository, mapper) { }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(Website_MasterIndustryCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<Website_MasterIndustry, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm)) ||
                (x.Description != null && x.Description.ToLower().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<Website_MasterIndustry>, IOrderedQueryable<Website_MasterIndustry>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                "description" => q => isAsc ? q.OrderBy(x => x.Description) : q.OrderByDescending(x => x.Description),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

    }
}



