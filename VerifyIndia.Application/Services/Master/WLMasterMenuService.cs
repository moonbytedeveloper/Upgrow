using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTOs.Master;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories.Master;

namespace VerifyIndia.Application.Services.Master
{
    public class WLMasterMenuService : MasterServiceBase<WL_MasterMenu, WLMasterMenuDto, WLMasterMenuCommand>, IWLMasterMenuService
    {
        private readonly IWLMasterMenuRepository _menuRepository;

        public WLMasterMenuService(IWLMasterMenuRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
            _menuRepository = repository;
        }

        public async Task<List<WLMasterMenuDto>> GetAllActiveAsync()
        {
            var entities = await _menuRepository.GetAllActiveAsync();
            // Map minimum fields needed for rendering/hierarchy
            return entities.Select(x => new WLMasterMenuDto
            {
                UUID = x.UUID,
                MenuName = x.MenuName,
                Url = x.Url,
                MenuLevel = x.MenuLevel,
                MenuIcon = x.MenuIcon?.Trim(),
                MainParentUUID = x.MainParentUUID,
                
                Sequence = x.Sequence,
                IsParent = x.IsParent,
                IsActive = x.IsActive,
                // Ensure entity contains PermissionUUID
                PermissionUUID = x.PermissionUUID
            }).ToList();
        }

        protected override async Task<bool> IsDuplicateAsync(WLMasterMenuCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.MenuName!.ToLower().Trim() == command.MenuName!.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        protected override Expression<Func<WL_MasterMenu, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.MenuName != null && x.MenuName.ToLower().Contains(searchTerm)) ||
                (x.Url != null && x.Url.ToLower().Contains(searchTerm));
        }

        protected override Func<IQueryable<WL_MasterMenu>, IOrderedQueryable<WL_MasterMenu>>? BuildSortExpression(
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

        public async Task<List<WLMasterMenuDto>> GetMainParentsAsync()
        {
            var mainParentList = await _menuRepository.GetMainParentAsync();

            return mainParentList
                // keep only active first-level parents (defensive)
                .Where(s => s.IsActive && (s.MenuLevel ?? 0) == 1)
                .Select(s => new WLMasterMenuDto
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
                   
                    PermissionUUID = s.PermissionUUID
                })
                .OrderBy(m => m.Sequence ?? 0)
                .ToList();
        }

        public async Task<List<WLMasterMenuDto>> GetSubParentsAsync(string mainParentUuid)
        {
            if (string.IsNullOrEmpty(mainParentUuid))
                return new List<WLMasterMenuDto>();

            var entities = await _menuRepository.GetSubParentAsync(mainParentUuid);

            return entities
                .Where(s => s.IsActive)
                .Select(x => new WLMasterMenuDto
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
                   
                    PermissionUUID = x.PermissionUUID
                })
                .OrderBy(m => m.Sequence ?? 0)
                .ToList();
        }
    }
}
