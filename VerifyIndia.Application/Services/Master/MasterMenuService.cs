using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTOs.Master;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories.Master;

namespace Upgrow.Application.Services.Master
{
    public class MasterMenuService : MasterServiceBase<Master_Menu, MasterMenuDto, MasterMenuCommand>, IMasterMenuService
    {
        private readonly IMasterMenuRepository _menuRepository;

        public MasterMenuService(IMasterMenuRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
            _menuRepository = repository;
        }

        public async Task<List<MasterMenuDto>> GetAllActiveAsync()
        {
            var entities = await _menuRepository.GetAllActiveAsync();
            // Map minimum fields needed for rendering/hierarchy
            return entities.Select(x => new MasterMenuDto
            {
                UUID = x.UUID,
                MenuName = x.MenuName,
                Url = x.Url,
                MenuLevel = x.MenuLevel,
                MainParentUUID = x.MainParentUUID,
                SubParentUUID = x.MainParentUUID,
                Sequence = x.Sequence,
                IsParent = x.IsParent,
                IsActive = x.IsActive,
                // Ensure entity contains PermissionUUID
                PermissionUUID = x.PermissionUUID
            }).ToList();
        }

        protected override async Task<bool> IsDuplicateAsync(MasterMenuCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.MenuName!.ToLower().Trim() == command.MenuName!.ToLower().Trim() &&
                x.MainParentUUID == command.MainParentUUID && x.MainParentUUID == command.SubParentUUID &&
                x.UUID != command.UUID);
        }

        protected override Expression<Func<Master_Menu, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.MenuName != null && x.MenuName.ToLower().Contains(searchTerm)) ||
                (x.Url != null && x.Url.ToLower().Contains(searchTerm));
        }             

        protected override Func<IQueryable<Master_Menu>, IOrderedQueryable<Master_Menu>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.MenuName) : q.OrderByDescending(x => x.MenuName),
                "url" => q => isAsc ? q.OrderBy(x => x.Url) : q.OrderByDescending(x => x.Url),
                "level" => q => isAsc ? q.OrderBy(x => x.MenuLevel) : q.OrderByDescending(x => x.MenuLevel),
                "isparent" => q => isAsc ? q.OrderBy(x => x.IsParent) : q.OrderByDescending(x => x.IsParent),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        public async Task<List<MasterMenuDto>> GetMainParentsAsync()
        {
            var mainParentList = await _menuRepository.GetMainParentAsync();

            return mainParentList
                // keep only active first-level parents (defensive)
                .Where(s => s.IsActive && (s.MenuLevel ?? 0) == 1)
                .Select(s => new MasterMenuDto
                {
                    UUID = s.UUID!,
                    MenuName = s.MenuName!,
                    MenuIcon = s.MenuIcon?.Trim(),
                    MenuLevel = s.MenuLevel,
                    IsActive = s.IsActive,
                    Url = s.Url,
                    Sequence = s.Sequence,
                    IsParent = s.IsParent,
                    MainParentUUID = s.MainParentUUID,
                    SubParentUUID = s.MainParentUUID,
                    PermissionUUID = s.PermissionUUID
                })
                .OrderBy(m => m.Sequence ?? 0)
                .ToList();
        }

        public async Task<List<MasterMenuDto>> GetSubParentsAsync(string mainParentUuid)
        {
            if (string.IsNullOrEmpty(mainParentUuid))
                return new List<MasterMenuDto>();

            var entities = await _menuRepository.GetSubParentAsync(mainParentUuid);

            return entities
                .Where(s => s.IsActive)
                .Select(x => new MasterMenuDto
                {
                    UUID = x.UUID!,
                    MenuName = x.MenuName!,
                    Url = x.Url,               // include Url for links if needed
                    MenuIcon = x.MenuIcon?.Trim(),     // optional: sub-items rarely need icons
                    MenuLevel = x.MenuLevel,
                    IsActive = x.IsActive,
                    Sequence = x.Sequence,
                    IsParent = x.IsParent,
                    MainParentUUID = x.MainParentUUID,
                    SubParentUUID = x.MainParentUUID,
                    PermissionUUID = x.PermissionUUID
                })
                .OrderBy(m => m.Sequence ?? 0)
                .ToList();
        }
    }
}
