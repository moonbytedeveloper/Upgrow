using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Master
{
    public class WLMasterPathService : MasterServiceBase<WL_Master_Path, WLMasterPathDto, WLMasterPathCommand>, IWLMasterPathService
    {
        public WLMasterPathService(IMasterRepository<WL_Master_Path> repository, IMapper mapper)
            : base(repository, mapper) { }

        protected override async Task<bool> IsDuplicateAsync(WLMasterPathCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Path!.ToLower().Trim() == command.Path.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        protected override Expression<Func<WL_Master_Path, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x => x.Path != null && x.Path.ToLower().Contains(searchTerm);
        }

        protected override Func<IQueryable<WL_Master_Path>, IOrderedQueryable<WL_Master_Path>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "path" => q => isAsc ? q.OrderBy(x => x.Path) : q.OrderByDescending(x => x.Path),
                _ => q => q.OrderByDescending(x => x.Id)
            };
        }

        public async Task<List<WLMasterPathDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<WLMasterPathDto>>(entities);
        }
    }
}