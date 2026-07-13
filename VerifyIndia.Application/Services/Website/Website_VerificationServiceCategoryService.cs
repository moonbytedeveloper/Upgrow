using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.Website;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTO.Website;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Application.IServices.Website;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Entities.Website;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Website
{
    public class Website_VerificationServiceCategoryService : MasterServiceBase<Website_VerificationServiceCategory, Website_VerificationServiceCategoryDto, Website_VerificationServiceCategoryCommand>, IWebsite_VerificationServiceCategoryService
    {
        public Website_VerificationServiceCategoryService(IMasterRepository<Website_VerificationServiceCategory> repository, IMapper mapper)
         : base(repository, mapper) { }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(Website_VerificationServiceCategoryCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<Website_VerificationServiceCategory, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<Website_VerificationServiceCategory>, IOrderedQueryable<Website_VerificationServiceCategory>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "title" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }




    }
}
    
