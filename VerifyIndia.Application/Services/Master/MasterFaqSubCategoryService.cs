using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Entities.Auth;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Master
{
    public class MasterFaqSubCategoryService : MasterServiceBase<Master_FAQSubCategory, MasterFaqSubCategoryDto, MasterFaqSubCategoryCommand>, IMasterFaqSubCategoryService

    {
        public MasterFaqSubCategoryService(IMasterRepository<Master_FAQSubCategory> repository, IMapper mapper)
        : base(repository, mapper) { }

        public async Task<List<MasterFaqSubCategoryDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<MasterFaqSubCategoryDto>>(entities);
        }

        protected override Expression<Func<Master_FAQSubCategory, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm) ||
                (x.FAQCategoryUUID != null && x.FAQCategoryUUID.ToLower().Contains(searchTerm)));
        }

        protected override async Task<bool> IsDuplicateAsync(MasterFaqSubCategoryCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
                x.UUID != command.UUID);
        }
        protected override Func<IQueryable<Master_FAQSubCategory>, IOrderedQueryable<Master_FAQSubCategory>>? BuildSortExpression(
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

 
