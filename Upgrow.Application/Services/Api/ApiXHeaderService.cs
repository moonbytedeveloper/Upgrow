using AutoMapper;
using System.Linq.Expressions;
using Upgrow.Application.Commands.Api;
using Upgrow.Application.DTO.Api;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Master
{
    public class ApiXHeaderService : MasterServiceBase<ApiXHeaders, ApiXHeaderDto, ApiXHeaderCommand>, IApiXHeaderService
    {
        public ApiXHeaderService(IMasterRepository<ApiXHeaders> repository, IMapper mapper)
         : base(repository, mapper) { }

        // Duplicate check: Title must be unique (case-insensitive, trimmed)
        protected override async Task<bool> IsDuplicateAsync(ApiXHeaderCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.FieldName.ToLower().Trim() == command.FieldName.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        public async Task<List<ApiXHeaderDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<ApiXHeaderDto>>(entities);
        }


        protected override Expression<Func<ApiXHeaders, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.FieldName != null && x.FieldName.ToLower().Contains(searchTerm)) ||
                (x.FieldDetails != null && x.FieldDetails.ToLower().Contains(searchTerm));
        }


        protected override Func<IQueryable<ApiXHeaders>, IOrderedQueryable<ApiXHeaders>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "title" => q => isAsc ? q.OrderBy(x => x.FieldName) : q.OrderByDescending(x => x.FieldName),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        public async Task<List<ApiXHeaderDto>> GetByApiXVersionUUIDAsync(string apiXVersionUUID)
        {
            var entities = await _repository.FindAllAsync(x => x.ApiXVersionUUID == apiXVersionUUID && x.IsActive);
            return _mapper.Map<List<ApiXHeaderDto>>(entities);
        }
    }
}