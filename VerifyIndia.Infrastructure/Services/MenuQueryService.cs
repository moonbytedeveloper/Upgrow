using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTOs.Master;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Application.IServices.Menu;
using VerifyIndia.Domain.IRepositories.Master;


namespace VerifyIndia.Infrastructure.Services
{
    public sealed class MenuQueryService : IMenuQueryService
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly IMasterMenuRepository _repository;
        private readonly IMenuRolePermissionService _rolePermissionService;
        private readonly IMasterPermissionService _permissionService;
        private readonly IMasterRoleService _menuRoleService;

        private const string CACHE_KEY = "MASTER_MENU_RIGHTS";
        private const string PUBLIC_KEY = "PUBLIC";

        public MenuQueryService(
            AppDbContext context,
            IMemoryCache cache,
            IMasterMenuRepository repository,
            IMenuRolePermissionService rolePermissionService,
            IMasterPermissionService permissionService,
             IMasterRoleService menuRoleService)
        {
            _context = context;
            _cache = cache;
            _repository = repository;
            _rolePermissionService = rolePermissionService;
            _permissionService = permissionService;
            _permissionService = permissionService;
            _menuRoleService = menuRoleService;
        }

        public void InvalidateCache()
        {
            _cache.Remove(CACHE_KEY);
        }

        // Backwards-compatible single-role method
        public Task<List<MasterMenuDto>> GetMenusForRoleAsync(string? roleUuid)
            => GetMenusForRolesAsync(roleUuid == null ? null : new[] { roleUuid });

        // New: accept multiple role UUIDs (handles public menus as well)
        public async Task<List<MasterMenuDto>> GetMenusForRolesAsync(IEnumerable<string?>? roleUuids)
        {
            var data = await GetCachedMenuRightsAsync();

            HashSet<string>? roleSet = null;
            if (roleUuids != null)
            {
                roleSet = new HashSet<string>(
                    roleUuids
                        .Where(r => !string.IsNullOrWhiteSpace(r))
                        .Select(r => r!.Trim()),
                    StringComparer.OrdinalIgnoreCase);
            }

            var menus = data
                .Where(x =>
                    x.UserTypeUUID == PUBLIC_KEY ||
                    (roleSet != null && roleSet.Contains(x.UserTypeUUID)))
                .Select(x => x.Menu)
                .DistinctBy(m => m.UUID)
                .ToList();

            return BuildTree(menus);
        }

        // --------------- PRIVATE ---------------

        private async Task<List<MenuRightCacheModel>> GetCachedMenuRightsAsync()
        {
            if (_cache.TryGetValue(CACHE_KEY, out List<MenuRightCacheModel>? cached) && cached != null)
                return cached;

            // Load menus (only active)
            var menus = await _repository.GetAllActiveAsync();

            // Load all active role-permission mappings
            var rolePerms = await _rolePermissionService.GetAllActiveAsync();

            // Load active permissions so we can ignore menus referencing inactive permissions
            var activePermissions = await _permissionService.GetAllActiveAsync();
            var activePermissionSet = new HashSet<string?>(
                activePermissions
                    .Where(p => !string.IsNullOrWhiteSpace(p.UUID))
                    .Select(p => p.UUID!.Trim()),
                StringComparer.OrdinalIgnoreCase);
            // Load active roles so we ignore mappings for roles that are inactive
            var activeRoles = await _menuRoleService.GetAllActiveAsync();
            var activeRoleSet = new HashSet<string?>(
                activeRoles
                    .Where(r => !string.IsNullOrWhiteSpace(r.UUID))
                    .Select(r => r.UUID!.Trim()),
                StringComparer.OrdinalIgnoreCase);

            var mapped = new List<MenuRightCacheModel>(capacity: menus.Count);
            foreach (var m in menus)
            {
                // if the menu references a permission that is not active, skip mapping it entirely
                if (!string.IsNullOrWhiteSpace(m.PermissionUUID) &&
                    !activePermissionSet.Contains(m.PermissionUUID.Trim()))
                {
                    // Skip this menu — its required permission is inactive
                    continue;
                }

                // build menu projection
                var menu = new Menu
                {
                    UUID = m.UUID ?? string.Empty,
                    Name = m.MenuName ?? string.Empty,
                    Icon = m.MenuIcon,
                    Url = m.Url,
                    Sequence = (int?)(m.Sequence ?? 0) ?? 0,
                    ParentUUID = m.MenuLevel == 1
                ? null
                : m.MainParentUUID
                };


                if (string.IsNullOrWhiteSpace(m.PermissionUUID))
                {
                    // public menu (no permission required)
                    mapped.Add(new MenuRightCacheModel
                    {
                        UserTypeUUID = PUBLIC_KEY,
                        Menu = menu
                    });
                }
                else
                {
                    // match role-permission rows where permission matches this menu's PermissionUUID
                    var matches = rolePerms
                        .Where(rp => !string.IsNullOrEmpty(rp.PermissionUUID)
                                     && string.Equals(rp.PermissionUUID?.Trim(), m.PermissionUUID?.Trim(), StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    foreach (var rp in matches)
                    {
                        // rp should contain the role identifier (Role_UUID); skip if missing
                        var roleUuid = (rp.RoleUUID ?? string.Empty).Trim();
                        if (string.IsNullOrEmpty(roleUuid))
                            continue;

                        // Only map if the role itself is active
                        if (!activeRoleSet.Contains(roleUuid))
                            continue;

                        mapped.Add(new MenuRightCacheModel
                        {
                            UserTypeUUID = roleUuid,
                            Menu = menu
                        });
                    }
                }
            }

            // cache result
            _cache.Set(
                CACHE_KEY,
                mapped,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12),
                    Priority = Microsoft.Extensions.Caching.Memory.CacheItemPriority.High
                });

            return mapped;
        }

        private static List<MasterMenuDto> BuildTree(List<Menu> menus)
        {
            var lookup = menus.ToLookup(x => x.ParentUUID);

            List<MasterMenuDto> Build(string? parentUUID)
            {
                return lookup[parentUUID]
                    .OrderBy(x => x.Sequence)
                    .Select(x => new MasterMenuDto
                    {
                        UUID = x.UUID,
                        MenuName = x.Name,
                        Url = x.Url,
                        Sequence = x.Sequence,
                        MenuIcon = x.Icon,
                        Children = Build(x.UUID)
                    })
                    .ToList();
            }

            return Build(null);
        }

        // internal small models
        private sealed class MenuRightCacheModel
        {
            public string UserTypeUUID { get; set; } = default!;
            public Menu Menu { get; set; } = default!;
        }

        private sealed class Menu
        {
            public string UUID { get; set; } = default!;
            public string Name { get; set; } = default!;
            public string? Icon { get; set; }
            public string? Url { get; set; }
            public int Sequence { get; set; }
            public string? ParentUUID { get; set; }
        }
    }
}
