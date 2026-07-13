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
using VerifyIndia.Domain.Entities.Auth;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Master
{
    public class MasterFaqCategoryService : MasterServiceBase<Master_FAQCategory, MasterFaqCategoryDto, MasterFaqCategoryCommand>, IMasterFaqCategoryService
    {
        public MasterFaqCategoryService(IMasterRepository<Master_FAQCategory> repository, IMapper mapper)
        : base(repository, mapper) { }

        public async Task<List<MasterFaqCategoryDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<MasterFaqCategoryDto>>(entities);
        }
        protected override Expression<Func<Master_FAQCategory, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm) ||
                 (x.ShortDescription != null && x.ShortDescription.ToLower().Contains(searchTerm)));
        }

        protected override async Task<bool> IsDuplicateAsync(MasterFaqCategoryCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
                x.UUID != command.UUID);
        }
        protected override Func<IQueryable<Master_FAQCategory>, IOrderedQueryable<Master_FAQCategory>>? BuildSortExpression(
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

        public async Task<List<CategoryApiDto>> GetAllActiveForApiAsync()
        {
            var entities = await _repository.GetAllActiveAsync();

            // Project to API DTO. If entity exposes Icon/ShortDescription later, map them here.
            return entities.Select(e => new CategoryApiDto
            {
                UUID = e.UUID,
                Title = e.Title,
                Icon = e.Icon,
                ShortDescription = e.ShortDescription
            }).ToList();
        }

    }
}

    
