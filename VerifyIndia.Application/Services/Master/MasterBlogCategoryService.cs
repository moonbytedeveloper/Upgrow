using AutoMapper;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTOs.Master;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;
using System.Linq.Expressions;

namespace VerifyIndia.Application.Services.Master
{
    public class MasterBlogCategoryService : MasterServiceBase<Master_BlogCategory, MasterBlogCategoryDto, MasterBlogCategoryCommand>, IMasterBlogCategoryService
    {
        public MasterBlogCategoryService(IMasterRepository<Master_BlogCategory> repository, IMapper mapper)
            : base(repository, mapper) { }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(MasterBlogCategoryCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<Master_BlogCategory, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm)) ||
                (x.Description != null && x.Description.ToLower().Contains(searchTerm));
        }

        // Define sorting
        protected override Func<IQueryable<Master_BlogCategory>, IOrderedQueryable<Master_BlogCategory>>? BuildSortExpression(
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