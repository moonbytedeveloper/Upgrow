using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO.CustomerPanel;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Master
{
    public class MasterApiCategoryService : MasterServiceBase<Api_Category, MasterApiCategoryDto, MasterApiCategoryCommand>, IMasterApiCategoryService
    {
        public MasterApiCategoryService(IMasterRepository<Api_Category> repository, IMapper mapper)
            : base(repository, mapper) { }

        public async Task<List<MasterApiCategoryDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<MasterApiCategoryDto>>(entities);
        }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(MasterApiCategoryCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.CategoryName!.ToLower().Trim() == command.CategoryName.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<Api_Category, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.CategoryName != null && x.CategoryName.ToLower().Contains(searchTerm)) ||
               (x.SequenceNo != null && Convert.ToString(x.SequenceNo).ToLower().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<Api_Category>, IOrderedQueryable<Api_Category>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "categoryname" => q => isAsc ? q.OrderBy(x => x.CategoryName) : q.OrderByDescending(x => x.CategoryName),
                "sequenceno" => q => isAsc ? q.OrderBy(x => x.SequenceNo) : q.OrderByDescending(x => x.SequenceNo),
                "icon" => q => isAsc ? q.OrderBy(x => x.Icon) : q.OrderByDescending(x => x.Icon),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
    }
}
