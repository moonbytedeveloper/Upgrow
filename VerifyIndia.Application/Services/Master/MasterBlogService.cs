using AutoMapper;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTOs;
using Upgrow.Application.DTOs.Master;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Application.Services.Master
{
    public class MasterBlogService : MasterServiceBase<Master_Blog, MasterBlogDto, MasterBlogCommand>, IMasterBlogService
    {
        private readonly IMasterRepository<Master_BlogCategory> _categoryRepository;

        public MasterBlogService(
            IMasterRepository<Master_Blog> repository,
            IMasterRepository<Master_BlogCategory> categoryRepository,
            IMapper mapper)
            : base(repository, mapper)
        {
            _categoryRepository = categoryRepository;
        }

       // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(MasterBlogCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
                x.UUID != command.UUID);
        }
      
        // Define which fields to search
        protected override Expression<Func<Master_Blog, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm)) ||
                (x.ShortDescription != null && x.ShortDescription.ToLower().Contains(searchTerm));          
        }

        // Define sorting
        protected override Func<IQueryable<Master_Blog>, IOrderedQueryable<Master_Blog>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "title" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                "blogdate" => q => isAsc ? q.OrderBy(x => x.BlogDate) : q.OrderByDescending(x => x.BlogDate),
                "blogcategory" => q => isAsc ? q.OrderBy(x => x.BlogCategoryUUID) : q.OrderByDescending(x => x.BlogCategoryUUID),
                "shortdescription" => q => isAsc ? q.OrderBy(x => x.ShortDescription) : q.OrderByDescending(x => x.ShortDescription),
               
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
    }
}