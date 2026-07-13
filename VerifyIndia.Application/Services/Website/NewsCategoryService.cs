using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.Website;
using VerifyIndia.Application.DTO;
using VerifyIndia.Application.DTO.CustomerPanel;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTO.Website;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Application.IServices.Website;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Website
{
    public class NewsCategoryService : MasterServiceBase<News_Category, NewsCategoryDto, NewsCategoryCommand>, INewsCategoryService
    {
        private readonly IMasterRepository<News> _newsRepository;
        public NewsCategoryService(IMasterRepository<News_Category> repository,
             IMasterRepository<News> newsRepository,
            IMapper mapper)
           : base(repository, mapper) 
        {
            _newsRepository = newsRepository;
        }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(NewsCategoryCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<News_Category, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm)) ||
                (x.ShortDescription != null && x.ShortDescription.ToLower().Contains(searchTerm));
        }

        // Define sorting
        protected override Func<IQueryable<News_Category>, IOrderedQueryable<News_Category>>? BuildSortExpression(
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

        public async Task<List<NewsCategoryApiDto>> GetAllAsync()
        {
            var entities = await _repository.FindAllAsync(x => x.IsActive ==  true);
            return _mapper.Map<List<NewsCategoryApiDto>>(entities);
        }

    }
}
    

