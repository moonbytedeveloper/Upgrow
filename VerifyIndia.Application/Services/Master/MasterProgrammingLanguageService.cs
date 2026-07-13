using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO.AIX;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Master
{
    public class MasterProgrammingLanguageService : MasterServiceBase<Master_ProgrammingLanguage, MasterProgrammingLanguageDto, MasterProgrammingLanguageCommand>, IMasterProgrammingLanguageService
    {
        public MasterProgrammingLanguageService(IMasterRepository<Master_ProgrammingLanguage> repository, IMapper mapper) : base(repository, mapper)
        {
        }
        public async Task<List<MasterProgrammingLanguageDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<MasterProgrammingLanguageDto>>(entities);
        }
        protected override async Task<bool> IsDuplicateAsync(MasterProgrammingLanguageCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<Master_ProgrammingLanguage, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<Master_ProgrammingLanguage>, IOrderedQueryable<Master_ProgrammingLanguage>>? BuildSortExpression(
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
