using AutoMapper;
using System.Linq.Expressions;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Master
{
    public class AppSettingsService : MasterServiceBase<AppSetting, AppSettingDto, AppSettingCommand>, IAppSettingsService
    {
        public AppSettingsService(IMasterRepository<AppSetting> repository, IMapper mapper)
         : base(repository, mapper) { }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(AppSettingCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Key!.ToLower().Trim() == command.Key.ToLower().Trim() &&
                x.Value!.ToLower().Trim() == command.Value.ToLower().Trim() &&
                x.Description!.ToLower().Trim() == command.Description.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<AppSetting, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Key != null && x.Key.ToLower().Contains(searchTerm)) ||
                (x.Value != null && x.Value.ToLower().Contains(searchTerm)) ||
                (x.Description != null && x.Description.ToLower().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<AppSetting>, IOrderedQueryable<AppSetting>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "key" => q => isAsc ? q.OrderBy(x => x.Key) : q.OrderByDescending(x => x.Key),
                "value" => q => isAsc ? q.OrderBy(x => x.Value) : q.OrderByDescending(x => x.Value),
                "description" => q => isAsc ? q.OrderBy(x => x.Description) : q.OrderByDescending(x => x.Description),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

    }
}



